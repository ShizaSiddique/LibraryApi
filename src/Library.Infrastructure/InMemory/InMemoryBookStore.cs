using Library.Application.Books;
using Library.Application.Common.Interfaces;
namespace Library.Infrastructure.InMemory;

public class InMemoryBookStore : IBookStore
{


    private readonly List<BookDto> _books = new()
    {
        new BookDto(1, "Clean Code", "978-0132350884", 5),
        new BookDto(2, "The Pragmatic Programmer", "978-0201616224", 3),
    };
    
     public IReadOnlyList<BookDto> GetAll() => _books;

    public BookDto? GetById(int id) => _books.FirstOrDefault(b => b.Id == id);

    public BookDto Add(CreateBookRequest request)
    {
        var newId = _books.Count == 0 ? 1 : _books.Max(b => b.Id) + 1;
        var book = new BookDto(newId, request.Title, request.Isbn, request.TotalCopies);
        _books.Add(book);
        return book;
    }

    public bool Delete(int id)
    {
        var book = GetById(id);
        if (book is null)
            return false;

        _books.Remove(book);
        return true;
    }
        
  

    public bool Update(int id, CreateBookRequest request)
    {
        var index = _books.FindIndex(b => b.Id == id);
        if (index == -1)
            return false;

        _books[index] = new BookDto(id, request.Title, request.Isbn, request.TotalCopies);
        return true;
    }
}
