using Recchx.SharedKernel.Entities;

namespace Recchx.Campaigns.Domain.Entities;

public class CampaignContact : BaseEntity
{
    public Guid CampaignId { get; private set; }
    public Guid ContactId { get; private set; }
    public CampaignContactStatus Status { get; private set; }
    public DateTime? SentAt { get; private set; }
    public DateTime? OpenedAt { get; private set; }
    public DateTime? ClickedAt { get; private set; }
    public DateTime? RepliedAt { get; private set; }
    public string? PersonalizedSubject { get; private set; }
    public string? PersonalizedBody { get; private set; }
    
    public Campaign Campaign { get; private set; } = null!;

    private CampaignContact() { } // EF Core

    public CampaignContact(Guid campaignId, Guid contactId)
    {
        CampaignId = campaignId;
        ContactId = contactId;
        Status = CampaignContactStatus.Pending;
    }

    public void MarkAsSent(string personalizedSubject, string personalizedBody)
    {
        Status = CampaignContactStatus.Sent;
        SentAt = DateTime.UtcNow;
        PersonalizedSubject = personalizedSubject;
        PersonalizedBody = personalizedBody;
        UpdateTimestamp();
    }

    public void MarkAsOpened()
    {
        if (Status == CampaignContactStatus.Sent)
        {
            Status = CampaignContactStatus.Opened;
            OpenedAt = DateTime.UtcNow;
            UpdateTimestamp();
        }
    }

    public void MarkAsClicked()
    {
        if (Status is CampaignContactStatus.Sent or CampaignContactStatus.Opened)
        {
            Status = CampaignContactStatus.Clicked;
            ClickedAt = DateTime.UtcNow;
            UpdateTimestamp();
        }
    }

    public void MarkAsReplied()
    {
        Status = CampaignContactStatus.Replied;
        RepliedAt = DateTime.UtcNow;
        UpdateTimestamp();
    }

    public void MarkAsFailed()
    {
        Status = CampaignContactStatus.Failed;
        UpdateTimestamp();
    }
}

public enum CampaignContactStatus
{
    Pending,
    Sent,
    Opened,
    Clicked,
    Replied,
    Failed
}

