using Microsoft.EntityFrameworkCore;
using Recchx.Users.Domain.Entities;
using Recchx.Users.Infrastructure.Persistence;
using Recchx.WebAPI.Infrastructure.Persistence;

namespace Recchx.WebAPI.Infrastructure.Repositories;

public class UserSessionRepository : IUserSessionRepository
{
    private readonly RecchxDbContext _context;

    public UserSessionRepository(RecchxDbContext context)
    {
        _context = context;
    }

    public async Task<UserSession?> GetBySessionIdAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        return await _context.UserSessions
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.SessionId == sessionId, cancellationToken);
    }

    public async Task<UserSession?> GetByJwtIdAsync(string jwtId, CancellationToken cancellationToken = default)
    {
        return await _context.UserSessions
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.JwtId == jwtId, cancellationToken);
    }

    public async Task<List<UserSession>> GetActiveSessionsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.UserSessions
            .Where(s => s.UserId == userId && s.IsActive && s.ExpiresAt > DateTime.UtcNow)
            .OrderByDescending(s => s.LastActivityAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(UserSession session, CancellationToken cancellationToken = default)
    {
        await _context.UserSessions.AddAsync(session, cancellationToken);
    }

    public Task UpdateAsync(UserSession session, CancellationToken cancellationToken = default)
    {
        _context.UserSessions.Update(session);
        return Task.CompletedTask;
    }

    public async Task RevokeAllUserSessionsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var sessions = await _context.UserSessions
            .Where(s => s.UserId == userId && s.IsActive)
            .ToListAsync(cancellationToken);

        foreach (var session in sessions)
        {
            session.Revoke();
        }
    }

    public async Task RevokeSessionAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        var session = await GetBySessionIdAsync(sessionId, cancellationToken);
        if (session != null)
        {
            session.Revoke();
        }
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
