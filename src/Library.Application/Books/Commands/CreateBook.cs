using Library.Application.Common.Exceptions;
using Library.Application.Common.Interfaces;
using Library.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Books.Commands;

public record CreateBookCommand(string Title, string Isbn, int AuthorId, int TotalCopies) : IRequest<int>;

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, int>
{
    private readonly ILibraryDbContext _context;

    public CreateBookCommandHandler(ILibraryDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        if (request.TotalCopies < 1)
            throw new BadRequestException("TotalCopies must be at least 1.");

        if (!await _context.Authors.AnyAsync(a => a.Id == request.AuthorId, cancellationToken))
            throw new BadRequestException($"Author {request.AuthorId} does not exist.");

        if (await _context.Books.AnyAsync(b => b.Isbn == request.Isbn, cancellationToken))
            throw new BadRequestException($"A book with ISBN {request.Isbn} already exists.");

        var book = new Book
        {
            Title = request.Title,
            Isbn = request.Isbn,
            AuthorId = request.AuthorId,
            TotalCopies = request.TotalCopies,
            AvailableCopies = request.TotalCopies
        };

        _context.Books.Add(book);
        await _context.SaveChangesAsync(cancellationToken);

        return book.Id;
    }
}