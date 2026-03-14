using Doctorly.Domain.Entities;
using Doctorly.Domain.Interfaces;
using Doctorly.Domain.ValueObjects;

namespace Doctorly.Application.Commands;

public class CreateEventCommandHandler
{
    private readonly ICalendarRepository _repository;
    private readonly INotificationService _notificationService;

    public CreateEventCommandHandler(ICalendarRepository repository, INotificationService notificationService)
    {
        _repository = repository;
        _notificationService = notificationService;
    }

    public async Task<Guid> Handle(CreateEventCommand command, CancellationToken ct = default)
    {
        var duration = new TimeRange(command.StartTime, command.EndTime);
        var calendarEvent = new CalendarEvent(command.Title, command.Description, duration);

        foreach (var attendeeDto in command.Attendees)
        {
            var attendee = new Attendee(attendeeDto.Name, attendeeDto.Email, attendeeDto.IsAttending);
            calendarEvent.AddAttendee(attendee);
        }

        await _repository.AddAsync(calendarEvent, ct);
        await _repository.SaveChangesAsync(ct);

        // Notify attendees (Satisfies "Must" requirement)
        foreach (var attendee in calendarEvent.Attendees)
        {
            await _notificationService.SendAsync(attendee.Email, "New Event Created", $"You have been added to {calendarEvent.Title}");
        }

        return calendarEvent.Id;
    }
}
