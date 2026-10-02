using System.Security.Claims;
using Library.Application.Loans;
using Library.Application.Loans.Commands;
using Library.Application.Loans.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LoansController : ControllerBase
{
    private readonly ISender _sender;

    public LoansController(ISender sender)
    {
        _sender = sender;
    }

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    // POST api/loans (borrow a book)
    [HttpPost]
    public async Task<IActionResult> Borrow(BorrowBookRequest request, CancellationToken ct)
    {
        var loanId = await _sender.Send(new BorrowBookCommand(request.BookId, UserId), ct);
        return Ok(new { loanId });
    }

    // PUT api/loans/5/return (return a book)
    [HttpPut("{id:int}/return")]
    public async Task<IActionResult> Return(int id, CancellationToken ct)
    {
        await _sender.Send(new ReturnBookCommand(id, UserId), ct);
        return NoContent();
    }

    // GET api/loans/me (my loans)
    [HttpGet("me")]
    public async Task<ActionResult<IReadOnlyList<LoanDto>>> MyLoans(CancellationToken ct)
    {
        return Ok(await _sender.Send(new GetMyLoansQuery(UserId), ct));
    }

    // GET api/loans/overdue (overdue report)
    [HttpGet("overdue")]
    public async Task<ActionResult<IReadOnlyList<OverdueLoanDto>>> Overdue(CancellationToken ct)
    {
        return Ok(await _sender.Send(new GetOverdueLoansQuery(), ct));
    }
}

public record BorrowBookRequest(int BookId);