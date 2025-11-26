using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Valoron.Activities.Domain;
using Valoron.Activities.Infrastructure.Persistence;
using Valoron.Activities.Infrastructure.Persistence.Repositories;
using Wolverine.EntityFrameworkCore;

namespace Valoron.Activities.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddActivitiesInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");

        services.AddDbContextWithWolverineIntegration<ActivitiesDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IActivityRepository, ActivityRepository>();
        services.AddScoped<IBookRepository, BookRepository>();

        return services;
    }
}
