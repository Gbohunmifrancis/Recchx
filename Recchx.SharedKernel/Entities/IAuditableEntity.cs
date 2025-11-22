namespace Recchx.SharedKernel.Entities;

public interface IAuditableEntity
{
    DateTime CreatedAt { get; }
    DateTime UpdatedAt { get; }
    void UpdateTimestamp();
}

