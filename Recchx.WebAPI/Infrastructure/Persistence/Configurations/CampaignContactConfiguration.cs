using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recchx.Campaigns.Domain.Entities;

namespace Recchx.WebAPI.Infrastructure.Persistence.Configurations;

public class CampaignContactConfiguration : IEntityTypeConfiguration<CampaignContact>
{
    public void Configure(EntityTypeBuilder<CampaignContact> builder)
    {
        builder.ToTable("CampaignContacts");

        builder.HasKey(cc => cc.Id);

        builder.Property(cc => cc.Id)
            .ValueGeneratedNever();

        builder.HasIndex(cc => new { cc.CampaignId, cc.ContactId })
            .IsUnique();

        builder.Property(cc => cc.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(cc => cc.PersonalizedSubject)
            .HasColumnType("text");

        builder.Property(cc => cc.PersonalizedBody)
            .HasColumnType("text");

        builder.Property(cc => cc.CreatedAt)
            .IsRequired();

        builder.Property(cc => cc.UpdatedAt)
            .IsRequired();
    }
}
