using Valoron.Activities.Domain;
using Valoron.Activities.Domain.Events;
using Valoron.RpgCore.Domain;
using Wolverine;

namespace Valoron.RpgCore.Application.Handlers;

public class ActivityProgressLoggedHandler
{
    private readonly IPlayerRepository _playerRepository;

    public ActivityProgressLoggedHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task<IEnumerable<object>> Handle(ActivityProgressLogged @event, CancellationToken cancellationToken)
    {
        // TODO: Get current player. For now, we assume single player or we need a way to map Activity to Player.
        // Since the requirements didn't specify multi-user, and it's a "Habitus" app (likely personal),
        // I'll assume a single player or I'll need to fetch a default one.
        // However, to be robust, I should probably have a PlayerId.
        // But the Activity doesn't have a UserId yet.
        // Given the context of "Habitus" and the current codebase, it seems single-user or the user context is implicit.
        // I'll create a "Default" player if one doesn't exist, or fetch the first one.
        // Ideally, we'd have a CurrentUser service, but I don't see one.
        // I will implement a "GetOrCreateDefaultPlayer" logic in the repository or here.
        
        // For this task, I'll assume a fixed PlayerId for the single user, or just fetch the only player.
        // Let's assume a hardcoded ID for the "Main User" for now, or fetch the first one.
        
        var playerId = Guid.Parse("00000000-0000-0000-0000-000000000001"); // Placeholder for single user
        var player = await _playerRepository.GetByIdAsync(playerId, cancellationToken);
        
        if (player == null)
        {
            player = new Player(playerId);
            await _playerRepository.AddAsync(player, cancellationToken);
        }

        int xpToAdd = 0;

        if (@event.Unit == MeasureUnit.Pages)
        {
            // "Une page lue augmente l'xp de 10"
            xpToAdd = (int)@event.Progress * 10;
        }
        
        if (xpToAdd > 0)
        {
            player.AddXp(xpToAdd);
        }

        return player.DomainEvents;
    }
}
