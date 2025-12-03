using Valoron.Activities.Domain.Events;
using Valoron.RpgCore.Domain;
using Valoron.BuildingBlocks;
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
        // Use the UserId from the event
        var playerId = @event.UserId;
        var player = await _playerRepository.GetByIdAsync(playerId, cancellationToken);

        if (player == null)
        {
            player = new Player(playerId);
            await _playerRepository.AddAsync(player, cancellationToken);
        }

        // Delegate XP calculation to the Domain
        player.GainXpFromActivity(@event.Unit, @event.Progress);

        // Explicitly save changes (RpgCore doesn't use Wolverine transaction integration)
        await _playerRepository.SaveChangesAsync(cancellationToken);

        return player.DomainEvents;
    }
}
