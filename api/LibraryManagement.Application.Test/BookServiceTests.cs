using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.Extensions.Exceptions;
using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using Moq;

namespace LibraryManagement.Application.Test
{
    [TestFixture]
    public class BookServiceTests
    {
        private Mock<IBookRepository> _mockBookRepository;
        private Mock<ICategoryRepository> _mockCategoryRepository;
        private BookService _bookService;

        [SetUp]
        public void Setup()
        {
            // Initialize mocks before each test
            _mockBookRepository = new Mock<IBookRepository>();
            _mockCategoryRepository = new Mock<ICategoryRepository>();

            // Create instance of the service with mocked dependencies
            _bookService = new BookService(_mockBookRepository.Object, _mockCategoryRepository.Object);
        }

        [Test]
        public async Task GetAllAsync_WhenBooksExist_ReturnsPaginatedBooks()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Fiction" };
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Test Book 1", Author = "Author 1", CategoryId = 1, Category = category, AvailableQuantity = 5, TotalQuantity = 5, DeletedAt = null },
                new Book { Id = 2, Title = "Test Book 2", Author = "Author 2", CategoryId = 1, Category = category, AvailableQuantity = 3, TotalQuantity = 3, DeletedAt = null }
            };
            var expectedDtoList = new List<UserBookOutputDto>
            {
                new UserBookOutputDto { Id = 1, Title = "Test Book 1", Author = "Author 1", CategoryName = "Fiction", AvailableQuantity = 5 },
                new UserBookOutputDto { Id = 2, Title = "Test Book 2", Author = "Author 2", CategoryName = "Fiction", AvailableQuantity = 3 }
            };

            _mockBookRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(books);

