using Recchx.Users.Domain.Entities;

namespace Recchx.Users.Infrastructure.Persistence;

public interface IUserProfileRepository
{
    Task<UserProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UserProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<UserProfile> AddAsync(UserProfile profile, CancellationToken cancellationToken = default);
    Task UpdateAsync(UserProfile profile, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

