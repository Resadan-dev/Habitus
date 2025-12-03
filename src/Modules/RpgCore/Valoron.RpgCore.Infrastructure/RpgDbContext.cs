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

            // Configuration explicite pour PlayerStats (owned type)
            entity.OwnsOne(e => e.Stats, statsBuilder =>
            {
                statsBuilder.Property(s => s.Strength).HasColumnName("Strength").IsRequired();
                statsBuilder.Property(s => s.Intellect).HasColumnName("Intellect").IsRequired();
                statsBuilder.Property(s => s.Stamina).HasColumnName("Stamina").IsRequired();
            });
        });
    }
}
