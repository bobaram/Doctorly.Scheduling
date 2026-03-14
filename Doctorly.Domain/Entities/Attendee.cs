using System.ComponentModel.DataAnnotations;
using Doctorly.Domain.Entities;

namespace Doctorly.Domain.Entities;

public class Attendee : BaseEntity
{
    [Required, MaxLength(100)]
    public string Name { get; private set; } = null!;

    [Required, EmailAddress, MaxLength(255)]
    public string Email { get; private set; } = null!;

    public bool IsAttending { get; private set; }

    private Attendee() { } // EF Core

    public Attendee(string name, string email, bool isAttending = false)
    {
        Name = name;
        Email = email;
        IsAttending = isAttending;
    }

    public void UpdateStatus(bool isAttending)
    {
        IsAttending = isAttending;
        SetUpdated();
    }
}
