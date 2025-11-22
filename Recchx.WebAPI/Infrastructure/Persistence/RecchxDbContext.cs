using Microsoft.EntityFrameworkCore;
using Recchx.Applications.Domain.Entities;
using Recchx.Campaigns.Domain.Entities;
using Recchx.Mailbox.Domain.Entities;
using Recchx.Prospects.Domain.Entities;
using Recchx.Users.Domain.Entities;

namespace Recchx.WebAPI.Infrastructure.Persistence;

public class RecchxDbContext : DbContext
{
    public RecchxDbContext(DbContextOptions<RecchxDbContext> options) : base(options)
    {
    }

    // Users Module
    public DbSet<User> Users => Set<User>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<UserSession> UserSessions => Set<UserSession>();

    // Prospects Module
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Contact> Contacts => Set<Contact>();

    // Mailbox Module
    public DbSet<MailboxConnection> MailboxConnections => Set<MailboxConnection>();

    // Campaigns Module
    public DbSet<EmailTemplate> EmailTemplates => Set<EmailTemplate>();
    public DbSet<Campaign> Campaigns => Set<Campaign>();
    public DbSet<CampaignContact> CampaignContacts => Set<CampaignContact>();

    // Applications Module (Matching)
    public DbSet<UserCompanyMatch> UserCompanyMatches => Set<UserCompanyMatch>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RecchxDbContext).Assembly);

        // Configure schema names (optional - can use default 'public' schema)
        modelBuilder.HasDefaultSchema("public");
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Update timestamps for modified entities
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.Entity is Recchx.SharedKernel.Entities.BaseEntity entity)
            {
                entity.UpdateTimestamp();
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}

