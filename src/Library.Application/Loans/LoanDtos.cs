namespace Library.Application.Loans;

public record LoanDto(
    int Id,
    int BookId,
    string BookTitle,
    DateTime BorrowedAt,
    DateTime DueDate,
    DateTime? ReturnedAt,
    bool IsOverdue);

public class OverdueLoanDto
{
    public int LoanId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string? UserEmail { get; set; }
    public DateTime BorrowedAt { get; set; }
    public DateTime DueDate { get; set; }
    public int DaysOverdue { get; set; }
}