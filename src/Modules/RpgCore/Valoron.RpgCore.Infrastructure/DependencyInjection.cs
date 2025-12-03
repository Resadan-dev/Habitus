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

        // RpgCore uses standard AddDbContext (not Wolverine integration)
        // because Wolverine 5.4.0 can only integrate with one DbContext for transactional outbox
        services.AddDbContext<RpgDbContext>(options =>
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorCodesToAdd: null);
            }));

        services.AddScoped<IPlayerRepository, PlayerRepository>();

        return services;
    }
}
