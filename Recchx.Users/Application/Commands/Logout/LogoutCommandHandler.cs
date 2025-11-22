using MediatR;
using Recchx.SharedKernel.Results;
using Recchx.Users.Infrastructure.Persistence;

namespace Recchx.Users.Application.Commands.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result<bool>>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserSessionRepository _sessionRepository;

    public LogoutCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IUserSessionRepository sessionRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _sessionRepository = sessionRepository;
    }

    public async Task<Result<bool>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        // Revoke the session
        await _sessionRepository.RevokeSessionAsync(request.SessionId, cancellationToken);
        await _sessionRepository.SaveChangesAsync(cancellationToken);

        // Revoke all active refresh tokens for this user (strict single session mode)
        await _refreshTokenRepository.RevokeAllUserTokensAsync(request.UserId, cancellationToken);
        await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

        return Result.Success(true);
    }
}
