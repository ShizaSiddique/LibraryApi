using Library.Application.Books;

namespace Library.Application.Common.Interfaces;

public interface IBookStore
{
    IReadOnlyList<BookDto> GetAll();
    BookDto? GetById(int id);
    BookDto Add(CreateBookRequest request);
    bool Update(int id, CreateBookRequest request);
    bool Delete(int id);
}