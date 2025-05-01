using Moq;
using LibraryManagement.Api.Controllers;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.DTOs.Book;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Application.Extensions.Exceptions;
using LibraryManagement.Application.DTOs.Common; 

namespace LibraryManagement.Api.Test
{
    [TestFixture]
    public class BookControllerTests
    {
        private Mock<IBookService> _mockBookService;
        private BookController _bookController;

        [SetUp]
        public void Setup()
        {
            _mockBookService = new Mock<IBookService>();
            _bookController = new BookController(_mockBookService.Object);
        }

        [Test]
        public async Task GetBooks_ReturnsOkResult_WithPaginatedBooks()
        {
            // Arrange
            var paginatedBooks = new PaginatedOutputDto<UserBookOutputDto>
            {
                Items = new List<UserBookOutputDto> { new UserBookOutputDto { Id = 1, Title = "Test Book" } },
                TotalCount = 1,
                PageNum = 1,
                PageSize = 10,
                TotalPage = 1
            };
            _mockBookService.Setup(service => service.GetAllAsync(1, 10)).ReturnsAsync(paginatedBooks);

            // Act
            var result = await _bookController.GetBooks(1, 10);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(paginatedBooks, okResult.Value);
            _mockBookService.Verify(s => s.GetAllAsync(1, 10), Times.Once);
        }

        [Test]
        public async Task GetBookById_WhenBookExists_ReturnsOkResult()
        {
            // Arrange
            int bookId = 1;
            var bookDto = new BookDetailOutputDto { Id = bookId, Title = "Test Book" };
            _mockBookService.Setup(service => service.GetByIdAsync(bookId)).ReturnsAsync(bookDto);

            // Act
            var result = await _bookController.GetBookById(bookId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(bookDto, okResult.Value);
            _mockBookService.Verify(s => s.GetByIdAsync(bookId), Times.Once);
        }

        [Test]
        public async Task GetBookById_WhenBookDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            int bookId = 99;
            _mockBookService.Setup(service => service.GetByIdAsync(bookId)).ReturnsAsync((BookDetailOutputDto)null);

            // Act
            var result = await _bookController.GetBookById(bookId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual($"Book with ID {bookId} not found.", notFoundResult.Value);
            _mockBookService.Verify(s => s.GetByIdAsync(bookId), Times.Once);
        }

        [Test]
        public async Task CreateBook_WhenSuccessful_ReturnsOkResult()
        {
            // Arrange
            var createDto = new CreateBookDto { Title = "New Book", Author = "New Author", CategoryId = 1 };
            var createdDto = new BookDetailOutputDto { Id = 10, Title = createDto.Title, Author = createDto.Author, CategoryName = "Fiction" };
            _mockBookService.Setup(service => service.CreateAsync(createDto)).ReturnsAsync(createdDto);

            // Act
            var result = await _bookController.CreateBook(createDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(createdDto, okResult.Value);
            _mockBookService.Verify(s => s.CreateAsync(createDto), Times.Once);
        }

        [Test]
        public void CreateBook_WhenConflictOccurs_ThrowsConflictException() 
        {
            // Arrange
            var createDto = new CreateBookDto { Title = "Existing Book", Author = "Existing Author", CategoryId = 1 };
            var conflictMessage = "Book already exists!";
            _mockBookService.Setup(service => service.CreateAsync(createDto))
                           .ThrowsAsync(new ConflictException(conflictMessage));

            // Act & Assert
            Assert.ThrowsAsync<ConflictException>(async () => await _bookController.CreateBook(createDto));
            _mockBookService.Verify(s => s.CreateAsync(createDto), Times.Once);
        }

        [Test]
        public async Task UpdateBook_WhenSuccessful_ReturnsOkResult()
        {
            // Arrange
            int bookId = 1;
            var updateDto = new UpdateBookDto { Title = "Updated", Author = "Updated", CategoryId = 1, TotalQuantity = 5 };
            var updatedDto = new BookDetailOutputDto { Id = bookId, Title = updateDto.Title, Author = updateDto.Author, CategoryName = "Fiction" };
            _mockBookService.Setup(service => service.UpdateAsync(bookId, updateDto)).ReturnsAsync(updatedDto);

            // Act
            var result = await _bookController.UpdateBook(bookId, updateDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(updatedDto, okResult.Value);
            _mockBookService.Verify(s => s.UpdateAsync(bookId, updateDto), Times.Once);
        }

        [Test]
        public void UpdateBook_WhenBookNotFound_ThrowsNotFoundExceptionWithMessage() 
        {
            // Arrange
            int bookId = 99;
            var updateDto = new UpdateBookDto { Title = "Update", Author = "Update", CategoryId = 1, TotalQuantity = 5 };
            var expectedMessage = $"Book with ID {bookId} not found."; 
            _mockBookService.Setup(service => service.UpdateAsync(bookId, updateDto)).ThrowsAsync(new NotFoundException(expectedMessage));

            // Act
            var ex = Assert.ThrowsAsync<NotFoundException>(async () => await _bookController.UpdateBook(bookId, updateDto));

            // Assert
            Assert.IsNotNull(ex);
            Assert.AreEqual(expectedMessage, ex.Message);
            _mockBookService.Verify(s => s.UpdateAsync(bookId, updateDto), Times.Once);
        }

        [Test]
        public async Task DeleteBook_WhenSuccessful_ReturnsNoContentResult()
        {
            // Arrange
            int bookId = 1;
            _mockBookService.Setup(service => service.DeleteAsync(bookId)).ReturnsAsync(true);

            // Act
            var result = await _bookController.DeleteBook(bookId);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
            _mockBookService.Verify(s => s.DeleteAsync(bookId), Times.Once);
        }

        [Test]
        public async Task DeleteBook_WhenBookNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            int bookId = 99;
            _mockBookService.Setup(service => service.DeleteAsync(bookId)).ReturnsAsync(false);

            // Act
            var result = await _bookController.DeleteBook(bookId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual($"Book with ID {bookId} not found.", notFoundResult.Value);
            _mockBookService.Verify(s => s.DeleteAsync(bookId), Times.Once);
        }
    }
}