namespace Library.Application.Books;

public record BookDto(
    int Id,
    string Title,
    string Isbn,
    int TotalCopies,
    int AvailableCopies,
    int AuthorId,
    string AuthorName);