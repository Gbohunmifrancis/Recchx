using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recchx.Campaigns.Domain.Entities;

namespace Recchx.WebAPI.Infrastructure.Persistence.Configurations;

public class CampaignConfiguration : IEntityTypeConfiguration<Campaign>
{
    public void Configure(EntityTypeBuilder<Campaign> builder)
    {
        builder.ToTable("Campaigns");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedNever();

        builder.HasIndex(c => c.UserId);

        builder.Property(c => c.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(c => c.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .IsRequired();

        builder.HasMany(c => c.CampaignContacts)
            .WithOne(cc => cc.Campaign)
            .HasForeignKey(cc => cc.CampaignId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

