using Valoron.Activities.Domain;
using Wolverine;

namespace Valoron.Activities.Application;

public class CreateActivityHandler
{
    private readonly IActivityRepository _activityRepository;
    private readonly TimeProvider _timeProvider;

    public CreateActivityHandler(IActivityRepository activityRepository, TimeProvider timeProvider)
    {
        _activityRepository = activityRepository;
        _timeProvider = timeProvider;
    }

    public async Task<(Guid, IEnumerable<object>)> Handle(CreateActivityCommand command, CancellationToken cancellationToken)
    {
        var category = ActivityCategory.FromCode(command.CategoryCode);
        var difficulty = ActivityDifficulty.Create(command.Difficulty);

        ActivityMeasurement measurement;
        if (command.MeasurementType.Equals("Binary", StringComparison.OrdinalIgnoreCase))
        {
            measurement = ActivityMeasurement.CreateBinary();
        }
        else if (command.MeasurementType.Equals("Quantifiable", StringComparison.OrdinalIgnoreCase))
        {
            measurement = ActivityMeasurement.CreateQuantifiable(command.MeasurementUnit, command.MeasurementTarget);
        }
        else
        {
            throw new ArgumentException($"Invalid measurement type: {command.MeasurementType}");
        }

        var activity = new Activity(
            Guid.NewGuid(),
            command.Title,
            category,
            difficulty,
            measurement,
            _timeProvider.GetUtcNow().DateTime,
            command.ResourceId);

        await _activityRepository.AddAsync(activity, cancellationToken);

        return (activity.Id, activity.DomainEvents);
    }
}
