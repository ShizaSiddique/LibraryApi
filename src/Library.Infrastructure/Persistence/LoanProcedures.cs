using Library.Application.Common.Interfaces;
using Library.Application.Loans;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Persistence;

public class LoanProcedures : ILoanProcedures
{
    private readonly LibraryDbContext _context;

    public LoanProcedures(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<int> BorrowBookAsync(int bookId, string userId, CancellationToken cancellationToken)
    {
        var result = await _context.Database
            .SqlQuery<int>($"EXEC dbo.sp_BorrowBook @BookId = {bookId}, @UserId = {userId}")
            .ToListAsync(cancellationToken);

        return result.FirstOrDefault();
    }

    public async Task<IReadOnlyList<OverdueLoanDto>> GetOverdueLoansAsync(CancellationToken cancellationToken)
    {
        return await _context.Database
            .SqlQuery<OverdueLoanDto>($"EXEC dbo.sp_GetOverdueLoans")
            .ToListAsync(cancellationToken);
    }
}