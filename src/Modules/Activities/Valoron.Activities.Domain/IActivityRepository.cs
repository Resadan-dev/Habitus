namespace Valoron.Activities.Domain;

public interface IActivityRepository
{
    Task<Activity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Activity activity, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
