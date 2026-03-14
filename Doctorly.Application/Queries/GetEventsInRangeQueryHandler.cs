using Doctorly.Application.DTOs;
using Doctorly.Domain.Interfaces;

namespace Doctorly.Application.Queries;

public record GetEventsInRangeQuery(DateTime From, DateTime To);

public class GetEventsInRangeQueryHandler
{
    private readonly ICalendarRepository _repository;

    public GetEventsInRangeQueryHandler(ICalendarRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CalendarEventDto>> Handle(GetEventsInRangeQuery query, CancellationToken ct = default)
    {
        var events = await _repository.GetScheduledEventsInRangeAsync(query.From, query.To, ct);
        
        return events.Select(e => new CalendarEventDto(
            e.Id,
            e.Title,
            e.Description,
            e.Duration.Start,
            e.Duration.End,
            e.Attendees.Select(a => new AttendeeDto(a.Name, a.Email, a.IsAttending)),
            e.RowVersion
        ));
    }
}
