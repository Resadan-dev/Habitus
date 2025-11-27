using Valoron.Activities.Domain.Events;
using Valoron.RpgCore.Domain;
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
        var playerId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var player = await _playerRepository.GetByIdAsync(playerId, cancellationToken);

        if (player == null)
        {
            player = new Player(playerId);
            await _playerRepository.AddAsync(player, cancellationToken);
        }

        // "Bonus de 500xp quand le livre est lu"
        player.AddXp(Rules.XpCalculator.BookFinishedBonus);

        return player.DomainEvents;
    }
}
