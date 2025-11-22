using Recchx.SharedKernel.Entities;

namespace Recchx.Users.Domain.Entities;

public class UserSession : BaseEntity
{
    public Guid UserId { get; private set; }
    public string SessionId { get; private set; }
    public string JwtId { get; private set; }
    public string DeviceFingerprint { get; private set; }
    public string DeviceInfo { get; private set; }
    public string IpAddress { get; private set; }
    public string UserAgent { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime LastActivityAt { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    
    // Navigation
    public User User { get; private set; }

    private UserSession() { } // EF Core

    public UserSession(
        Guid userId,
        string sessionId,
        string jwtId,
        string deviceFingerprint,
        string deviceInfo,
        string ipAddress,
        string userAgent,
        DateTime expiresAt)
    {
        UserId = userId;
        SessionId = sessionId ?? throw new ArgumentNullException(nameof(sessionId));
        JwtId = jwtId ?? throw new ArgumentNullException(nameof(jwtId));
        DeviceFingerprint = deviceFingerprint ?? throw new ArgumentNullException(nameof(deviceFingerprint));
        DeviceInfo = deviceInfo ?? "Unknown";
        IpAddress = ipAddress ?? "Unknown";
        UserAgent = userAgent ?? "Unknown";
        ExpiresAt = expiresAt;
        LastActivityAt = DateTime.UtcNow;
        IsActive = true;
    }

    public void UpdateActivity()
    {
        LastActivityAt = DateTime.UtcNow;
        UpdateTimestamp();
    }

    public void Revoke()
    {
        IsActive = false;
        RevokedAt = DateTime.UtcNow;
        UpdateTimestamp();
    }

    public bool IsExpired() => DateTime.UtcNow >= ExpiresAt;

    public bool IsValid() => IsActive && !IsExpired();
}
