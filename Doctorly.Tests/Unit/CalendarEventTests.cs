using Doctorly.Domain.Entities;
using Doctorly.Domain.ValueObjects;
using Doctorly.Domain.Events;
using Xunit;

namespace Doctorly.Tests.Unit;

public class CalendarEventTests
{
    [Fact]
    public void Constructor_Should_InitializeEvent_And_RaiseDomainEvent()
    {
        // Arrange
        var start = DateTime.UtcNow.AddHours(1);
        var end = DateTime.UtcNow.AddHours(2);
        var duration = new TimeRange(start, end);

        // Act
        var calendarEvent = new CalendarEvent("Meeting", "Desc", duration);

        // Assert
        Assert.Equal("Meeting", calendarEvent.Title);
        Assert.Single(calendarEvent.DomainEvents);
        Assert.IsType<CalendarEventCreated>(calendarEvent.DomainEvents.First());
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentException_When_StartIsAfterEnd()
    {
        // Arrange
        var start = DateTime.UtcNow.AddHours(2);
        var end = DateTime.UtcNow.AddHours(1);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new TimeRange(start, end));
    }

    [Fact]
    public void UpdateAttendeeStatus_Should_ChangeStatus_And_RaiseDomainEvent()
    {
        // Arrange
        var duration = new TimeRange(DateTime.UtcNow.AddHours(1), DateTime.UtcNow.AddHours(2));
        var calendarEvent = new CalendarEvent("Meeting", "Desc", duration);
        var attendee = new Attendee("John", "john@example.com");
        calendarEvent.AddAttendee(attendee);
        calendarEvent.ClearDomainEvents();

        // Act
        calendarEvent.UpdateAttendeeStatus("john@example.com", true);

        // Assert
        Assert.True(attendee.IsAttending);
        Assert.Single(calendarEvent.DomainEvents);
        Assert.IsType<AttendeeStatusChanged>(calendarEvent.DomainEvents.First());
    }
}
