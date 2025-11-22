using MediatR;
using Recchx.SharedKernel.Results;
using Recchx.Users.Application.DTOs;
using Recchx.Users.Application.Services;
using Recchx.Users.Domain.Entities;
using Recchx.Users.Infrastructure.Persistence;

namespace Recchx.Users.Application.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<AuthResponseDto>>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserSessionRepository _sessionRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IDeviceFingerprintService _deviceFingerprintService;

    public RefreshTokenCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IUserSessionRepository sessionRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        IDeviceFingerprintService deviceFingerprintService)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _sessionRepository = sessionRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _deviceFingerprintService = deviceFingerprintService;
    }

    public async Task<Result<AuthResponseDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // Validate refresh token
        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);
        
        if (refreshToken == null)
            return Result.Failure<AuthResponseDto>("Invalid refresh token");

        if (!refreshToken.IsValid())
            return Result.Failure<AuthResponseDto>("Refresh token is expired or revoked");

        // Generate device fingerprint
        var deviceFingerprint = _deviceFingerprintService.GenerateFingerprint(request.IpAddress, request.UserAgent);

        // Optional: Verify device fingerprint matches (for extra security)
        if (refreshToken.DeviceFingerprint != deviceFingerprint)
        {
            // Log suspicious activity - token used from different device
            // For now, we'll allow it but you can make this stricter
        }

        // Generate new tokens
        var sessionId = Guid.NewGuid().ToString();
        var jwtId = Guid.NewGuid().ToString();
        var newAccessToken = _jwtTokenGenerator.GenerateToken(refreshToken.User, sessionId, jwtId, deviceFingerprint);
        var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken();

        // Revoke old refresh token (rotation)
        refreshToken.Revoke();
        await _refreshTokenRepository.UpdateAsync(refreshToken, cancellationToken);

        // Create new refresh token
        var deviceInfo = _deviceFingerprintService.GetDeviceInfo(request.UserAgent);
        var newRefreshTokenEntity = new Domain.Entities.RefreshToken(
            refreshToken.UserId,
            newRefreshToken,
            DateTime.UtcNow.AddDays(7),
            deviceInfo,
            request.IpAddress,
            request.UserAgent,
            deviceFingerprint,
            sessionId
        );

        await _refreshTokenRepository.AddAsync(newRefreshTokenEntity, cancellationToken);

        // Create new session
        var newSession = new UserSession(
            refreshToken.UserId,
            sessionId,
            jwtId,
            deviceFingerprint,
            deviceInfo,
            request.IpAddress,
            request.UserAgent,
            DateTime.UtcNow.AddHours(1)
        );

        await _sessionRepository.AddAsync(newSession, cancellationToken);
        await _sessionRepository.SaveChangesAsync(cancellationToken);
        await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

        var response = new AuthResponseDto
        {
            Token = newAccessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            SessionId = sessionId,
            User = new UserDto
            {
                Id = refreshToken.User.Id,
                Email = refreshToken.User.Email,
                FirstName = refreshToken.User.FirstName,
                LastName = refreshToken.User.LastName,
                CreatedAt = refreshToken.User.CreatedAt
            }
        };

        return Result.Success(response);
    }
}
