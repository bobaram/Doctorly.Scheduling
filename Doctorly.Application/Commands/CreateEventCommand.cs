namespace Doctorly.Application.Events.Commands;

public record AttendeeDto(string Name, string Email, bool IsAttending);

public record CreateEventCommand(
    string Title, 
    string? Description, 
    DateTime StartTime, 
    DateTime EndTime, 
    List<AttendeeDto> Attendees);
