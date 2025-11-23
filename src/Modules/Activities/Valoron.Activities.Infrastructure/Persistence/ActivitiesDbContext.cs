using Microsoft.EntityFrameworkCore;
using Valoron.Activities.Domain;

namespace Valoron.Activities.Infrastructure.Persistence;

public class ActivitiesDbContext : DbContext
{
    public DbSet<Activity> Activities { get; set; }
    public DbSet<Book> Books { get; set; }

    public ActivitiesDbContext(DbContextOptions<ActivitiesDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("activities");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ActivitiesDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
