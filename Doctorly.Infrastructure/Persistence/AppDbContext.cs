using Doctorly.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Doctorly.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<CalendarEvent> CalendarEvents => Set<CalendarEvent>();
    public DbSet<Attendee> Attendees => Set<Attendee>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        
        modelBuilder.Entity<CalendarEvent>(builder =>
        {
            builder.OwnsOne(x => x.Duration);
            builder.Property(x => x.RowVersion).IsConcurrencyToken();
        });

        base.OnModelCreating(modelBuilder);
    }
}
