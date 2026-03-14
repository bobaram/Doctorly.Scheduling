using System.ComponentModel.DataAnnotations;
using Doctorly.Application.DTOs;

namespace Doctorly.Application.Commands;

public record UpdateEventCommand(
    [Required] Guid Id,
    [Required][MaxLength(200)] string Title,
    [MaxLength(1000)] string? Description,
    [Required] DateTime StartTime,
    [Required] DateTime EndTime,
    [Required] byte[] RowVersion);

public record DeleteEventCommand(Guid Id);

public record UpdateAttendeeStatusCommand(Guid EventId, string Email, bool IsAttending);
