using Valoron.BuildingBlocks;
using Valoron.RpgCore.Application.Dtos;
using Valoron.RpgCore.Domain;

namespace Valoron.RpgCore.Application.Queries;

public class GetPlayerQueryHandler
{
    private readonly IPlayerRepository _playerRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetPlayerQueryHandler(IPlayerRepository playerRepository, ICurrentUserService currentUserService)
    {
        _playerRepository = playerRepository;
        _currentUserService = currentUserService;
    }

    public async Task<PlayerDto?> Handle(GetPlayerQuery query, CancellationToken cancellationToken)
    {
        var player = await _playerRepository.GetByIdAsync(_currentUserService.UserId, cancellationToken);

        if (player == null)
        {
            return null;
        }

        return new PlayerDto(
            player.Id,
            player.Level,
            player.Xp,
            player.Stats.Strength,
            player.Stats.Intellect,
            player.Stats.Stamina);
    }
}
