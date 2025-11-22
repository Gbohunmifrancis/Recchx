using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recchx.Prospects.Domain.Entities;

namespace Recchx.WebAPI.Infrastructure.Persistence.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedNever();

        builder.Property(c => c.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(c => c.Domain)
            .HasMaxLength(255);

        builder.HasIndex(c => c.Domain)
            .IsUnique()
            .HasFilter("\"Domain\" IS NOT NULL");

        builder.Property(c => c.Industry)
            .HasMaxLength(255);

        builder.Property(c => c.Size)
            .HasMaxLength(50);

        builder.Property(c => c.Description)
            .HasColumnType("text");

        builder.Property(c => c.Embedding)
            .HasColumnType("real[]");

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .IsRequired();

        builder.HasMany(c => c.Contacts)
            .WithOne(ct => ct.Company)
            .HasForeignKey(ct => ct.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

