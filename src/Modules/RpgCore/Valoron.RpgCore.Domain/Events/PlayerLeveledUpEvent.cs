using Valoron.BuildingBlocks;

namespace Valoron.RpgCore.Domain.Events;

public record PlayerLeveledUpEvent(Guid PlayerId, int NewLevel);
