using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recchx.Prospects.Domain.Entities;
using Recchx.SharedKernel.ValueObjects;

namespace Recchx.WebAPI.Infrastructure.Persistence.Configurations;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("Contacts");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedNever();

        builder.Property(c => c.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Role)
            .HasMaxLength(255);

        builder.Property(c => c.Email)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value))
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(c => c.Email)
            .IsUnique();

        builder.Property(c => c.Verified)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .IsRequired();
    }
}

