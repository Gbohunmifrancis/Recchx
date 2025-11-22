using MediatR;
using Recchx.SharedKernel.Results;
using Recchx.Users.Infrastructure.Persistence;

namespace Recchx.Users.Application.Commands.Logout;

public class LogoutAllSessionsCommandHandler : IRequestHandler<LogoutAllSessionsCommand, Result<bool>>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserSessionRepository _sessionRepository;

    public LogoutAllSessionsCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IUserSessionRepository sessionRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _sessionRepository = sessionRepository;
    }

    public async Task<Result<bool>> Handle(LogoutAllSessionsCommand request, CancellationToken cancellationToken)
    {
        // Revoke all sessions
        await _sessionRepository.RevokeAllUserSessionsAsync(request.UserId, cancellationToken);
        await _sessionRepository.SaveChangesAsync(cancellationToken);

        // Revoke all refresh tokens
        await _refreshTokenRepository.RevokeAllUserTokensAsync(request.UserId, cancellationToken);
        await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

        return Result.Success(true);
    }
}
