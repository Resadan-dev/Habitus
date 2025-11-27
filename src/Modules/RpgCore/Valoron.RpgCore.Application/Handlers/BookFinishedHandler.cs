using Valoron.Activities.Domain.Events;
using Valoron.RpgCore.Domain;
using Valoron.RpgCore.Domain.Rules;
using Wolverine;

namespace Valoron.RpgCore.Application.Handlers;

public class BookFinishedHandler
{
    private readonly IPlayerRepository _playerRepository;

    public BookFinishedHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task<IEnumerable<object>> Handle(BookFinishedEvent @event, CancellationToken cancellationToken)
    {
        var playerId = @event.UserId;
        var player = await _playerRepository.GetByIdAsync(playerId, cancellationToken);

        if (player == null)
        {
            player = new Player(playerId);
            await _playerRepository.AddAsync(player, cancellationToken);
        }

        // "Bonus de 500xp quand le livre est lu"
        player.AddXp(XpCalculator.BookFinishedBonus);

        return player.DomainEvents;
    }
}
