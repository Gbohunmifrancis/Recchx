namespace Recchx.SharedKernel.Events;

public interface IDomainEvent
{
    Guid EventId { get; }
    DateTime OccurredOn { get; }
}

