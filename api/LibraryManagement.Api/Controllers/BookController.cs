using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        [Route("/api/books")]
        public async Task<IActionResult> GetBooks([FromQuery] int pageNum = 1, [FromQuery] int pageSize = 5)
        {
            var books = await _bookService.GetAllAsync(pageNum, pageSize);

            return Ok(books);
        }

        [HttpGet("{id}")]
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
        public async Task<IActionResult> CreateBook(CreateBookDto createBookDto)
        {
            var createdBook = await _bookService.CreateAsync(createBookDto);

            return Ok(createdBook);
        }

        [HttpPatch("{id}")]
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
