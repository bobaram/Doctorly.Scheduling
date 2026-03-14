using System.ComponentModel.DataAnnotations;
using Doctorly.Domain.ValueObjects;
using Doctorly.Domain.Events;

namespace Doctorly.Domain.Entities;

public class CalendarEvent : BaseEntity
{
    [Required, MaxLength(200)]
    public string Title { get; private set; } = null!;

    [MaxLength(1000)]
    public string? Description { get; private set; }

    public TimeRange Duration { get; private set; } = null!;

    private readonly List<Attendee> _attendees = [];
    public IReadOnlyCollection<Attendee> Attendees => _attendees.AsReadOnly();

    public byte[] RowVersion { get; private set; } = []; // For concurrency

    private CalendarEvent() { } // EF Core

    public CalendarEvent(string title, string? description, TimeRange duration)
    {
        Title = title;
        Description = description;
        Duration = duration;
        
        AddDomainEvent(new CalendarEventCreated(this));
    }

    public void AddAttendee(Attendee attendee)
    {
        if (_attendees.Any(a => a.Email.Equals(attendee.Email, StringComparison.OrdinalIgnoreCase)))
            return;

        _attendees.Add(attendee);
        SetUpdated();
    }

    public void UpdateAttendeeStatus(string email, bool isAttending)
    {
        var attendee = _attendees.FirstOrDefault(a => a.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        if (attendee == null) return;

        attendee.UpdateStatus(isAttending);
        AddDomainEvent(new AttendeeStatusChanged(Id, email, isAttending));
        SetUpdated();
    }

    public void UpdateDetails(string title, string? description, TimeRange duration)
    {
        Title = title;
        Description = description;
        Duration = duration;
        SetUpdated();
    }
}
