using Library.Application.Common.Exceptions;
using Library.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Loans.Commands;

public record ReturnBookCommand(int LoanId, string UserId) : IRequest;

public class ReturnBookCommandHandler : IRequestHandler<ReturnBookCommand>
{
    private readonly ILibraryDbContext _context;

    public ReturnBookCommandHandler(ILibraryDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ReturnBookCommand request, CancellationToken cancellationToken)
    {
        var loan = await _context.Loans
            .Include(l => l.Book)
            .FirstOrDefaultAsync(l => l.Id == request.LoanId && l.UserId == request.UserId, cancellationToken)
            ?? throw new NotFoundException($"Loan {request.LoanId} was not found.");

        if (loan.ReturnedAt is not null)
            throw new BadRequestException("This book has already been returned.");

        loan.ReturnedAt = DateTime.UtcNow;
        loan.Book.AvailableCopies++;

        await _context.SaveChangesAsync(cancellationToken);
    }
}