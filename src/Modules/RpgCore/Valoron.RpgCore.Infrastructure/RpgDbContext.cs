using Microsoft.EntityFrameworkCore;
using Valoron.RpgCore.Domain;

namespace Valoron.RpgCore.Infrastructure;

public class RpgDbContext : DbContext
{
    public DbSet<Player> Players { get; set; }

    public RpgDbContext(DbContextOptions<RpgDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("rpg");

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Xp).IsRequired();
            entity.Property(e => e.Level).IsRequired();
        });
    }
}
