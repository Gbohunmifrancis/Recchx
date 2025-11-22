using Microsoft.EntityFrameworkCore;
using Recchx.Users.Domain.Entities;
using Recchx.Users.Infrastructure.Persistence;
using Recchx.WebAPI.Infrastructure.Persistence;

namespace Recchx.WebAPI.Infrastructure.Repositories;

public class UserProfileRepository : IUserProfileRepository
{
    private readonly RecchxDbContext _context;

    public UserProfileRepository(RecchxDbContext context)
    {
        _context = context;
    }

    public async Task<UserProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<UserProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);
    }

    public async Task<UserProfile> AddAsync(UserProfile profile, CancellationToken cancellationToken = default)
    {
        await _context.UserProfiles.AddAsync(profile, cancellationToken);
        return profile;
    }

    public Task UpdateAsync(UserProfile profile, CancellationToken cancellationToken = default)
    {
        _context.UserProfiles.Update(profile);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}

