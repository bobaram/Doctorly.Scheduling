using Doctorly.Domain.Interfaces;
using Doctorly.Domain.ValueObjects;

namespace Doctorly.Application.Commands;

public class UpdateEventCommandHandler
{
    private readonly ICalendarRepository _repository;

    public UpdateEventCommandHandler(ICalendarRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateEventCommand command, CancellationToken ct = default)
    {
        var calendarEvent = await _repository.GetByIdAsync(command.Id, ct);
        if (calendarEvent == null) throw new KeyNotFoundException("Event not found");

        var duration = new TimeRange(command.StartTime, command.EndTime);
        calendarEvent.UpdateDetails(command.Title, command.Description, duration);
        
        // EF Core will use RowVersion for optimistic concurrency
        await _repository.UpdateAsync(calendarEvent, ct);
        await _repository.SaveChangesAsync(ct);
    }
}

public class DeleteEventCommandHandler
{
    private readonly ICalendarRepository _repository;

    public DeleteEventCommandHandler(ICalendarRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteEventCommand command, CancellationToken ct = default)
    {
        await _repository.DeleteAsync(command.Id, ct);
        await _repository.SaveChangesAsync(ct);
    }
}

public class UpdateAttendeeStatusCommandHandler
{
    private readonly ICalendarRepository _repository;

    public UpdateAttendeeStatusCommandHandler(ICalendarRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateAttendeeStatusCommand command, CancellationToken ct = default)
    {
        var calendarEvent = await _repository.GetByIdAsync(command.EventId, ct);
        if (calendarEvent == null) throw new KeyNotFoundException("Event not found");

        calendarEvent.UpdateAttendeeStatus(command.Email, command.IsAttending);
        
        // Use repository update to ensure the graph is marked as dirty
        await _repository.UpdateAsync(calendarEvent, ct);
        await _repository.SaveChangesAsync(ct);
    }
}
