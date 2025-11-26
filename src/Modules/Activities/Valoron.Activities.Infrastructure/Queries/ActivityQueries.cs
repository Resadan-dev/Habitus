using Microsoft.EntityFrameworkCore;
using Valoron.Activities.Application.Dtos;
using Valoron.Activities.Application.Queries;
using Valoron.Activities.Infrastructure.Persistence;

namespace Valoron.Activities.Infrastructure.Queries;

public class ActivityQueries : IActivityQueries
{
    private readonly ActivitiesDbContext _context;

    public ActivityQueries(ActivitiesDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ActivityDto>> GetActivitiesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Activities
            .AsNoTracking()
            .Select(a => new ActivityDto(
                a.Id,
                a.Title,
                a.Category,
                a.Difficulty,
                a.IsCompleted,
                a.CreatedAt,
                a.CompletedAt,
                a.ResourceId,
                a.Measurement))
            .ToListAsync(cancellationToken);
    }

    public async Task<ActivityDto?> GetActivityByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Activities
            .AsNoTracking()
            .Where(a => a.Id == id)
            .Select(a => new ActivityDto(
                a.Id,
                a.Title,
                a.Category,
                a.Difficulty,
                a.IsCompleted,
                a.CreatedAt,
                a.CompletedAt,
                a.ResourceId,
                a.Measurement))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
