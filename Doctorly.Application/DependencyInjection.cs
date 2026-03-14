using Microsoft.Extensions.DependencyInjection;
using Doctorly.Application.Commands;

namespace Doctorly.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateEventCommandHandler>();
        // Add other handlers here as we build them
        return services;
    }
}