            // Act
            var result = await _bookService.GetAllAsync(1, 10); // pageNum 1, pageSize 10

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.TotalCount);
            Assert.AreEqual(1, result.TotalPage);
            Assert.AreEqual(expectedDtoList.Count, result.Items.Count);
            Assert.AreEqual(expectedDtoList[0].Title, result.Items[0].Title);
            Assert.AreEqual(expectedDtoList[1].Author, result.Items[1].Author);
            _mockBookRepository.Verify(repo => repo.GetAllAsync(), Times.Once); // Verify the repo method was called
        }

        [Test]
        public async Task GetAllAsync_ShouldSkipDeletedBooks()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Fiction" };
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Active Book", Author = "Author 1", CategoryId = 1, Category = category, AvailableQuantity = 5, TotalQuantity = 5, DeletedAt = null },
                new Book { Id = 2, Title = "Deleted Book", Author = "Author 2", CategoryId = 1, Category = category, AvailableQuantity = 0, TotalQuantity = 3, DeletedAt = DateTime.UtcNow } // Deleted book
            };
            var expectedDtoList = new List<UserBookOutputDto>
            {
                 new UserBookOutputDto { Id = 1, Title = "Active Book", Author = "Author 1", CategoryName = "Fiction", AvailableQuantity = 5 }
             };

            _mockBookRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(books);

            // Act
            var result = await _bookService.GetAllAsync(1, 10);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.TotalCount); // Only the active book should be counted
            Assert.AreEqual(1, result.Items.Count);
            Assert.AreEqual(expectedDtoList[0].Title, result.Items[0].Title);
            _mockBookRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_WhenBookExists_ReturnsBookDetail()
        {
            // Arrange
            int bookId = 1;
            var category = new Category { Id = 1, Name = "Fiction" };
            var book = new Book { Id = bookId, Title = "Test Book", Author = "Test Author", Category = category, TotalQuantity = 5, AvailableQuantity = 3 };
            _mockBookRepository.Setup(repo => repo.GetByIdAsync(bookId)).ReturnsAsync(book);

            // Act
            var result = await _bookService.GetByIdAsync(bookId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(bookId, result.Id);
            Assert.AreEqual(book.Title, result.Title);
            Assert.AreEqual(book.Author, result.Author);
            Assert.AreEqual(book.Category.Name, result.CategoryName);
            _mockBookRepository.Verify(repo => repo.GetByIdAsync(bookId), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_WhenBookDoesNotExist_ReturnsNull()
        {
            // Arrange
            int bookId = 99;
            _mockBookRepository.Setup(repo => repo.GetByIdAsync(bookId)).ReturnsAsync((Book)null);

            // Act
            var result = await _bookService.GetByIdAsync(bookId);

            // Assert
            Assert.IsNull(result);
            _mockBookRepository.Verify(repo => repo.GetByIdAsync(bookId), Times.Once);
        }

        [Test]
        public async Task CreateAsync_WhenBookDoesNotExist_CreatesAndReturnsBook()
        {
            // Arrange
            var createDto = new CreateBookDto { Title = "New Book", Author = "New Author", CategoryId = 1 };
            var category = new Category { Id = 1, Name = "Fiction" };
            var createdBook = new Book { Id = 10, Title = createDto.Title, Author = createDto.Author, CategoryId = createDto.CategoryId, Category = category, TotalQuantity = 1, AvailableQuantity = 1 };

            _mockBookRepository.Setup(repo => repo.GetByTitleAndAuthorAsync(createDto.Title, createDto.Author)).ReturnsAsync((Book)null); // No existing book
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(createDto.CategoryId)).ReturnsAsync(category); // Category exists
            _mockBookRepository.Setup(repo => repo.CreateAsync(It.IsAny<Book>())).ReturnsAsync(createdBook); // Mock creation

            // Act
            var result = await _bookService.CreateAsync(createDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(createdBook.Id, result.Id);
            Assert.AreEqual(createdBook.Title, result.Title);
            Assert.AreEqual(createdBook.Category.Name, result.CategoryName);
            _mockBookRepository.Verify(repo => repo.CreateAsync(It.Is<Book>(b => b.Title == createDto.Title)), Times.Once);
        }

        [Test]
        public async Task CreateAsync_WhenBookIsSoftDeleted_UndeletesAndReturnsBook()
        {
            // Arrange
            var createDto = new CreateBookDto { Title = "Existing Deleted Book", Author = "Existing Author", CategoryId = 1 };
            var category = new Category { Id = 1, Name = "Fiction" };
            var existingDeletedBook = new Book { Id = 5, Title = createDto.Title, Author = createDto.Author, CategoryId = createDto.CategoryId, Category = category, TotalQuantity = 2, AvailableQuantity = 0, DeletedAt = DateTime.UtcNow.AddDays(-1) };
            var updatedBook = new Book { Id = 5, Title = createDto.Title, Author = createDto.Author, CategoryId = createDto.CategoryId, Category = category, TotalQuantity = 2, AvailableQuantity = 0, DeletedAt = null }; // Should be undeleted

            _mockBookRepository.Setup(repo => repo.GetByTitleAndAuthorAsync(createDto.Title, createDto.Author)).ReturnsAsync(existingDeletedBook); // Found existing deleted book
            _mockBookRepository.Setup(repo => repo.UpdateAsync(It.Is<Book>(b => b.Id == existingDeletedBook.Id && b.DeletedAt == null))).ReturnsAsync(updatedBook); // Mock update (undelete)

            // Act
            var result = await _bookService.CreateAsync(createDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(existingDeletedBook.Id, result.Id);
            Assert.AreEqual(existingDeletedBook.Title, result.Title);
            Assert.IsNull(result.DeletedAt); // Check it's undeleted
            _mockBookRepository.Verify(repo => repo.UpdateAsync(It.Is<Book>(b => b.Id == existingDeletedBook.Id && b.DeletedAt == null)), Times.Once);
            _mockBookRepository.Verify(repo => repo.CreateAsync(It.IsAny<Book>()), Times.Never); // Ensure CreateAsync was NOT called
        }

        [Test]
        public void CreateAsync_WhenBookAlreadyExistsAndNotDeleted_ThrowsConflictException()
        {
            // Arrange
            var createDto = new CreateBookDto { Title = "Existing Active Book", Author = "Existing Author", CategoryId = 1 };
            var existingActiveBook = new Book { Id = 6, Title = createDto.Title, Author = createDto.Author, CategoryId = 1, DeletedAt = null }; // Active book

            _mockBookRepository.Setup(repo => repo.GetByTitleAndAuthorAsync(createDto.Title, createDto.Author)).ReturnsAsync(existingActiveBook);

            // Act & Assert
            Assert.ThrowsAsync<ConflictException>(() => _bookService.CreateAsync(createDto));
            _mockBookRepository.Verify(repo => repo.CreateAsync(It.IsAny<Book>()), Times.Never);
            _mockBookRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Book>()), Times.Never);
        }

        [Test]
        public async Task UpdateAsync_WhenBookExistsAndNoConflict_UpdatesAndReturnsBook()
        {
            // Arrange
            int bookId = 1;
            var updateDto = new UpdateBookDto { Title = "Updated Title", Author = "Updated Author", CategoryId = 2, TotalQuantity = 10 };
            var existingBook = new Book { Id = bookId, Title = "Old Title", Author = "Old Author", CategoryId = 1, TotalQuantity = 5, AvailableQuantity = 3 };
            var newCategory = new Category { Id = 2, Name = "Science" };
            var updatedBook = new Book { Id = bookId, Title = updateDto.Title, Author = updateDto.Author, CategoryId = updateDto.CategoryId, Category = newCategory, TotalQuantity = updateDto.TotalQuantity, AvailableQuantity = 8 }; // 3 + (10 - 5) = 8

            _mockBookRepository.Setup(repo => repo.GetByTitleAndAuthorAsync(updateDto.Title, updateDto.Author)).ReturnsAsync((Book)null); // No duplicate title/author
            _mockBookRepository.Setup(repo => repo.GetByIdAsync(bookId)).ReturnsAsync(existingBook);
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(updateDto.CategoryId)).ReturnsAsync(newCategory);
            _mockBookRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Book>())).ReturnsAsync(updatedBook);

            // Act
            var result = await _bookService.UpdateAsync(bookId, updateDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(updateDto.Title, result.Title);
            Assert.AreEqual(updateDto.Author, result.Author);
            Assert.AreEqual(updateDto.TotalQuantity, result.TotalQuantity);
            Assert.AreEqual(8, result.AvailableQuantity); // Check calculated available quantity
            Assert.AreEqual(newCategory.Name, result.CategoryName);
            _mockBookRepository.Verify(repo => repo.UpdateAsync(It.Is<Book>(b => b.Id == bookId && b.Title == updateDto.Title)), Times.Once);
        }

        [Test]
        public void UpdateAsync_WhenBookNotFound_ThrowsNotFoundException()
        {
            // Arrange
            int bookId = 99;
            var updateDto = new UpdateBookDto { Title = "Any Title", Author = "Any Author", CategoryId = 1, TotalQuantity = 5 };

            _mockBookRepository.Setup(repo => repo.GetByTitleAndAuthorAsync(updateDto.Title, updateDto.Author)).ReturnsAsync((Book)null);
            _mockBookRepository.Setup(repo => repo.GetByIdAsync(bookId)).ReturnsAsync((Book)null); // Book not found

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => _bookService.UpdateAsync(bookId, updateDto));
            _mockBookRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Book>()), Times.Never);
        }

        [Test]
        public void UpdateAsync_WhenDuplicateTitleAuthorExists_ThrowsConflictException()
        {
            // Arrange
            int bookIdToUpdate = 1;
            int duplicateBookId = 2;
            var updateDto = new UpdateBookDto { Title = "Duplicate Title", Author = "Duplicate Author", CategoryId = 1, TotalQuantity = 5 };
            var duplicateBook = new Book { Id = duplicateBookId, Title = updateDto.Title, Author = updateDto.Author, CategoryId = 1 };

            _mockBookRepository.Setup(repo => repo.GetByTitleAndAuthorAsync(updateDto.Title, updateDto.Author)).ReturnsAsync(duplicateBook); // Duplicate found

            // Act & Assert
            Assert.ThrowsAsync<ConflictException>(() => _bookService.UpdateAsync(bookIdToUpdate, updateDto));
            _mockBookRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<int>()), Times.Never); // GetById should not be called if duplicate found first
            _mockBookRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Book>()), Times.Never);
        }

        [Test]
        public void UpdateAsync_WhenNewTotalQuantityIsInvalid_ThrowsConflictException()
        {
            // Arrange
            int bookId = 1;
            var updateDto = new UpdateBookDto { Title = "Updated Title", Author = "Updated Author", CategoryId = 1, TotalQuantity = 1 }; // New total is less than currently borrowed
            var existingBook = new Book { Id = bookId, Title = "Old Title", Author = "Old Author", CategoryId = 1, TotalQuantity = 5, AvailableQuantity = 3 }; // 2 books are borrowed (5 - 3)

            _mockBookRepository.Setup(repo => repo.GetByTitleAndAuthorAsync(updateDto.Title, updateDto.Author)).ReturnsAsync((Book)null);
            _mockBookRepository.Setup(repo => repo.GetByIdAsync(bookId)).ReturnsAsync(existingBook);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ConflictException>(() => _bookService.UpdateAsync(bookId, updateDto));
            Assert.That(ex.Message, Contains.Substring("Minimum value of new total quantity is '2'")); // Check specific error message if needed
            _mockBookRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Book>()), Times.Never);
        }


        [Test]
        public async Task DeleteAsync_WhenBookExists_ReturnsTrue()
        {
            // Arrange
            int bookId = 1;
            var existingBook = new Book { Id = bookId, Title = "Test", Author = "Test Author" };
            _mockBookRepository.Setup(repo => repo.GetByIdAsync(bookId)).ReturnsAsync(existingBook);
            _mockBookRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Book>())).Returns(Task.CompletedTask); // Mock deletion

            // Act
            var result = await _bookService.DeleteAsync(bookId);

            // Assert
            Assert.IsTrue(result);
            _mockBookRepository.Verify(repo => repo.DeleteAsync(It.Is<Book>(b => b.Id == bookId && b.DeletedAt != null)), Times.Once); // Verify soft delete
        }

        [Test]
        public async Task DeleteAsync_WhenBookDoesNotExist_ReturnsFalse()
        {
            // Arrange
            int bookId = 99;
            _mockBookRepository.Setup(repo => repo.GetByIdAsync(bookId)).ReturnsAsync((Book)null);

            // Act
            var result = await _bookService.DeleteAsync(bookId);

            // Assert
            Assert.IsFalse(result);
            _mockBookRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Book>()), Times.Never);
        }
    }
}

