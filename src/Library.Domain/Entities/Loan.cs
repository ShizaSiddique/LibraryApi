namespace Library.Domain.Entities;

public class Loan
{
    public int Id { get; set; }

    public int BookId { get; set; }
    public Book Book { get; set; } = null!;

    public required string UserId { get; set; }

    public DateTime BorrowedAt { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnedAt { get; set; }

    public bool IsOverdue => ReturnedAt is null && DateTime.UtcNow > DueDate;
}