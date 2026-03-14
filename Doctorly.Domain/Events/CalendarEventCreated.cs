using Doctorly.Domain.Entities;

namespace Doctorly.Domain.Events;

public record CalendarEventCreated(CalendarEvent Event) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
