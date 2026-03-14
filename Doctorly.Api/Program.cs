using Doctorly.Api.Middleware;
using Doctorly.Application.Commands;
using Doctorly.Application.Queries;
using Doctorly.Domain.Interfaces;
using Doctorly.Infrastructure.Messaging;
using Doctorly.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Framework Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Infrastructure Registrations
builder.Services.AddScoped<AuditInterceptor>();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>((sp, options) =>
{
    options.UseSqlite(connectionString)
           .UseSnakeCaseNamingConvention()
           .AddInterceptors(sp.GetRequiredService<AuditInterceptor>());
});

builder.Services.AddScoped<ICalendarRepository, CalendarRepository>();
builder.Services.AddScoped<INotificationService, EmailNotificationService>();

// Application Registrations (Handlers)
builder.Services.AddScoped<CreateEventCommandHandler>();
builder.Services.AddScoped<GetEventQueryHandler>();
builder.Services.AddScoped<GetEventsInRangeQueryHandler>();
builder.Services.AddScoped<FindEventsByKeywordQueryHandler>();
builder.Services.AddScoped<UpdateEventCommandHandler>();
builder.Services.AddScoped<DeleteEventCommandHandler>();
builder.Services.AddScoped<UpdateAttendeeStatusCommandHandler>();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

// Apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (db.Database.IsRelational())
    {
        db.Database.Migrate();
    }
}

app.UseMiddleware<ConcurrencyExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }
