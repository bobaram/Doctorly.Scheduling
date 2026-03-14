using Doctorly.Application.DTOs;
using Doctorly.Domain.Interfaces;

namespace Doctorly.Application.Queries;

public record FindEventsByKeywordQuery(string Keyword);

public class FindEventsByKeywordQueryHandler
{
    private readonly ICalendarRepository _repository;

    public FindEventsByKeywordQueryHandler(ICalendarRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CalendarEventDto>> Handle(FindEventsByKeywordQuery query, CancellationToken ct = default)
    {
        var events = await _repository.FindEventsByKeywordAsync(query.Keyword, ct);
        
        return events.Select(e => new CalendarEventDto(
            e.Id,
            e.Title,
            e.Description,
            e.Duration.Start,
            e.Duration.End,
            e.Attendees.Select(a => new AttendeeDto(a.Name, a.Email, a.IsAttending))
        ));
    }
}
