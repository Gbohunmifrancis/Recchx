using Recchx.SharedKernel.Entities;
using Recchx.SharedKernel.ValueObjects;

namespace Recchx.Prospects.Domain.Entities;

public class Contact : BaseEntity
{
    public Guid CompanyId { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string? Role { get; private set; }
    public Email Email { get; private set; }
    public bool Verified { get; private set; }
    public DateTime? LastVerifiedAt { get; private set; }
    
    public Company Company { get; private set; } = null!;

    private Contact() { } // EF Core

    public Contact(Guid companyId, string firstName, string lastName, string? role, Email email)
    {
        CompanyId = companyId;
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        Role = role;
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Verified = false;
    }

    public void Update(string firstName, string lastName, string? role)
    {
        FirstName = firstName;
        LastName = lastName;
        Role = role;
        UpdateTimestamp();
    }

    public void MarkAsVerified()
    {
        Verified = true;
        LastVerifiedAt = DateTime.UtcNow;
        UpdateTimestamp();
    }

    public void MarkAsUnverified()
    {
        Verified = false;
        UpdateTimestamp();
    }
}

