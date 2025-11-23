using Microsoft.EntityFrameworkCore;
using Valoron.Activities.Domain;

namespace Valoron.Activities.Infrastructure.Persistence.Repositories;

public class ActivityRepository : IActivityRepository
{
    private readonly ActivitiesDbContext _context;

    public ActivityRepository(ActivitiesDbContext context)
    {
        _context = context;
    }

    public async Task<Activity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Activities
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task SaveAsync(Activity activity, CancellationToken cancellationToken = default)
    {
        if (_context.Entry(activity).State == EntityState.Detached)
        {
            await _context.Activities.AddAsync(activity, cancellationToken);
        }
        
        await _context.SaveChangesAsync(cancellationToken);
    }
}
