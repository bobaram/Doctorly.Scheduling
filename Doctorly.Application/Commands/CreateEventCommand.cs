using System.ComponentModel.DataAnnotations;
using Doctorly.Application.DTOs;

namespace Doctorly.Application.Commands;

public record CreateEventCommand(
    [Required][MaxLength(200)] string Title, 
    [MaxLength(1000)] string? Description, 
    [Required] DateTime StartTime, 
    [Required] DateTime EndTime, 
    [Required][MinLength(1, ErrorMessage = "At least one attendee is required.")] List<AttendeeDto> Attendees);
