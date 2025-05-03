using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("overview")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetBookOverview()
        {
            var bookCount = await _bookService.GetBookCountAsync();
            return Ok(bookCount);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetBooks([FromQuery] int pageNum=Constants.DefaultPageNum, [FromQuery] int pageSize=Constants.DefaultPageSize)
        {
            var books = await _bookService.GetAllAsync(pageNum, pageSize);
            return Ok(books);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _bookService.GetByIdAsync(id);

            if (book is null)
            {
                return NotFound($"Book with ID {id} not found.");
            }

            return Ok(book);
        }
            
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateBook(CreateBookDto createBookDto)
        {
            var createdBook = await _bookService.CreateAsync(createBookDto);

            return Ok(createdBook);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBook(int id, UpdateBookDto updateBookDto)
        {
            var updatedBook = await _bookService.UpdateAsync(id, updateBookDto);
            if (updatedBook is null)
            {
                return NotFound($"Book with ID {id} not found.");
            }
            return Ok(updatedBook);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var deleted = await _bookService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound($"Book with ID {id} not found.");
            }

            return NoContent();
        }
    }
}
