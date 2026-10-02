using Library.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Loans.Queries;

public record GetMyLoansQuery(string UserId) : IRequest<IReadOnlyList<LoanDto>>;

public class GetMyLoansQueryHandler : IRequestHandler<GetMyLoansQuery, IReadOnlyList<LoanDto>>
{
    private readonly ILibraryDbContext _context;

    public GetMyLoansQueryHandler(ILibraryDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<LoanDto>> Handle(GetMyLoansQuery request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        return await _context.Loans
            .AsNoTracking()
            .Where(l => l.UserId == request.UserId)
            .OrderByDescending(l => l.BorrowedAt)
            .Select(l => new LoanDto(
                l.Id, l.BookId, l.Book.Title, l.BorrowedAt, l.DueDate, l.ReturnedAt,
                l.ReturnedAt == null && l.DueDate < now))
            .ToListAsync(cancellationToken);
    }
}