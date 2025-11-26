using Microsoft.EntityFrameworkCore;
using Valoron.Activities.Domain;

namespace Valoron.Activities.Infrastructure.Persistence.Repositories;

public class BookRepository : IBookRepository
{
    private readonly ActivitiesDbContext _context;

    public BookRepository(ActivitiesDbContext context)
    {
        _context = context;
    }

    public async Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task AddAsync(Book book, CancellationToken cancellationToken = default)
    {
        await _context.Books.AddAsync(book, cancellationToken);
    }


}
