using Library.Application.Books;
using Library.Application.Books.Commands;
using Library.Application.Books.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BooksController : ControllerBase
{

    ////ISender, MediatR's interface for sending requests.
    //It has no idea which handlers exist or what they do. 
    // That's the "mediator" idea: the sender and the handler don't know each other.
    private readonly ISender _sender;

    public BooksController(ISender sender)
    {
        _sender = sender;
    }

    // GET api/books (anyone)
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<BookDto>>> GetAll(CancellationToken ct)
    {
        return Ok(await _sender.Send(new GetBooksQuery(), ct));
    }

    // GET api/books/1 (anyone)
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<BookDto>> GetById(int id, CancellationToken ct)
    {
        return Ok(await _sender.Send(new GetBookByIdQuery(id), ct));
    }

    // POST api/books (logged-in users)
    [HttpPost]
    public async Task<ActionResult<BookDto>> Create(CreateBookCommand command, CancellationToken ct)
    {
        var id = await _sender.Send(command, ct);
        var book = await _sender.Send(new GetBookByIdQuery(id), ct);
        return CreatedAtAction(nameof(GetById), new { id }, book);
    }

    // PUT api/books/1 (logged-in users)
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateBookCommand command, CancellationToken ct)
    {
        await _sender.Send(command with { Id = id }, ct);
        return NoContent();
    }

    // DELETE api/books/1 (logged-in users)
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _sender.Send(new DeleteBookCommand(id), ct);
        return NoContent();
    }
}// public class BooksController : ControllerBase
// {
//      private readonly ISender _bookStore;


// //constructor injection, the standard DI pattern in .NET.
//     public BooksController(IBookStore bookStore)
//     {
//         _bookStore = bookStore;
//     }

//     // GET api/books
//     [HttpGet]
//     public async Task<ActionResult<IReadOnlyList<BookDto>>> GetAll()
//     {
//         var books = await _bookStore.GetAllAsync();
//         return Ok(books);
//     }

//     // GET api/books/1
//     [HttpGet("{id:int}")]
//     public async Task<ActionResult<BookDto>> GetById(int id)
//     {
//         var book = await _bookStore.GetByIdAsync(id);

//         if (book is null)
//             return NotFound();

//         return Ok(book);
//     }

//     // POST api/books
//     [HttpPost]
//     public async Task<ActionResult<BookDto>> Create(CreateBookRequest request)
//     {
//         var book = await _bookStore.AddAsync(request);
//         return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
//     }


//     // PUT api/books/1
//     [HttpPut("{id:int}")]
//      public async Task<IActionResult> Update(int id, CreateBookRequest request)
//     {
//         if (!await _bookStore.UpdateAsync(id, request))
//             return NotFound();

//         return NoContent();
//     }

//     // DELETE api/books/1
//     [HttpDelete("{id:int}")]
//     public async Task<IActionResult> Delete(int id)
//     {
//         if (!await _bookStore.DeleteAsync(id))
//             return NotFound();

//         return NoContent();
//     }
// }
