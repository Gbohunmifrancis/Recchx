using Recchx.SharedKernel.Entities;

namespace Recchx.Campaigns.Domain.Entities;

public class EmailTemplate : BaseEntity
{
    public Guid UserId { get; private set; }
    public string Name { get; private set; }
    public string Subject { get; private set; }
    public string Body { get; private set; }

    private EmailTemplate() { } // EF Core

    public EmailTemplate(Guid userId, string name, string subject, string body)
    {
        UserId = userId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Subject = subject ?? throw new ArgumentNullException(nameof(subject));
        Body = body ?? throw new ArgumentNullException(nameof(body));
    }

    public void Update(string name, string subject, string body)
    {
        Name = name;
        Subject = subject;
        Body = body;
        UpdateTimestamp();
    }
}

