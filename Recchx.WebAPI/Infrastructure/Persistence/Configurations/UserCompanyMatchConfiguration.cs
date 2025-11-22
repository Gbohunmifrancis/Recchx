using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recchx.Applications.Domain.Entities;

namespace Recchx.WebAPI.Infrastructure.Persistence.Configurations;

public class UserCompanyMatchConfiguration : IEntityTypeConfiguration<UserCompanyMatch>
{
    public void Configure(EntityTypeBuilder<UserCompanyMatch> builder)
    {
        builder.ToTable("UserCompanyMatches");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .ValueGeneratedNever();

        builder.HasIndex(m => new { m.UserId, m.CompanyId })
            .IsUnique();

        builder.HasIndex(m => m.UserId);

        builder.Property(m => m.Score)
            .IsRequired();

        builder.Property(m => m.GeneratedAt)
            .IsRequired();

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.Property(m => m.UpdatedAt)
            .IsRequired();
    }
}

