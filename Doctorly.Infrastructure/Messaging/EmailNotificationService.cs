using Doctorly.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Doctorly.Infrastructure.Messaging;

public class EmailNotificationService : INotificationService
{
    private readonly ILogger<EmailNotificationService> _logger;

    public EmailNotificationService(ILogger<EmailNotificationService> logger)
    {
        _logger = logger;
    }

    public Task SendAsync(string recipient, string subject, string body)
    {
        _logger.LogInformation("Sending Email to {Recipient}: {Subject}", recipient, subject);
        return Task.CompletedTask;
    }
}
