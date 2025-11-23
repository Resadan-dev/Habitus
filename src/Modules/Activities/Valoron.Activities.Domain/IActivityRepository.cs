namespace Valoron.Activities.Domain;

public interface IActivityRepository
{
    Task<Activity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task SaveAsync(Activity activity, CancellationToken cancellationToken = default);
}
