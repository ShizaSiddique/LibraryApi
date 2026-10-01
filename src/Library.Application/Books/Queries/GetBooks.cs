using Library.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Books.Queries;


//s MediatR's interface meaning "this is a request, and its answer is a list of BookDto." The generic type is the return type.

public record GetBooksQuery : IRequest<IReadOnlyList<BookDto>>;




//This is the handler. IRequestHandler<TRequest, TResponse> means "I handle GetBooksQuery and return a list of BookDto."
public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, IReadOnlyList<BookDto>>
{
    private readonly ILibraryDbContext _context;


//DI provides the database.
    public GetBooksQueryHandler(ILibraryDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        return await _context.Books
            .AsNoTracking()
            .OrderBy(b => b.Title)
            .Select(b => new BookDto(
                b.Id, b.Title, b.Isbn, b.TotalCopies, b.AvailableCopies, b.AuthorId, b.Author.Name))
            .ToListAsync(cancellationToken);
    }
}