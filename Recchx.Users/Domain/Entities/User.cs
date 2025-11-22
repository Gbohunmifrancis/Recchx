using Recchx.SharedKernel.Entities;
using Recchx.SharedKernel.ValueObjects;

namespace Recchx.Users.Domain.Entities;

public class User : BaseEntity
{
    public string? AuthProviderId { get; private set; }
    public Email Email { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string PasswordHash { get; private set; }
    public string? PasswordResetToken { get; private set; }
    public DateTime? PasswordResetTokenExpiry { get; private set; }
    
    public UserProfile? Profile { get; private set; }

    private User() { } // EF Core

    public User(Email email, string firstName, string lastName, string passwordHash)
    {
        Email = email ?? throw new ArgumentNullException(nameof(email));
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
    }

    public void UpdateProfile(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        UpdateTimestamp();
    }

    public void SetPasswordResetToken(string token, DateTime expiry)
    {
        PasswordResetToken = token;
        PasswordResetTokenExpiry = expiry;
        UpdateTimestamp();
    }

    public void ResetPassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        PasswordResetToken = null;
        PasswordResetTokenExpiry = null;
        UpdateTimestamp();
    }

    public bool IsPasswordResetTokenValid(string token)
    {
        return PasswordResetToken == token && 
               PasswordResetTokenExpiry.HasValue && 
               PasswordResetTokenExpiry.Value > DateTime.UtcNow;
    }
}

