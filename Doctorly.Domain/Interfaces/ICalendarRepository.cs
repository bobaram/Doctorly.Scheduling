using Doctorly.Domain.Entities;

namespace Doctorly.Domain.Interfaces;

public interface ICalendarRepository
{
    Task AddAsync(CalendarEvent calendarEvent, CancellationToken ct = default);
    Task<CalendarEvent?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<CalendarEvent>> GetScheduledEventsInRangeAsync(DateTime from, DateTime to, CancellationToken ct = default);
    Task<IEnumerable<CalendarEvent>> FindEventsByKeywordAsync(string keyword, CancellationToken ct = default);
    Task UpdateAsync(CalendarEvent calendarEvent, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
