using Recchx.SharedKernel.Entities;

namespace Recchx.Mailbox.Domain.Entities;

public class MailboxConnection : BaseEntity
{
    public Guid UserId { get; private set; }
    public MailboxProvider Provider { get; private set; }
    public string AccessToken { get; private set; }
    public string RefreshToken { get; private set; }
    public string? Scope { get; private set; }
    public DateTime ConnectedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }

    private MailboxConnection() { } // EF Core

    public MailboxConnection(Guid userId, MailboxProvider provider, string accessToken, 
        string refreshToken, string? scope, DateTime expiresAt)
    {
        UserId = userId;
        Provider = provider;
        AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
        RefreshToken = refreshToken ?? throw new ArgumentNullException(nameof(refreshToken));
        Scope = scope;
        ConnectedAt = DateTime.UtcNow;
        ExpiresAt = expiresAt;
    }

    public void UpdateTokens(string accessToken, string refreshToken, DateTime expiresAt)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        ExpiresAt = expiresAt;
        UpdateTimestamp();
    }

    public bool IsExpired() => DateTime.UtcNow >= ExpiresAt;
}

public enum MailboxProvider
{
    Gmail,
    Outlook
}

