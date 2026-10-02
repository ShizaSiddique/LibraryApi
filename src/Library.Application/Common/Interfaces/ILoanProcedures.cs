using Library.Application.Loans;

namespace Library.Application.Common.Interfaces;

public interface ILoanProcedures
{
    Task<int> BorrowBookAsync(int bookId, string userId, CancellationToken cancellationToken);
    Task<IReadOnlyList<OverdueLoanDto>> GetOverdueLoansAsync(CancellationToken cancellationToken);
}