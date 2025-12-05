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
        var query = from activity in _context.Activities.AsNoTracking()
                    join book in _context.Books.AsNoTracking() on activity.ResourceId equals book.Id into books
                    from book in books.DefaultIfEmpty()
                    select new ActivityDto(
                        activity.Id,
                        activity.Title,
                        activity.Category,
                        activity.Difficulty,
                        activity.IsCompleted,
                        activity.CreatedAt,
                        activity.CompletedAt,
                        activity.ResourceId,
                        activity.Measurement,
                        book != null ? book.Status.ToString() : null);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<ActivityDto?> GetActivityByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = from activity in _context.Activities.AsNoTracking()
                    where activity.Id == id
                    join book in _context.Books.AsNoTracking() on activity.ResourceId equals book.Id into books
                    from book in books.DefaultIfEmpty()
                    select new ActivityDto(
                        activity.Id,
                        activity.Title,
                        activity.Category,
                        activity.Difficulty,
                        activity.IsCompleted,
                        activity.CreatedAt,
                        activity.CompletedAt,
                        activity.ResourceId,
                        activity.Measurement,
                        book != null ? book.Status.ToString() : null);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }
}
