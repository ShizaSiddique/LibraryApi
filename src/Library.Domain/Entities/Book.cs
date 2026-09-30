namespace Library.Domain.Entities;

public class Book
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Isbn { get; set; }
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }

    public int AuthorId { get; set; }
    public Author Author { get; set; } = null!;

    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}