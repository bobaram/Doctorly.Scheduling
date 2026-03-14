using Doctorly.Application.DTOs;

namespace Doctorly.Application.Commands;

public record CreateEventCommand(
    string Title, 
    string? Description, 
    DateTime StartTime, 
    DateTime EndTime, 
    List<AttendeeDto> Attendees);
