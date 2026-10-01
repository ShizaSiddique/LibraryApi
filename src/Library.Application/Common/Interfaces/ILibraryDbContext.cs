using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Common.Interfaces;

public interface ILibraryDbContext
{
    DbSet<Author> Authors { get; }
    DbSet<Book> Books { get; }
    DbSet<Loan> Loans { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}