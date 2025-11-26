using Valoron.Activities.Application.Dtos;

namespace Valoron.Activities.Application.Queries;

public interface IActivityQueries
{
    Task<IEnumerable<ActivityDto>> GetActivitiesAsync(CancellationToken cancellationToken = default);
    Task<ActivityDto?> GetActivityByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
