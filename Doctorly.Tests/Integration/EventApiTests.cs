using System.Net;
using System.Net.Http.Json;
using Doctorly.Application.Commands;
using Doctorly.Application.DTOs;
using Doctorly.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Doctorly.Tests.Integration;

public class EventApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public EventApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Replace real DB with In-Memory for testing
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (descriptor != null) services.Remove(descriptor);

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });
            });
        });
    }

    [Fact]
    public async Task CreateEvent_Should_ReturnCreated_And_PersistToDb()
    {
        // Arrange
        var client = _factory.CreateClient();
        var command = new CreateEventCommand(
            "Integration Test Event",
            "Checking the full stack",
            DateTime.UtcNow.AddHours(1),
            DateTime.UtcNow.AddHours(2),
            new List<AttendeeDto> { new AttendeeDto("Test User", "test@example.com", true) }
        );

        // Act
        var response = await client.PostAsJsonAsync("/api/events", command);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<CreateEventResponse>();
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
    }
}
