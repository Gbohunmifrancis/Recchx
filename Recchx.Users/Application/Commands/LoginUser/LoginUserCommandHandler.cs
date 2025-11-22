using MediatR;
using Recchx.SharedKernel.Results;
using Recchx.SharedKernel.ValueObjects;
using Recchx.Users.Application.DTOs;
using Recchx.Users.Application.Services;
using Recchx.Users.Domain.Entities;
using Recchx.Users.Infrastructure.Persistence;

namespace Recchx.Users.Application.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<AuthResponseDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserSessionRepository _sessionRepository;
    private readonly IDeviceFingerprintService _deviceFingerprintService;

    public LoginUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshTokenRepository refreshTokenRepository,
        IUserSessionRepository sessionRepository,
        IDeviceFingerprintService deviceFingerprintService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenRepository = refreshTokenRepository;
        _sessionRepository = sessionRepository;
        _deviceFingerprintService = deviceFingerprintService;
    }

    public async Task<Result<AuthResponseDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var email = Email.Create(request.Email);
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);

        if (user == null)
        {
            return Result.Failure<AuthResponseDto>("Invalid email or password");
        }

        // Verify password
        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            return Result.Failure<AuthResponseDto>("Invalid email or password");
        }

        // Single session mode: Revoke all existing sessions
        await _sessionRepository.RevokeAllUserSessionsAsync(user.Id, cancellationToken);
        await _refreshTokenRepository.RevokeAllUserTokensAsync(user.Id, cancellationToken);

        // Generate device fingerprint
        var deviceFingerprint = _deviceFingerprintService.GenerateFingerprint(request.IpAddress, request.UserAgent);
        var deviceInfo = _deviceFingerprintService.GetDeviceInfo(request.UserAgent);

        // Generate new tokens
        var sessionId = Guid.NewGuid().ToString();
        var jwtId = Guid.NewGuid().ToString();
        var token = _jwtTokenGenerator.GenerateToken(user, sessionId, jwtId, deviceFingerprint);
        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddHours(1);

        // Create refresh token
        var refreshTokenEntity = new Domain.Entities.RefreshToken(
            user.Id,
            refreshToken,
            DateTime.UtcNow.AddDays(7),
            deviceInfo,
            request.IpAddress,
            request.UserAgent,
            deviceFingerprint,
            sessionId
        );

        await _refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);

        // Create session
        var session = new UserSession(
            user.Id,
            sessionId,
            jwtId,
            deviceFingerprint,
            deviceInfo,
            request.IpAddress,
            request.UserAgent,
            expiresAt
        );

        await _sessionRepository.AddAsync(session, cancellationToken);
        await _sessionRepository.SaveChangesAsync(cancellationToken);
        await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

        var response = new AuthResponseDto
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpiresAt = expiresAt,
            SessionId = sessionId,
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreatedAt = user.CreatedAt
            }
        };

        return Result.Success(response);
    }
}

