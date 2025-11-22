using Recchx.Users.Domain.Entities;

namespace Recchx.Users.Infrastructure.Persistence;

public interface IUserSessionRepository
{
    Task<UserSession?> GetBySessionIdAsync(string sessionId, CancellationToken cancellationToken = default);
    Task<UserSession?> GetByJwtIdAsync(string jwtId, CancellationToken cancellationToken = default);
    Task<List<UserSession>> GetActiveSessionsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(UserSession session, CancellationToken cancellationToken = default);
    Task UpdateAsync(UserSession session, CancellationToken cancellationToken = default);
    Task RevokeAllUserSessionsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task RevokeSessionAsync(string sessionId, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
