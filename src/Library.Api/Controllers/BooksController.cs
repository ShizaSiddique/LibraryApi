using Library.Application.Books;
using Library.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
     private readonly IBookStore _bookStore;


//constructor injection, the standard DI pattern in .NET.
    public BooksController(IBookStore bookStore)
    {
        _bookStore = bookStore;
    }

    // GET api/books
    [HttpGet]
    public ActionResult<IEnumerable<BookDto>> GetAll()
    {
        return Ok(_bookStore.GetAll());
    }

    // GET api/books/1
    [HttpGet("{id:int}")]
    public ActionResult<BookDto> GetById(int id)
    {
        var book = _bookStore.GetById(id);

        if (book is null)
            return NotFound();

        return Ok(book);
    }

    // POST api/books
    [HttpPost]
    public ActionResult<BookDto> Create(CreateBookRequest request)
    {
         var book = _bookStore.Add(request);
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    // PUT api/books/1
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, CreateBookRequest request)
    {
       if (!_bookStore.Update(id, request))
            return NotFound();

        return NoContent();
    }

    // DELETE api/books/1
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        if (!_bookStore.Delete(id))
            return NotFound();

        return NoContent();
    }
}
