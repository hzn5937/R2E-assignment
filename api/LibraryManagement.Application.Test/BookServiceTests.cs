using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.Extensions.Exceptions;
using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Common;
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
            _mockBookRepository = new Mock<IBookRepository>();
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _bookService = new BookService(_mockBookRepository.Object, _mockCategoryRepository.Object);
        }

        private List<Book> GetMockBooks()
        {
            // Creates a list of categories for books
            var category1 = new Category { Id = 1, Name = "Fiction" };
            var category2 = new Category { Id = 2, Name = "Science" };

            // Returns a list of sample books for testing
            return new List<Book>
            {
                new Book { Id = 1, Title = "The Great Novel", Author = "Author A", Category = category1, CategoryId = 1, TotalQuantity = 5, AvailableQuantity = 3, DeletedAt = null },
                new Book { Id = 2, Title = "Another Story", Author = "Author B", Category = category1, CategoryId = 1, TotalQuantity = 2, AvailableQuantity = 1, DeletedAt = null },
                new Book { Id = 3, Title = "Science Explained", Author = "Author C", Category = category2, CategoryId = 2, TotalQuantity = 10, AvailableQuantity = 10, DeletedAt = null },
                new Book { Id = 4, Title = "History Book", Author = "Author A", Category = category2, CategoryId = 2, TotalQuantity = 7, AvailableQuantity = 0, DeletedAt = null },
                new Book { Id = 5, Title = "Deleted Novel", Author = "Author D", Category = category1, CategoryId = 1, TotalQuantity = 3, AvailableQuantity = 3, DeletedAt = DateTime.UtcNow }, // Represents a deleted book
                new Book { Id = 6, Title = "Uncategorized Title", Author = "Author E", Category = null, CategoryId = null, TotalQuantity = 1, AvailableQuantity = 1, DeletedAt = null } // Represents a book with no category
            };
        }

        [Test]
        public async Task SearchBooksAsync_WithValidSearchTermMatchingTitle_ReturnsMatchingBooks()
        {
            // Arrange
            var searchTerm = "Novel"; // Should match book 1 title
            var mockBooks = GetMockBooks();
            // Setup mock repository to return the list of books
            _mockBookRepository.Setup(repo => repo.GetAllAsync())
                               .ReturnsAsync(mockBooks);

            // Act
            // Call the service method with the search term
            var result = await _bookService.SearchBooksAsync(searchTerm);

            // Assert
            // Verify the results are as expected
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.TotalCount); // Expecting one match
            Assert.AreEqual(1, result.Items.Count);
            Assert.IsTrue(result.Items.Any(b => b.Id == 1)); // Check if the correct book ID is present
            Assert.AreEqual("The Great Novel", result.Items.First().Title); // Check if the title matches
        }

        [Test]
        public async Task SearchBooksAsync_WithValidSearchTermMatchingAuthor_ReturnsMatchingBooks()
        {
            // Arrange
            var searchTerm = "author a"; // Should match books 1 and 4 by author (case-insensitive)
            var mockBooks = GetMockBooks();
            // Setup mock repository
            _mockBookRepository.Setup(repo => repo.GetAllAsync())
                              .ReturnsAsync(mockBooks);

            // Act
            var result = await _bookService.SearchBooksAsync(searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.TotalCount); // Expecting two matches
            Assert.AreEqual(2, result.Items.Count);
            Assert.IsTrue(result.Items.Any(b => b.Id == 1)); // Check for book 1
            Assert.IsTrue(result.Items.Any(b => b.Id == 4)); // Check for book 4
        }

        [Test]
        public async Task SearchBooksAsync_WithValidSearchTermMatchingCategory_ReturnsMatchingBooks()
        {
            // Arrange
            var searchTerm = "SCIENCE"; // Should match books 3 and 4 by category (case-insensitive)
            var mockBooks = GetMockBooks();
            // Setup mock repository
            _mockBookRepository.Setup(repo => repo.GetAllAsync())
                              .ReturnsAsync(mockBooks);

            // Act
            var result = await _bookService.SearchBooksAsync(searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.TotalCount); // Expecting two matches
            Assert.AreEqual(2, result.Items.Count);
            Assert.IsTrue(result.Items.Any(b => b.Id == 3)); // Check for book 3
            Assert.IsTrue(result.Items.Any(b => b.Id == 4)); // Check for book 4
                                                             // Verify all returned books belong to the 'Science' category
            Assert.IsTrue(result.Items.All(b => b.CategoryName.Equals("Science", StringComparison.OrdinalIgnoreCase)));
        }

        [Test]
        public async Task SearchBooksAsync_WithSearchTermMatchingNullCategoryName_ReturnsMatchingBook()
        {
            // Arrange
            // Use the constant defined for books without a category
            var searchTerm = Constants.NullCategoryName;
            var mockBooks = GetMockBooks();
            // Setup mock repository
            _mockBookRepository.Setup(repo => repo.GetAllAsync())
                               .ReturnsAsync(mockBooks);

            // Act
            var result = await _bookService.SearchBooksAsync(searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.TotalCount); // Expecting only book 6
            Assert.AreEqual(1, result.Items.Count);
            Assert.IsTrue(result.Items.Any(b => b.Id == 6)); // Check for book 6
            // Verify the category name matches the constant for null categories
            Assert.AreEqual(Constants.NullCategoryName, result.Items.First().CategoryName);
            // Verify the category ID is null
            Assert.IsNull(result.Items.First().CategoryId);
        }

        [Test]
        public async Task SearchBooksAsync_WithSearchTermNotMatchingAnyBooks_ReturnsEmpty()
        {
            // Arrange
            var searchTerm = "NonExistentTerm"; // A term that shouldn't match anything
            var mockBooks = GetMockBooks();
            // Setup mock repository
            _mockBookRepository.Setup(repo => repo.GetAllAsync())
                               .ReturnsAsync(mockBooks);

            // Act
            var result = await _bookService.SearchBooksAsync(searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.TotalCount); // Expecting zero matches
            Assert.AreEqual(0, result.Items.Count); // Expecting zero items
        }

        [Test]
        public async Task SearchBooksAsync_WithSearchTermMatchingDeletedBook_DoesNotReturnDeletedBook()
        {
            // Arrange
            var searchTerm = "Deleted Novel"; // Matches the title of book 5, which is deleted
            var mockBooks = GetMockBooks();
            // Setup mock repository
            _mockBookRepository.Setup(repo => repo.GetAllAsync())
                               .ReturnsAsync(mockBooks);

            // Act
            var result = await _bookService.SearchBooksAsync(searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.TotalCount); // Deleted books should be excluded
            Assert.AreEqual(0, result.Items.Count);
        }

        [Test]
        public async Task SearchBooksAsync_WithPagination_ReturnsCorrectPage()
        {
            // Arrange
            var searchTerm = "Author A"; // Matches books 1 and 4
            var pageNum = 2; // Request the second page
            var pageSize = 1; // With one item per page
            var mockBooks = GetMockBooks();
            // Setup mock repository
            _mockBookRepository.Setup(repo => repo.GetAllAsync())
                               .ReturnsAsync(mockBooks);

            // Act
            var result = await _bookService.SearchBooksAsync(searchTerm, pageNum, pageSize);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.TotalCount); // Total matches across all pages
            Assert.AreEqual(pageSize, result.Items.Count); // Items on the current page
            Assert.AreEqual(pageNum, result.PageNum); // Verify current page number
            Assert.AreEqual(pageSize, result.PageSize); // Verify page size
            Assert.AreEqual(2, result.TotalPage); // Calculate total pages
            // Assuming default ordering, book 4 should be on the second page
            Assert.IsTrue(result.Items.Any(b => b.Id == 4));
            // Book 1 should be on the first page, not the second
            Assert.IsFalse(result.Items.Any(b => b.Id == 1));
        }

        // Corrected Test: Handles delegation to GetAllAsync when search term is empty
        [Test]
        public async Task SearchBooksAsync_WithEmptySearchTerm_ReturnsAllNonDeletedBooksPaginatedCorrectly()
        {
            // Arrange
            string searchTerm = ""; // Empty search term triggers delegation
            var pageNum = 1;
            var pageSize = 3;
            var mockBooks = GetMockBooks();
            var nonDeletedBooks = mockBooks.Where(b => b.DeletedAt == null).ToList(); // Filter out deleted books for expectation
            var expectedTotalCount = nonDeletedBooks.Count;
            // Expected items on the first page
            var expectedItems = nonDeletedBooks.Take(pageSize).Select(book => new UserBookOutputDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                CategoryName = book.CategoryId == null ? Constants.NullCategoryName : book.Category.Name,
                CategoryId = book.CategoryId,
                TotalQuantity = book.TotalQuantity,
                AvailableQuantity = book.AvailableQuantity,
            }).ToList();

            // Setup the repository mock - SearchBooksAsync still calls this first
            _mockBookRepository.Setup(repo => repo.GetAllAsync())
                              .ReturnsAsync(mockBooks);

            // IMPORTANT: We are testing the SearchBooksAsync behavior.
            // Since it delegates to GetAllAsync (another method in BookService) when the term is empty,
            // we simulate the *expected outcome* of that delegation based on the mock data.
            // We assume BookService.GetAllAsync correctly filters deleted items and paginates.

            // Act
            var result = await _bookService.SearchBooksAsync(searchTerm, pageNum, pageSize);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedTotalCount, result.TotalCount); // Should count all non-deleted books (5)
            Assert.AreEqual(pageSize, result.Items.Count); // Should return the first page size (3)
            Assert.AreEqual(pageNum, result.PageNum);
            Assert.AreEqual(pageSize, result.PageSize);
            Assert.AreEqual((int)Math.Ceiling((double)expectedTotalCount / pageSize), result.TotalPage); // 5 items / 3 per page = 2 pages
            Assert.IsFalse(result.Items.Any(b => b.Id == 5)); // Ensure deleted book is not included
            // Compare the actual items with the expected items for the page
            CollectionAssert.AreEquivalent(expectedItems.Select(i => i.Id), result.Items.Select(i => i.Id));
        }

        // Corrected Test: Handles delegation to GetAllAsync when search term is whitespace
        [Test]
        public async Task SearchBooksAsync_WithWhitespaceSearchTerm_ReturnsAllNonDeletedBooksPaginatedCorrectly()
        {
            // Arrange
            string searchTerm = "   "; // Whitespace search term also triggers delegation
            var pageNum = 1;
            var pageSize = 5; // Get all in one page
            var mockBooks = GetMockBooks();
            var nonDeletedBooks = mockBooks.Where(b => b.DeletedAt == null).ToList(); // Filter out deleted books for expectation
            var expectedTotalCount = nonDeletedBooks.Count;
            // Expected items on the first page (all non-deleted items)
            var expectedItems = nonDeletedBooks.Take(pageSize).Select(book => new UserBookOutputDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                CategoryName = book.CategoryId == null ? Constants.NullCategoryName : book.Category.Name,
                CategoryId = book.CategoryId,
                TotalQuantity = book.TotalQuantity,
                AvailableQuantity = book.AvailableQuantity,
            }).ToList();

            // Setup the repository mock
            _mockBookRepository.Setup(repo => repo.GetAllAsync())
                             .ReturnsAsync(mockBooks);

            // Simulate the expected outcome of delegation to BookService.GetAllAsync

            // Act
            var result = await _bookService.SearchBooksAsync(searchTerm, pageNum, pageSize);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedTotalCount, result.TotalCount); // Total non-deleted
            // All non-deleted items fit on the page
            Assert.AreEqual(expectedTotalCount, result.Items.Count);
            Assert.AreEqual(pageNum, result.PageNum);
            Assert.AreEqual(pageSize, result.PageSize);
            Assert.AreEqual(1, result.TotalPage); // Only one page needed
            Assert.IsFalse(result.Items.Any(b => b.Id == 5)); // Ensure deleted book is not included
            // Compare the actual items with the expected items for the page
            CollectionAssert.AreEquivalent(expectedItems.Select(i => i.Id), result.Items.Select(i => i.Id));
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
            var result = await _bookService.GetAllAsync(1, 10);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.TotalCount);
            Assert.AreEqual(1, result.TotalPage);
            Assert.AreEqual(expectedDtoList.Count, result.Items.Count);
            Assert.AreEqual(expectedDtoList[0].Title, result.Items[0].Title);
            Assert.AreEqual(expectedDtoList[1].Author, result.Items[1].Author);
            _mockBookRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetAllAsync_ShouldSkipDeletedBooks()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Fiction" };
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Active Book", Author = "Author 1", CategoryId = 1, Category = category, AvailableQuantity = 5, TotalQuantity = 5, DeletedAt = null },
                new Book { Id = 2, Title = "Deleted Book", Author = "Author 2", CategoryId = 1, Category = category, AvailableQuantity = 0, TotalQuantity = 3, DeletedAt = DateTime.UtcNow }
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
            Assert.AreEqual(1, result.TotalCount);
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
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(createDto.CategoryId)).ReturnsAsync(category); 
            _mockBookRepository.Setup(repo => repo.CreateAsync(It.IsAny<Book>())).ReturnsAsync(createdBook);

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

            _mockBookRepository.Setup(repo => repo.GetByTitleAndAuthorAsync(createDto.Title, createDto.Author)).ReturnsAsync(existingDeletedBook);
            _mockBookRepository.Setup(repo => repo.UpdateAsync(It.Is<Book>(b => b.Id == existingDeletedBook.Id && b.DeletedAt == null))).ReturnsAsync(updatedBook); 

            // Act
            var result = await _bookService.CreateAsync(createDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(existingDeletedBook.Id, result.Id);
            Assert.AreEqual(existingDeletedBook.Title, result.Title);
            Assert.IsNull(result.DeletedAt); // Check it's undeleted
            _mockBookRepository.Verify(repo => repo.UpdateAsync(It.Is<Book>(b => b.Id == existingDeletedBook.Id && b.DeletedAt == null)), Times.Once);
            _mockBookRepository.Verify(repo => repo.CreateAsync(It.IsAny<Book>()), Times.Never); 
        }

        [Test]
        public void CreateAsync_WhenBookAlreadyExistsAndNotDeleted_ThrowsConflictException()
        {
            // Arrange
            var createDto = new CreateBookDto { Title = "Existing Active Book", Author = "Existing Author", CategoryId = 1 };
            var existingActiveBook = new Book { Id = 6, Title = createDto.Title, Author = createDto.Author, CategoryId = 1, DeletedAt = null };

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

            _mockBookRepository.Setup(repo => repo.GetByTitleAndAuthorAsync(updateDto.Title, updateDto.Author)).ReturnsAsync((Book)null); 
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
            Assert.AreEqual(8, result.AvailableQuantity); 
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

            _mockBookRepository.Setup(repo => repo.GetByTitleAndAuthorAsync(updateDto.Title, updateDto.Author)).ReturnsAsync(duplicateBook); 

            // Act & Assert
            Assert.ThrowsAsync<ConflictException>(() => _bookService.UpdateAsync(bookIdToUpdate, updateDto));
            _mockBookRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<int>()), Times.Never); 
            _mockBookRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Book>()), Times.Never);
        }

        [Test]
        public void UpdateAsync_WhenNewTotalQuantityIsInvalid_ThrowsConflictException()
        {
            // Arrange
            int bookId = 1;
            var updateDto = new UpdateBookDto { Title = "Updated Title", Author = "Updated Author", CategoryId = 1, TotalQuantity = 1 }; 
            var existingBook = new Book { Id = bookId, Title = "Old Title", Author = "Old Author", CategoryId = 1, TotalQuantity = 5, AvailableQuantity = 3 }; 

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
            _mockBookRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Book>())).Returns(Task.CompletedTask); 

            // Act
            var result = await _bookService.DeleteAsync(bookId);

            // Assert
            Assert.IsTrue(result);
            _mockBookRepository.Verify(repo => repo.DeleteAsync(It.Is<Book>(b => b.Id == bookId && b.DeletedAt != null)), Times.Once); 
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

        [Test]
        public async Task FilterBooksAsync_NoFilters_ReturnsAllNonDeletedBooksPaginated()
        {
            // Arrange
            var mockBooks = GetMockBooks();
            var nonDeletedBooks = mockBooks.Where(b => b.DeletedAt == null).ToList();
            var expectedTotalCount = nonDeletedBooks.Count;
            var pageNum = 1;
            var pageSize = 3;
            // Setup mock repository
            _mockBookRepository.Setup(repo => repo.GetAllAsync())
                               .ReturnsAsync(mockBooks);

            // Act
            var result = await _bookService.FilterBooksAsync(categoryId: null, isAvailable: null, pageNum: pageNum, pageSize: pageSize);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedTotalCount, result.TotalCount); // Should be 5 (book 5 is deleted)
            Assert.AreEqual(pageSize, result.Items.Count); // Items on page 1
            Assert.AreEqual(pageNum, result.PageNum);
            Assert.AreEqual(pageSize, result.PageSize);
            Assert.AreEqual((int)Math.Ceiling((double)expectedTotalCount / pageSize), result.TotalPage); // 5 / 3 = 2 pages
            Assert.IsFalse(result.Items.Any(b => b.Id == 5)); // Ensure deleted book is excluded
            // Check items on page 1 (assuming default order: 1, 2, 3)
            CollectionAssert.AreEquivalent(new[] { 1, 2, 3 }, result.Items.Select(i => i.Id));
        }

        [Test]
        public async Task FilterBooksAsync_ByCategory_ReturnsMatchingBooks()
        {
            // Arrange
            var categoryIdToFilter = 1; // Fiction
            var mockBooks = GetMockBooks();
            // Setup mock repository
            _mockBookRepository.Setup(repo => repo.GetAllAsync())
                               .ReturnsAsync(mockBooks);

            // Act
            var result = await _bookService.FilterBooksAsync(categoryId: categoryIdToFilter);

            // Assert
            Assert.IsNotNull(result);
            // Expected: Books 1, 2 (Book 5 is deleted)
            Assert.AreEqual(2, result.TotalCount);
            Assert.AreEqual(2, result.Items.Count);
            Assert.IsTrue(result.Items.All(b => b.CategoryId == categoryIdToFilter));
            Assert.IsTrue(result.Items.Any(b => b.Id == 1));
            Assert.IsTrue(result.Items.Any(b => b.Id == 2));
            Assert.IsFalse(result.Items.Any(b => b.Id == 5)); // Ensure deleted book is excluded
        }

        [Test]
        public async Task FilterBooksAsync_ByIsAvailableTrue_ReturnsAvailableBooks()
        {
            // Arrange
            var mockBooks = GetMockBooks();
            // Setup mock repository
            _mockBookRepository.Setup(repo => repo.GetAllAsync( ))
                               .ReturnsAsync(mockBooks);

            // Act
            var result = await _bookService.FilterBooksAsync(isAvailable: true);

            // Assert
            Assert.IsNotNull(result);
            // Expected: Books 1, 2, 3, 6 (Book 4 is unavailable, Book 5 is deleted)
            Assert.AreEqual(4, result.TotalCount);
            Assert.AreEqual(4, result.Items.Count);
            Assert.IsTrue(result.Items.All(b => b.AvailableQuantity > 0));
            Assert.IsTrue(result.Items.Any(b => b.Id == 1));
            Assert.IsTrue(result.Items.Any(b => b.Id == 2));
            Assert.IsTrue(result.Items.Any(b => b.Id == 3));
            Assert.IsTrue(result.Items.Any(b => b.Id == 6));
            Assert.IsFalse(result.Items.Any(b => b.Id == 4)); // Ensure unavailable book is excluded
            Assert.IsFalse(result.Items.Any(b => b.Id == 5)); // Ensure deleted book is excluded
        }

        [Test]
        public async Task FilterBooksAsync_ByIsAvailableFalse_ReturnsUnavailableBooks()
        {
            // Arrange
            var mockBooks = GetMockBooks();
            // Setup mock repository
            _mockBookRepository.Setup(repo => repo.GetAllAsync())
                               .ReturnsAsync(mockBooks);

            // Act
            var result = await _bookService.FilterBooksAsync(isAvailable: false);

            // Assert
            Assert.IsNotNull(result);
            // Expected: Book 4 (Book 5 is deleted)
            Assert.AreEqual(1, result.TotalCount);
            Assert.AreEqual(1, result.Items.Count);
            Assert.IsTrue(result.Items.All(b => b.AvailableQuantity <= 0));
            Assert.IsTrue(result.Items.Any(b => b.Id == 4));
            Assert.IsFalse(result.Items.Any(b => b.Id == 5)); // Ensure deleted book is excluded
        }

        [Test]
        public async Task FilterBooksAsync_ByCategoryAndIsAvailableTrue_ReturnsMatchingBooks()
        {
            // Arrange
            var categoryIdToFilter = 2; // Science
            var mockBooks = GetMockBooks();
            // Setup mock repository
            _mockBookRepository.Setup(repo => repo.GetAllAsync())
                               .ReturnsAsync(mockBooks);

            // Act
            var result = await _bookService.FilterBooksAsync(categoryId: categoryIdToFilter, isAvailable: true);

            // Assert
            Assert.IsNotNull(result);
            // Expected: Book 3 (Book 4 is Science but unavailable, Book 5 is deleted)
            Assert.AreEqual(1, result.TotalCount);
            Assert.AreEqual(1, result.Items.Count);
            Assert.IsTrue(result.Items.All(b => b.CategoryId == categoryIdToFilter && b.AvailableQuantity > 0));
            Assert.IsTrue(result.Items.Any(b => b.Id == 3));
        }

        [Test]
        public async Task FilterBooksAsync_ByCategoryAndIsAvailableFalse_ReturnsMatchingBooks()
        {
            // Arrange
            var categoryIdToFilter = 2; // Science
            var mockBooks = GetMockBooks();
            // Setup mock repository
            _mockBookRepository.Setup(repo => repo.GetAllAsync())
                               .ReturnsAsync(mockBooks);

            // Act
            var result = await _bookService.FilterBooksAsync(categoryId: categoryIdToFilter, isAvailable: false);

            // Assert
            Assert.IsNotNull(result);
            // Expected: Book 4 (Book 3 is Science but available, Book 5 is deleted)
            Assert.AreEqual(1, result.TotalCount);
            Assert.AreEqual(1, result.Items.Count);
            Assert.IsTrue(result.Items.All(b => b.CategoryId == categoryIdToFilter && b.AvailableQuantity <= 0));
            Assert.IsTrue(result.Items.Any(b => b.Id == 4));
        }

        [Test]
        public async Task FilterBooksAsync_FilterYieldsNoResults_ReturnsEmpty()
        {
            // Arrange
            var categoryIdToFilter = 1; // Fiction
            var mockBooks = GetMockBooks();
            // Setup mock repository
            _mockBookRepository.Setup(repo => repo.GetAllAsync())
                               .ReturnsAsync(mockBooks);

            // Act
            // Filter for Fiction books that are unavailable (none exist in mock data)
            var result = await _bookService.FilterBooksAsync(categoryId: categoryIdToFilter, isAvailable: false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.TotalCount);
            Assert.AreEqual(0, result.Items.Count);
        }

        [Test]
        public async Task FilterBooksAsync_WithPagination_ReturnsCorrectPage()
        {
            // Arrange
            var pageNum = 2;
            var pageSize = 2;
            var mockBooks = GetMockBooks();
            // Setup mock repository
            _mockBookRepository.Setup(repo => repo.GetAllAsync())
                               .ReturnsAsync(mockBooks);

            // Act
            // No filters, get page 2 with size 2
            var result = await _bookService.FilterBooksAsync(categoryId: null, isAvailable: null, pageNum: pageNum, pageSize: pageSize);

            // Assert
            Assert.IsNotNull(result);
            // Expected total non-deleted: 5 (1, 2, 3, 4, 6)
            Assert.AreEqual(5, result.TotalCount);
            Assert.AreEqual(pageSize, result.Items.Count); // Items on page 2
            Assert.AreEqual(pageNum, result.PageNum);
            Assert.AreEqual(pageSize, result.PageSize);
            Assert.AreEqual((int)Math.Ceiling(5.0 / pageSize), result.TotalPage); // 5 / 2 = 3 pages
            // Expected items on page 2 (assuming default order 1,2 | 3,4 | 6)
            CollectionAssert.AreEquivalent(new[] { 3, 4 }, result.Items.Select(i => i.Id));
        }

    }
}

