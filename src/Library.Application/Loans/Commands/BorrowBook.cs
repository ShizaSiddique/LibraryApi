using Library.Application.Common.Exceptions;
using Library.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Loans.Commands;

public record BorrowBookCommand(int BookId, string UserId) : IRequest<int>;

public class BorrowBookCommandHandler : IRequestHandler<BorrowBookCommand, int>
{

      //The handler uses two dependencies: EF (_context) for the existence check, 
        // and the procedure (_procedures) for the actual borrow. Mixing both is normal.
    private readonly ILibraryDbContext _context;
    private readonly ILoanProcedures _procedures;

    public BorrowBookCommandHandler(ILibraryDbContext context, ILoanProcedures procedures)
    {
        _context = context;
        _procedures = procedures;
    }

    public async Task<int> Handle(BorrowBookCommand request, CancellationToken cancellationToken)
    {
      
        if (!await _context.Books.AnyAsync(b => b.Id == request.BookId, cancellationToken))
            throw new NotFoundException($"Book {request.BookId} was not found.");

        var loanId = await _procedures.BorrowBookAsync(request.BookId, request.UserId, cancellationToken);

        if (loanId == 0)
            throw new BadRequestException("No copies of this book are currently available.");

        return loanId;
    }
}