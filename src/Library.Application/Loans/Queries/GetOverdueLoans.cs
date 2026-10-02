using Library.Application.Common.Interfaces;
using MediatR;

namespace Library.Application.Loans.Queries;

public record GetOverdueLoansQuery : IRequest<IReadOnlyList<OverdueLoanDto>>;

public class GetOverdueLoansQueryHandler : IRequestHandler<GetOverdueLoansQuery, IReadOnlyList<OverdueLoanDto>>
{
    private readonly ILoanProcedures _procedures;

    public GetOverdueLoansQueryHandler(ILoanProcedures procedures)
    {
        _procedures = procedures;
    }

    public Task<IReadOnlyList<OverdueLoanDto>> Handle(GetOverdueLoansQuery request, CancellationToken cancellationToken)
    {
        return _procedures.GetOverdueLoansAsync(cancellationToken);

        // EF with LINQ

    //     var overdue = await _context.Loans
    // .Where(l => l.ReturnedAt == null && l.DueDate < now)
    // .OrderBy(l => l.DueDate)
    // .Select(l => new OverdueLoanDto
    // {
    //     LoanId = l.Id,
    //     BookTitle = l.Book.Title,
    //     UserId = l.UserId,
    //     BorrowedAt = l.BorrowedAt,
    //     DueDate = l.DueDate,
    //     DaysOverdue = EF.Functions.DateDiffDay(l.DueDate, now)
    // })
    // .ToListAsync();
    }
}