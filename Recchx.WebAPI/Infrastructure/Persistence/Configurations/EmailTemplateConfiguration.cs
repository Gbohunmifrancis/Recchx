using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recchx.Campaigns.Domain.Entities;

namespace Recchx.WebAPI.Infrastructure.Persistence.Configurations;

public class EmailTemplateConfiguration : IEntityTypeConfiguration<EmailTemplate>
{
    public void Configure(EntityTypeBuilder<EmailTemplate> builder)
    {
        builder.ToTable("EmailTemplates");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .ValueGeneratedNever();

        builder.HasIndex(t => t.UserId);

        builder.Property(t => t.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(t => t.Subject)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(t => t.Body)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        builder.Property(t => t.UpdatedAt)
            .IsRequired();
    }
}

