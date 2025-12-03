using Microsoft.Extensions.DependencyInjection;
using Valoron.RpgCore.Application.Queries;

namespace Valoron.RpgCore.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddRpgCoreApplication(this IServiceCollection services)
    {
        services.AddScoped<GetPlayerQueryHandler>();
        return services;
    }
}
