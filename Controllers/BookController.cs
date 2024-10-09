using BookProject.Contracts.Book.Queries;
using BookProject.Contracts.Book.Requests;
using BookProject.Contracts.Book.Responses;
using BookProject.Events;
using BookProject.Models;
using BookProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost]
        public IActionResult CreateBook([FromBody] CreateBookRequest request)
        {
            var book = new Book(
                Guid.NewGuid(),
                request.Title,
                request.Description,
                request.Authors,
                request.PublishedDate
            );

            _bookService.CreateBook(book);

            return CreatedAtAction(nameof(CreateBook), new { id = book.Id }, MapBookResponse(book));
        }

        [HttpGet("{id}")]
        public IActionResult GetBook(Guid id)
        {
            var book = _bookService.GetBook(id);

            return Ok(MapBookResponse(book));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(UpdateBookRequest request, Guid id)
        {
            ValidateUpdateBookRequest(request, id);

            var book = new Book(
                id,
                request.Title,
                request.Description,
                request.Authors,
                request.PublishedDate
            );

            _bookService.UpdateBook(book);

            return CreatedAtAction(nameof(CreateBook), null);
        }

        [HttpGet("history")]
        public IActionResult GetBookHistory([FromQuery] BookHistorySearchQuery query)
        {
            var book = _bookService.GetBookHistory(query);
            if (book is null)
            {
                throw new KeyNotFoundException($"The key {query.Id} was not found in the library");
            }

            var output = book.ToDictionary(ele => ele.Key, ele => ComposeResponse(ele.Value as BookUpdated));

            return Ok(output);
        }

        private static void ValidateUpdateBookRequest(UpdateBookRequest request, Guid id)
        {
            if (id == null)
            {
                throw new ArgumentException($"The {nameof(id)} must have a value if added to the request");
            }

            if (request.Title == null && request.Description == null && request.Authors == null && request.PublishedDate == null)
            {
                throw new ArgumentException($"The update request must have at least one field meant to be updated");
            }

            if (request.Title != null && request.Title.Trim().Length == 0)
            {
                throw new ArgumentException($"The {nameof(request.Title)} must have a value if added to the request");
            }

            if (request.Description != null && request.Description.Trim().Length == 0)
            {
                throw new ArgumentException($"The {nameof(request.Description)} must have a value if added to the request");
            }

            if (request.Authors != null && !request.Authors.Any())
            {
                throw new ArgumentException($"The {nameof(request.Authors)} must have a value if added to the request");
            }
        }

        private static BookResponse MapBookResponse(Book book)
        {
            return new BookResponse(
                book.Id,
                book.Title,
                book.Description,
                book.Authors,
                book.PublishedDate
            );
        }

        private static string ComposeResponse(BookUpdated bookUpdated)
        {
            var result = string.Empty;
            result += bookUpdated.GetUpdatedValues();

            return result;
        }
    }
}
