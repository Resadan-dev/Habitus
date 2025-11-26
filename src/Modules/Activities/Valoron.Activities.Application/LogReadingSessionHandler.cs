using Valoron.Activities.Domain;

namespace Valoron.Activities.Application;

public class LogReadingSessionHandler
{
    private readonly IActivityRepository _activityRepository;

    public LogReadingSessionHandler(IActivityRepository activityRepository)
    {
        _activityRepository = activityRepository;
    }

    public async Task<IEnumerable<object>> Handle(LogReadingSessionCommand command, CancellationToken cancellationToken)
    {
        var activity = await _activityRepository.GetByIdAsync(command.ActivityId, cancellationToken);
        if (activity == null)
        {
            throw new InvalidOperationException($"Activity with ID {command.ActivityId} not found.");
        }

        if (activity.ResourceId == null)
        {
            throw new InvalidOperationException("Activity is not linked to a book.");
        }

        activity.LogProgress(command.PagesRead);

        await _activityRepository.SaveChangesAsync(cancellationToken);

        return activity.DomainEvents;
    }
}
