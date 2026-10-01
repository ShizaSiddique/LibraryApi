// using Library.Application.Books;
// using Library.Application.Common.Interfaces;
// using Library.Domain.Entities;
// using Microsoft.EntityFrameworkCore;

// namespace Library.Infrastructure.Persistence;

// public class EfBookStore(LibraryDbContext context) : IBookStore
// {
//     private readonly LibraryDbContext _context = context;

//     public async Task<IReadOnlyList<BookDto>> GetAllAsync()
//     {
//         return await _context.Books
//             .AsNoTracking()
//             .OrderBy(b => b.Title)
//             .Select(b => new BookDto(//EF converts this lambda into SQL t
//                 b.Id, b.Title, b.Isbn, b.TotalCopies, b.AvailableCopies, b.AuthorId, b.Author.Name))
//             .ToListAsync();
//     }

//     public async Task<BookDto?> GetByIdAsync(int id)
//     {
//         return await _context.Books
//             .AsNoTracking()
//             .Where(b => b.Id == id)
//             .Select(b => new BookDto(
//                 b.Id, b.Title, b.Isbn, b.TotalCopies, b.AvailableCopies, b.AuthorId, b.Author.Name))
//             .FirstOrDefaultAsync();
//     }

//     public async Task<BookDto> AddAsync(CreateBookRequest request)
//     {
//         var book = new Book
//         {
//             Title = request.Title,
//             Isbn = request.Isbn,
//             AuthorId = request.AuthorId,
//             TotalCopies = request.TotalCopies,
//             AvailableCopies = request.TotalCopies
//         };

//         _context.Books.Add(book);
//         await _context.SaveChangesAsync();

//         return (await GetByIdAsync(book.Id))!;
//     }

//     public async Task<bool> UpdateAsync(int id, CreateBookRequest request)
//     {
//         var book = await _context.Books.FindAsync(id);
//         if (book is null)
//             return false;

//         var borrowedCopies = book.TotalCopies - book.AvailableCopies;

//         book.Title = request.Title;
//         book.Isbn = request.Isbn;
//         book.AuthorId = request.AuthorId;
//         book.TotalCopies = request.TotalCopies;
//         book.AvailableCopies = request.TotalCopies - borrowedCopies;

//         await _context.SaveChangesAsync();//notices what changed and generates an UPDATE for only those columns. That's EF's change tracker.
//         return true;
//     }

//     public async Task<bool> DeleteAsync(int id)
//     {
//         var book = await _context.Books.FindAsync(id);
//         if (book is null)
//             return false;

//         _context.Books.Remove(book);
//         await _context.SaveChangesAsync();
//         return true;
//     }
// }