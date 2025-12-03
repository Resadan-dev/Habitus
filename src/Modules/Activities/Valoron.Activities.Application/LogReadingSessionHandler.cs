using Valoron.Activities.Domain;
using Valoron.BuildingBlocks;

namespace Valoron.Activities.Application;

public class LogReadingSessionHandler
{
    private readonly IActivityRepository _activityRepository;
    private readonly TimeProvider _timeProvider;
    private readonly ICurrentUserService _currentUserService;

    public LogReadingSessionHandler(IActivityRepository activityRepository, TimeProvider timeProvider, ICurrentUserService currentUserService)
    {
        _activityRepository = activityRepository;
        _timeProvider = timeProvider;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<object>> Handle(LogReadingSessionCommand command, CancellationToken cancellationToken)
    {
        var activity = await _activityRepository.GetByIdAsync(command.ActivityId, cancellationToken);
        if (activity == null)
        {
            throw new InvalidOperationException($"Activity with ID {command.ActivityId} not found.");
        }

        if (activity.UserId != _currentUserService.UserId)
        {
            throw new UnauthorizedAccessException("This activity does not belong to you.");
        }

        if (activity.ResourceId == null)
        {
            throw new InvalidOperationException("Activity is not linked to a book.");
        }

        activity.LogProgress(command.PagesRead, DateTime.SpecifyKind(_timeProvider.GetUtcNow().DateTime, DateTimeKind.Utc));
        await _activityRepository.SaveChangesAsync(cancellationToken);

        return activity.DomainEvents;
    }
}
