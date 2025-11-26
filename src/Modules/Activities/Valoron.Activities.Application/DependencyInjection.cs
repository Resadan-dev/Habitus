using Microsoft.Extensions.DependencyInjection;

namespace Valoron.Activities.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddActivitiesApplication(this IServiceCollection services)
    {
        services.AddScoped<LogReadingSessionHandler>();
        services.AddScoped<CreateBookHandler>();
        services.AddScoped<CreateActivityHandler>();
        services.AddScoped<UpdateBookProgressHandler>();

        return services;
    }
}
