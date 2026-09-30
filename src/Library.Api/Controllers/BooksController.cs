using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    // TEMPORARY: in-memory data until we add the database in Step 5
    private static readonly List<BookDto> _books = new()
    {
        new BookDto(1, "Clean Code", "978-0132350884", 5),
        new BookDto(2, "The Pragmatic Programmer", "978-0201616224", 3),
    };

    // GET api/books
    [HttpGet]
    public ActionResult<IEnumerable<BookDto>> GetAll()
    {
        return Ok(_books);
    }

    // GET api/books/1
    [HttpGet("{id:int}")]
    public ActionResult<BookDto> GetById(int id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);

        if (book is null)
            return NotFound();

        return Ok(book);
    }

    // POST api/books
    [HttpPost]
    public ActionResult<BookDto> Create(CreateBookRequest request)
    {
        var newId = _books.Count == 0 ? 1 : _books.Max(b => b.Id) + 1;
        var book = new BookDto(newId, request.Title, request.Isbn, request.TotalCopies);

        _books.Add(book);

        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    // PUT api/books/1
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, CreateBookRequest request)
    {
        var index = _books.FindIndex(b => b.Id == id);

        if (index == -1)
            return NotFound();

        _books[index] = new BookDto(id, request.Title, request.Isbn, request.TotalCopies);
        return NoContent();
    }

    // DELETE api/books/1
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);

        if (book is null)
            return NotFound();

        _books.Remove(book);
        return NoContent();
    }
}

// TEMPORARY: these move to the Application layer in Step 7
public record BookDto(int Id, string Title, string Isbn, int TotalCopies);
public record CreateBookRequest(string Title, string Isbn, int TotalCopies);