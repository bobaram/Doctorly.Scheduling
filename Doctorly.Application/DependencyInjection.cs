using Microsoft.Extensions.DependencyInjection;
using Doctorly.Application.Commands;
using Doctorly.Application.Queries;

namespace Doctorly.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateEventCommandHandler>();
        services.AddScoped<GetEventQueryHandler>();
        services.AddScoped<GetEventsInRangeQueryHandler>();
        services.AddScoped<FindEventsByKeywordQueryHandler>();
        return services;
    }
}
