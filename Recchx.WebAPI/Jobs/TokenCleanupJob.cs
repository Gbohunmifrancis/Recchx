using Hangfire;
using Microsoft.Extensions.Logging;
using Recchx.Users.Infrastructure.Persistence;

namespace Recchx.WebAPI.Jobs;

public class TokenCleanupJob
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserSessionRepository _sessionRepository;
    private readonly ILogger<TokenCleanupJob> _logger;

    public TokenCleanupJob(
        IRefreshTokenRepository refreshTokenRepository,
        IUserSessionRepository sessionRepository,
        ILogger<TokenCleanupJob> logger)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _sessionRepository = sessionRepository;
        _logger = logger;
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task CleanupOldTokensAndSessions()
    {
        try
        {
            _logger.LogInformation("Starting token and session cleanup job...");

            // Delete revoked tokens older than 30 days
            var deletedTokens = await _refreshTokenRepository.DeleteOldTokensAsync(30);
            await _refreshTokenRepository.SaveChangesAsync();

            // Delete expired/revoked sessions older than 7 days
            var deletedSessions = await _sessionRepository.DeleteOldSessionsAsync(7);
            await _sessionRepository.SaveChangesAsync();

            _logger.LogInformation(
                "Token cleanup completed. Deleted {TokenCount} tokens and {SessionCount} sessions.",
                deletedTokens,
                deletedSessions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during token cleanup job");
            throw;
        }
    }
}
