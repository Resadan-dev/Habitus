namespace Valoron.RpgCore.Domain;

public interface IPlayerRepository
{
    Task<Player?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Player player, CancellationToken cancellationToken);
}
