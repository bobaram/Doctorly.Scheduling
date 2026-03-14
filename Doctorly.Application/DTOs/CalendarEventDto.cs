namespace Doctorly.Application.DTOs;

public record CalendarEventDto(
    Guid Id,
    string Title,
    string? Description,
    DateTime StartTime,
    DateTime EndTime,
    IEnumerable<AttendeeDto> Attendees);
