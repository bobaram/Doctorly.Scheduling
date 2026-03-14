using Doctorly.Application.Commands;
using Doctorly.Application.DTOs;
using Doctorly.Domain.Entities;
using Doctorly.Domain.Interfaces;
using NSubstitute;
using Xunit;

namespace Doctorly.Tests.Unit;

public class CreateEventCommandHandlerTests
{
    private readonly ICalendarRepository _repository;
    private readonly INotificationService _notificationService;
    private readonly CreateEventCommandHandler _handler;

    public CreateEventCommandHandlerTests()
    {
        _repository = Substitute.For<ICalendarRepository>();
        _notificationService = Substitute.For<INotificationService>();
        _handler = new CreateEventCommandHandler(_repository, _notificationService);
    }

    [Fact]
    public async Task Handle_Should_CreateEvent_And_NotifyAttendees()
    {
        // Arrange
        var command = new CreateEventCommand(
            "Lunch", 
            "Team lunch", 
            DateTime.UtcNow.AddHours(1), 
            DateTime.UtcNow.AddHours(2),
            new List<AttendeeDto> { new AttendeeDto("Omen", "omen@example.com", false) }
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        await _repository.Received(1).AddAsync(Arg.Any<CalendarEvent>(), Arg.Any<CancellationToken>());
        await _repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        await _notificationService.Received(1).SendAsync("omen@example.com", Arg.Any<string>(), Arg.Any<string>());
    }
}
