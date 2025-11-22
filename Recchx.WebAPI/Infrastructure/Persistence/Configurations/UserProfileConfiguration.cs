using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recchx.Users.Domain.Entities;

namespace Recchx.WebAPI.Infrastructure.Persistence.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("UserProfiles");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedNever();

        builder.HasIndex(p => p.UserId)
            .IsUnique();

        builder.Property(p => p.Summary)
            .HasColumnType("text");

        builder.Property(p => p.Skills)
            .HasColumnType("text[]");

        builder.Property(p => p.Preferences)
            .HasColumnType("jsonb");

        builder.Property(p => p.Embedding)
            .HasColumnType("real[]");

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .IsRequired();
    }
}

