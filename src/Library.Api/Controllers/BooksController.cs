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
    public async Task<ActionResult<IReadOnlyList<BookDto>>> GetAll()
    {
        var books = await _bookStore.GetAllAsync();
        return Ok(books);
    }

    // GET api/books/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookDto>> GetById(int id)
    {
        var book = await _bookStore.GetByIdAsync(id);

        if (book is null)
            return NotFound();

        return Ok(book);
    }

    // POST api/books
    [HttpPost]
    public async Task<ActionResult<BookDto>> Create(CreateBookRequest request)
    {
        var book = await _bookStore.AddAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }


    // PUT api/books/1
    [HttpPut("{id:int}")]
     public async Task<IActionResult> Update(int id, CreateBookRequest request)
    {
        if (!await _bookStore.UpdateAsync(id, request))
            return NotFound();

        return NoContent();
    }

    // DELETE api/books/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await _bookStore.DeleteAsync(id))
            return NotFound();

        return NoContent();
    }
}
