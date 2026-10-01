using Library.Application.Common.Exceptions;
using Library.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Books.Commands;

public record UpdateBookCommand(int Id, string Title, string Isbn, int AuthorId, int TotalCopies) : IRequest;

public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand>
{
    private readonly ILibraryDbContext _context;

    public UpdateBookCommandHandler(ILibraryDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException($"Book {request.Id} was not found.");

        var borrowedCopies = book.TotalCopies - book.AvailableCopies;
        if (request.TotalCopies < borrowedCopies)
            throw new BadRequestException($"TotalCopies cannot be less than the {borrowedCopies} copies currently on loan.");

        if (!await _context.Authors.AnyAsync(a => a.Id == request.AuthorId, cancellationToken))
            throw new BadRequestException($"Author {request.AuthorId} does not exist.");

        if (await _context.Books.AnyAsync(b => b.Isbn == request.Isbn && b.Id != request.Id, cancellationToken))
            throw new BadRequestException($"A book with ISBN {request.Isbn} already exists.");

        book.Title = request.Title;
        book.Isbn = request.Isbn;
        book.AuthorId = request.AuthorId;
        book.TotalCopies = request.TotalCopies;
        book.AvailableCopies = request.TotalCopies - borrowedCopies;

        await _context.SaveChangesAsync(cancellationToken);
    }
}