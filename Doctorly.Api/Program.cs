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
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
           .UseSnakeCaseNamingConvention());

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
