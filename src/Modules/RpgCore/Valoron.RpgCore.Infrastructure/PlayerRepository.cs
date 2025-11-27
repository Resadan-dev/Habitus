using Valoron.RpgCore.Domain;

namespace Valoron.RpgCore.Infrastructure;

public class PlayerRepository : IPlayerRepository
{
    private readonly RpgDbContext _dbContext;

    public PlayerRepository(RpgDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Player?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Players.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task AddAsync(Player player, CancellationToken cancellationToken)
    {
        await _dbContext.Players.AddAsync(player, cancellationToken);
    }
}
