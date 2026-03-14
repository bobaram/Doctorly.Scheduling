using Doctorly.Application.DTOs;
using Doctorly.Domain.Interfaces;

namespace Doctorly.Application.Queries;

public record GetEventQuery(Guid Id);

public class GetEventQueryHandler
{
    private readonly ICalendarRepository _repository;

    public GetEventQueryHandler(ICalendarRepository repository)
    {
        _repository = repository;
    }

    public async Task<CalendarEventDto?> Handle(GetEventQuery query, CancellationToken ct = default)
    {
        var calendarEvent = await _repository.GetByIdAsync(query.Id, ct);
        
        if (calendarEvent == null) return null;

        return new CalendarEventDto(
            calendarEvent.Id,
            calendarEvent.Title,
            calendarEvent.Description,
            calendarEvent.Duration.Start,
            calendarEvent.Duration.End,
            calendarEvent.Attendees.Select(a => new AttendeeDto(a.Name, a.Email, a.IsAttending))
        );
    }
}
