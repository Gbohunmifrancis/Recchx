using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recchx.Mailbox.Domain.Entities;

namespace Recchx.WebAPI.Infrastructure.Persistence.Configurations;

public class MailboxConnectionConfiguration : IEntityTypeConfiguration<MailboxConnection>
{
    public void Configure(EntityTypeBuilder<MailboxConnection> builder)
    {
        builder.ToTable("MailboxConnections");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .ValueGeneratedNever();

        builder.HasIndex(m => m.UserId);

        builder.Property(m => m.Provider)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(m => m.AccessToken)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(m => m.RefreshToken)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(m => m.Scope)
            .HasColumnType("text");

        builder.Property(m => m.ConnectedAt)
            .IsRequired();

        builder.Property(m => m.ExpiresAt)
            .IsRequired();

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.Property(m => m.UpdatedAt)
            .IsRequired();
    }
}

