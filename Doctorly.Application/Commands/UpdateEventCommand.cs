using Doctorly.Application.DTOs;

namespace Doctorly.Application.Commands;

public record UpdateEventCommand(
    Guid Id,
    string Title,
    string? Description,
    DateTime StartTime,
    DateTime EndTime,
    byte[] RowVersion);

public record DeleteEventCommand(Guid Id);

public record UpdateAttendeeStatusCommand(Guid EventId, string Email, bool IsAttending);
