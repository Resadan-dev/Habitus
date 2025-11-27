using Valoron.Activities.Domain;

namespace Valoron.Activities.Application;

public class LogReadingSessionHandler
{
    private readonly IActivityRepository _activityRepository;
    private readonly TimeProvider _timeProvider;

    public LogReadingSessionHandler(IActivityRepository activityRepository, TimeProvider timeProvider)
    {
        _activityRepository = activityRepository;
        _timeProvider = timeProvider;
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

        activity.LogProgress(command.PagesRead, _timeProvider.GetUtcNow().DateTime);



        return activity.DomainEvents;
    }
}
