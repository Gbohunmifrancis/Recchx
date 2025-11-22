using Recchx.SharedKernel.Entities;

namespace Recchx.Users.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public Guid UserId { get; private set; }
    public string Token { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsRevoked { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    
    // Device fingerprinting
    public string DeviceInfo { get; private set; }
    public string IpAddress { get; private set; }
    public string UserAgent { get; private set; }
    public string DeviceFingerprint { get; private set; }
    
    // Session tracking
    public string SessionId { get; private set; }
    public DateTime LastActivityAt { get; private set; }
    
    // Navigation
    public User User { get; private set; }

    private RefreshToken() { } // EF Core

    public RefreshToken(
        Guid userId, 
        string token, 
        DateTime expiresAt, 
        string deviceInfo, 
        string ipAddress, 
        string userAgent,
        string deviceFingerprint,
        string sessionId)
    {
        UserId = userId;
        Token = token ?? throw new ArgumentNullException(nameof(token));
        ExpiresAt = expiresAt;
        DeviceInfo = deviceInfo ?? "Unknown";
        IpAddress = ipAddress ?? "Unknown";
        UserAgent = userAgent ?? "Unknown";
        DeviceFingerprint = deviceFingerprint ?? throw new ArgumentNullException(nameof(deviceFingerprint));
        SessionId = sessionId ?? throw new ArgumentNullException(nameof(sessionId));
        LastActivityAt = DateTime.UtcNow;
        IsRevoked = false;
    }

    public void Revoke()
    {
        IsRevoked = true;
        RevokedAt = DateTime.UtcNow;
        UpdateTimestamp();
    }

    public void UpdateActivity()
    {
        LastActivityAt = DateTime.UtcNow;
        UpdateTimestamp();
    }

    public bool IsExpired() => DateTime.UtcNow >= ExpiresAt;

    public bool IsValid() => !IsRevoked && !IsExpired();
}
