namespace Doctorly.Domain.Events;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
