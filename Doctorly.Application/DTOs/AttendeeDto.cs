using System.ComponentModel.DataAnnotations;

namespace Doctorly.Application.DTOs;

public record AttendeeDto(
    [Required][MaxLength(100)] string Name, 
    [Required][EmailAddress][MaxLength(255)] string Email, 
    bool IsAttending);
