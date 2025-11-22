using Recchx.SharedKernel.Entities;

namespace Recchx.Applications.Domain.Entities;

public class UserCompanyMatch : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid CompanyId { get; private set; }
    public double Score { get; private set; }
    public DateTime GeneratedAt { get; private set; }

    private UserCompanyMatch() { } // EF Core

    public UserCompanyMatch(Guid userId, Guid companyId, double score)
    {
        UserId = userId;
        CompanyId = companyId;
        Score = score;
        GeneratedAt = DateTime.UtcNow;
    }
}

