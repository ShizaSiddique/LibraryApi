using Library.Application.Common.Exceptions;
using Library.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Books.Commands;

public record DeleteBookCommand(int Id) : IRequest;

public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand>
{
    private readonly ILibraryDbContext _context;

    public DeleteBookCommandHandler(ILibraryDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException($"Book {request.Id} was not found.");

        if (await _context.Loans.AnyAsync(l => l.BookId == request.Id, cancellationToken))
            throw new BadRequestException("Cannot delete a book that has loan history.");

        _context.Books.Remove(book);
        await _context.SaveChangesAsync(cancellationToken);
    }
}