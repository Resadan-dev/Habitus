using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Valoron.RpgCore.Domain;
using Wolverine.EntityFrameworkCore;

namespace Valoron.RpgCore.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddRpgCoreInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");

        services.AddDbContextWithWolverineIntegration<RpgDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IPlayerRepository, PlayerRepository>();

        return services;
    }
}
