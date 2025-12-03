using Valoron.Activities.Domain;
using Valoron.Activities.Domain.Events;
using Valoron.BuildingBlocks;

namespace Valoron.Activities.Application;

public class BookCreatedHandler
{
    private readonly IActivityRepository _activityRepository;
    private readonly TimeProvider _timeProvider;

    public BookCreatedHandler(IActivityRepository activityRepository, TimeProvider timeProvider)
    {
        _activityRepository = activityRepository;
        _timeProvider = timeProvider;
    }

    public async Task Handle(BookCreated @event, CancellationToken ct)
    {
        var activity = new Activity(
            Guid.NewGuid(),
            @event.UserId,
            @event.Title,
            ActivityCategory.Learning,
            ActivityDifficulty.Medium,
            ActivityMeasurement.CreateQuantifiable(MeasureUnit.Pages, @event.TotalPages),
            DateTime.SpecifyKind(_timeProvider.GetUtcNow().DateTime, DateTimeKind.Utc),
            @event.Id
        );

        await _activityRepository.AddAsync(activity, ct);
        await _activityRepository.SaveChangesAsync(ct);
    }
}
