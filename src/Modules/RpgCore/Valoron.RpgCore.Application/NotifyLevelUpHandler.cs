using Microsoft.Extensions.Logging;
using Valoron.RpgCore.Domain.Events;
using Wolverine;

namespace Valoron.RpgCore.Application;

public class NotifyLevelUpHandler
{
    private readonly ILogger<NotifyLevelUpHandler> _logger;

    public NotifyLevelUpHandler(ILogger<NotifyLevelUpHandler> logger)
    {
        _logger = logger;
    }

    public void Handle(PlayerLeveledUpEvent @event)
    {
        _logger.LogInformation("Player {PlayerId} leveled up to level {NewLevel}! Stats increased!", @event.PlayerId, @event.NewLevel);
        
        // TODO: In the future, we could unlock achievements or badges here.
    }
}
