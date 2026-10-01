using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Library.Application.Common.Interfaces;

namespace Library.Infrastructure.Persistence;

public class LibraryDbContext : DbContext,ILibraryDbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
    {
    }

    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Loan> Loans => Set<Loan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryDbContext).Assembly);
    }
}