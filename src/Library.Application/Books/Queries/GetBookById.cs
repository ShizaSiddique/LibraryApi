using Library.Application.Common.Exceptions;
using Library.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Books.Queries;

public record GetBookByIdQuery(int Id) : IRequest<BookDto>;

public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, BookDto>
{
    private readonly ILibraryDbContext _context;

    public GetBookByIdQueryHandler(ILibraryDbContext context)
    {
        _context = context;
    }

    public async Task<BookDto> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = await _context.Books
            .AsNoTracking()
            .Where(b => b.Id == request.Id)
            .Select(b => new BookDto(
                b.Id, b.Title, b.Isbn, b.TotalCopies, b.AvailableCopies, b.AuthorId, b.Author.Name))
            .FirstOrDefaultAsync(cancellationToken);

        return book ?? throw new NotFoundException($"Book {request.Id} was not found.");
    }
}