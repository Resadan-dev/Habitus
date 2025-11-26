using Valoron.Activities.Domain;
using Wolverine;

namespace Valoron.Activities.Application;

public class CreateActivityHandler
{
    private readonly IActivityRepository _activityRepository;

    public CreateActivityHandler(IActivityRepository activityRepository)
    {
        _activityRepository = activityRepository;
    }

    public async Task<Guid> Handle(CreateActivityCommand command, IMessageContext context, CancellationToken cancellationToken)
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
            command.ResourceId);

        await _activityRepository.SaveAsync(activity, cancellationToken);

        foreach (var domainEvent in activity.DomainEvents)
        {
            await context.PublishAsync(domainEvent);
        }

        return activity.Id;
    }
}
