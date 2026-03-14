using Doctorly.Domain.Entities;

namespace Doctorly.Domain.Events;

public record AttendeeStatusChanged(Guid EventId, string Email, bool IsAttending) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
