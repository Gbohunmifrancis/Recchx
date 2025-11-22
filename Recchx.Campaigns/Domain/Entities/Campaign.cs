using Recchx.SharedKernel.Entities;

namespace Recchx.Campaigns.Domain.Entities;

public class Campaign : BaseEntity
{
    public Guid UserId { get; private set; }
    public string Name { get; private set; }
    public Guid TemplateId { get; private set; }
    public CampaignStatus Status { get; private set; }
    public DateTime? ScheduledAt { get; private set; }
    public int SentCount { get; private set; }
    public int OpenCount { get; private set; }
    public int ClickCount { get; private set; }
    public int ReplyCount { get; private set; }
    
    public List<CampaignContact> CampaignContacts { get; private set; } = new();

    private Campaign() { } // EF Core

    public Campaign(Guid userId, string name, Guid templateId, DateTime? scheduledAt)
    {
        UserId = userId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        TemplateId = templateId;
        Status = CampaignStatus.Draft;
        ScheduledAt = scheduledAt;
    }

    public void UpdateStatus(CampaignStatus status)
    {
        Status = status;
        UpdateTimestamp();
    }

    public void IncrementSentCount()
    {
        SentCount++;
        UpdateTimestamp();
    }

    public void IncrementOpenCount()
    {
        OpenCount++;
        UpdateTimestamp();
    }

    public void IncrementClickCount()
    {
        ClickCount++;
        UpdateTimestamp();
    }

    public void IncrementReplyCount()
    {
        ReplyCount++;
        UpdateTimestamp();
    }
}

public enum CampaignStatus
{
    Draft,
    Scheduled,
    Sending,
    Sent,
    Paused,
    Cancelled
}

