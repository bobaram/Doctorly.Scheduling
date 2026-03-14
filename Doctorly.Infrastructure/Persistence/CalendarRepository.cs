using Doctorly.Domain.Entities;
using Doctorly.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Doctorly.Infrastructure.Persistence;

public class CalendarRepository : ICalendarRepository
{
    private readonly AppDbContext _context;

    public CalendarRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(CalendarEvent calendarEvent, CancellationToken ct = default)
    {
        await _context.CalendarEvents.AddAsync(calendarEvent, ct);
    }

    public async Task<CalendarEvent?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.CalendarEvents
            .Include(x => x.Attendees)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<IEnumerable<CalendarEvent>> GetScheduledEventsInRangeAsync(DateTime from, DateTime to, CancellationToken ct = default)
    {
        return await _context.CalendarEvents
            .AsNoTracking()
            .Include(x => x.Attendees)
            .Where(x => x.Duration.Start >= from && x.Duration.End <= to)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<CalendarEvent>> FindEventsByKeywordAsync(string keyword, CancellationToken ct = default)
    {
        return await _context.CalendarEvents
            .AsNoTracking()
            .Include(x => x.Attendees)
            .Where(x => x.Title.Contains(keyword) || (x.Description != null && x.Description.Contains(keyword)))
            .ToListAsync(ct);
    }

    public Task UpdateAsync(CalendarEvent calendarEvent, CancellationToken ct = default)
    {
        _context.Entry(calendarEvent).State = EntityState.Modified;
        // Also ensure attendees are tracked if they changed
        foreach (var attendee in calendarEvent.Attendees)
        {
            _context.Entry(attendee).State = EntityState.Modified;
        }
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var calendarEvent = await GetByIdAsync(id, ct);
        if (calendarEvent != null)
        {
            _context.CalendarEvents.Remove(calendarEvent);
        }
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }
}
