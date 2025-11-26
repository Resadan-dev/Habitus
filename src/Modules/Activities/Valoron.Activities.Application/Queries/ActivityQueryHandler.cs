using Valoron.Activities.Application.Dtos;

namespace Valoron.Activities.Application.Queries;

public class ActivityQueryHandler
{
    private readonly IActivityQueries _queries;

    public ActivityQueryHandler(IActivityQueries queries)
    {
        _queries = queries;
    }

    public Task<IEnumerable<ActivityDto>> Handle(GetActivitiesQuery query, CancellationToken cancellationToken)
    {
        return _queries.GetActivitiesAsync(cancellationToken);
    }

    public Task<ActivityDto?> Handle(GetActivityByIdQuery query, CancellationToken cancellationToken)
    {
        return _queries.GetActivityByIdAsync(query.Id, cancellationToken);
    }
}
