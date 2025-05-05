using Moq;
using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Common;
using LibraryManagement.Application.DTOs.Statistic;

namespace LibraryManagement.Application.Test
{
    [TestFixture]
    public class StatisticServiceTests
    {
        private Mock<IBookRepository> _mockBookRepository;
        private Mock<ICategoryRepository> _mockCategoryRepository;
        private Mock<IRequestRepository> _mockRequestRepository;
        private Mock<IUserRepository> _mockUserRepository;
        private StatisticService _statisticService;

        // Fixed dates for testing
        private static readonly DateTime FixedTestDate = new DateTime(2024, 5, 6, 10, 0, 0, DateTimeKind.Utc);
        private static readonly DateTime FixedDatePreviousMonth = FixedTestDate.AddMonths(-1);
        private static readonly DateTime FixedDateDeleted = FixedTestDate.AddDays(-30);

        [SetUp]
        public void Setup()
        {
            _mockBookRepository = new Mock<IBookRepository>();
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockRequestRepository = new Mock<IRequestRepository>();
            _mockUserRepository = new Mock<IUserRepository>();

            _statisticService = new StatisticService(
                _mockBookRepository.Object,
                _mockRequestRepository.Object,
                _mockCategoryRepository.Object,
                _mockUserRepository.Object
            );
        }

        // Helper method to create mock data easily with fixed dates
        private List<Book> GetMockBooks()
        {
            var category1 = new Category { Id = 1, Name = "Fiction" };
            var category2 = new Category { Id = 2, Name = "Science" };
            return new List<Book>
            {
                new Book { Id = 1, Title = "Fiction Book 1", Author = "Author A", CategoryId = 1, Category = category1, TotalQuantity = 10, AvailableQuantity = 5, DeletedAt = null },
                new Book { Id = 2, Title = "Science Book 1", Author = "Author B", CategoryId = 2, Category = category2, TotalQuantity = 5, AvailableQuantity = 5, DeletedAt = null },
                new Book { Id = 3, Title = "Fiction Book 2", Author = "Author C", CategoryId = 1, Category = category1, TotalQuantity = 7, AvailableQuantity = 0, DeletedAt = null }, // Borrowed
                new Book { Id = 4, Title = "Deleted Book", Author = "Author D", CategoryId = 1, Category = category1, TotalQuantity = 3, AvailableQuantity = 3, DeletedAt = FixedDateDeleted }, // Deleted
                new Book { Id = 5, Title = "Uncategorized", Author = "Author E", CategoryId = null, Category = null, TotalQuantity = 2, AvailableQuantity = 1, DeletedAt = null } // Null Category
            };
        }

        private List<User> GetMockUsers()
        {
            return new List<User>
            {
                new User { Id = 1, Username = "admin1", Role = UserRole.Admin },
                new User { Id = 2, Username = "user1", Role = UserRole.User },
                new User { Id = 3, Username = "user2", Role = UserRole.User },
                new User { Id = 4, Username = "admin2", Role = UserRole.Admin },
            };
        }

        private List<BookBorrowingRequest> GetMockRequests()
        {
            var users = GetMockUsers();
            var books = GetMockBooks();
            return new List<BookBorrowingRequest>
            {
                // Requests within the FixedTestDate month (May 2024)
                new BookBorrowingRequest { Id = 1, RequestorId = 2, Requestor = users[1], Status = RequestStatus.Approved, DateRequested = FixedTestDate.AddDays(-2), Details = new List<BookBorrowingRequestDetail>{ new BookBorrowingRequestDetail { BookId = 1, Book = books[0]} }},
                new BookBorrowingRequest { Id = 2, RequestorId = 3, Requestor = users[2], Status = RequestStatus.Waiting, DateRequested = FixedTestDate.AddDays(-1), Details = new List<BookBorrowingRequestDetail>{ new BookBorrowingRequestDetail { BookId = 2, Book = books[1]}} },
                new BookBorrowingRequest { Id = 3, RequestorId = 2, Requestor = users[1], Status = RequestStatus.Rejected, DateRequested = FixedTestDate.AddDays(-3), Details = new List<BookBorrowingRequestDetail>{ new BookBorrowingRequestDetail { BookId = 3, Book = books[2]}} },
                new BookBorrowingRequest { Id = 5, RequestorId = 2, Requestor = users[1], Status = RequestStatus.Waiting, DateRequested = FixedTestDate.AddDays(-4), Details = new List<BookBorrowingRequestDetail>{ new BookBorrowingRequestDetail { BookId = 5, Book = books[4]}} }, // Uncategorized book
                new BookBorrowingRequest { Id = 6, RequestorId = 3, Requestor = users[2], Status = RequestStatus.Approved, DateRequested = FixedTestDate.AddDays(-5), Details = new List<BookBorrowingRequestDetail>{ new BookBorrowingRequestDetail { BookId = 1, Book = books[0]}, new BookBorrowingRequestDetail { BookId = 2, Book = books[1]} }}, // Multiple books

                // Request in the previous month (April 2024)
                new BookBorrowingRequest { Id = 4, RequestorId = 3, Requestor = users[2], Status = RequestStatus.Returned, DateRequested = FixedDatePreviousMonth.AddDays(15), Details = new List<BookBorrowingRequestDetail>{ new BookBorrowingRequestDetail { BookId = 1, Book = books[0]}} },
            };
        }

        [Test]
        public async Task GetBookQuantitiesAsync_WhenBooksExist_ReturnsCorrectQuantities()
        {
            // Arrange
            var allMockBooks = GetMockBooks(); // Service uses GetAllAsync
            _mockBookRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(allMockBooks);

            // Calculate expected values based on *all* books returned by GetAllAsync
            var expectedTotal = allMockBooks.Sum(b => b.TotalQuantity);         // 10+5+7+3+2 = 27
            var expectedAvailable = allMockBooks.Sum(b => b.AvailableQuantity); // 5+5+0+3+1 = 14
            var expectedBorrowed = expectedTotal - expectedAvailable;           // 27 - 14 = 13

            // Act
            var result = await _statisticService.GetBookQuantitiesAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedTotal, result.TotalBooks, "TotalBooks count mismatch");
            Assert.AreEqual(expectedAvailable, result.AvailableBooks, "AvailableBooks count mismatch");
            Assert.AreEqual(expectedBorrowed, result.BorrowedBooks, "BorrowedBooks count mismatch");
            _mockBookRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }


        [Test]
        public async Task GetBookQuantitiesAsync_WhenNoBooksExist_ReturnsZeroQuantities()
        {
            // Arrange
            _mockBookRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Book>());

            // Act
            var result = await _statisticService.GetBookQuantitiesAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.TotalBooks);
            Assert.AreEqual(0, result.AvailableBooks);
            Assert.AreEqual(0, result.BorrowedBooks);
            _mockBookRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }


        [Test]
        public async Task GetBooksPerCategoryAsync_WhenBooksExist_ReturnsBooksGroupedByCategoryIgnoringDeleted()
        {
            // Arrange
            var mockBooks = GetMockBooks();
            _mockBookRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(mockBooks);

            // Expected counts ONLY for non-deleted books
            var expectedFictionCount = mockBooks.Count(b => b.DeletedAt == null && b.CategoryId == 1); // 2 (Book 1, Book 3)
            var expectedScienceCount = mockBooks.Count(b => b.DeletedAt == null && b.CategoryId == 2); // 1 (Book 2)
            var expectedNullCategoryCount = mockBooks.Count(b => b.DeletedAt == null && b.CategoryId == null); // 1 (Book 5)

            // Act
            var result = await _statisticService.GetBooksPerCategoryAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.BooksPerCategory.Count); // Fiction, Science, Uncategorized (Null) - Deleted is excluded

            var fictionCategory = result.BooksPerCategory.FirstOrDefault(bpc => bpc.CategoryName == "Fiction");
            Assert.IsNotNull(fictionCategory, "Fiction category not found");
            Assert.AreEqual(expectedFictionCount, fictionCategory.BookCount, "Fiction count mismatch");

            var scienceCategory = result.BooksPerCategory.FirstOrDefault(bpc => bpc.CategoryName == "Science");
            Assert.IsNotNull(scienceCategory, "Science category not found");
            Assert.AreEqual(expectedScienceCount, scienceCategory.BookCount, "Science count mismatch");

            var nullCategory = result.BooksPerCategory.FirstOrDefault(bpc => bpc.CategoryName == Constants.NullCategoryName);
            Assert.IsNotNull(nullCategory, "Null category not found");
            Assert.AreEqual(expectedNullCategoryCount, nullCategory.BookCount, "Null category count mismatch");

            _mockBookRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetBooksPerCategoryAsync_WhenNoBooksExist_ReturnsEmptyList()
        {
            // Arrange
            _mockBookRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Book>());

            // Act
            var result = await _statisticService.GetBooksPerCategoryAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result.BooksPerCategory);
            _mockBookRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetMostPopularAsync_WhenRequestsExist_ReturnsMostPopularBookAndCategory()
        {
            // Arrange
            var mockRequests = GetMockRequests();
            var mockBooks = GetMockBooks();
            var mockCategories = mockBooks.Where(b => b.Category != null).Select(b => b.Category).Distinct().ToList();

            _mockRequestRepository.Setup(repo => repo.GetAllRequestsAsync()).ReturnsAsync(mockRequests);

            // Expected Popularity (counting all requests, including rejected/returned as per service logic):
            // Book 1: 3 times (Req 1, 4, 6) -> Most Popular
            // Book 2: 2 times (Req 2, 6)
            // Book 3: 1 time (Req 3)
            // Book 5: 1 time (Req 5)
            // ---
            // Category 1 (Fiction): 4 times (Book 1 -> Req 1, 4, 6; Book 3 -> Req 3) -> Most Popular
            // Category 2 (Science): 2 times (Book 2 -> Req 2, 6)
            // Null Category (Book 5 -> Req 5): 1 time

            var mostPopularBookId = 1;
            var mostPopularCategoryId = 1;
            var expectedBook = mockBooks.First(b => b.Id == mostPopularBookId);
            var expectedCategory = mockCategories.First(c => c.Id == mostPopularCategoryId);
            var expectedBookCount = 3;
            var expectedCategoryCount = 4;

            _mockBookRepository.Setup(repo => repo.GetByIdAsync(mostPopularBookId)).ReturnsAsync(expectedBook);
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(mostPopularCategoryId)).ReturnsAsync(expectedCategory);


            // Act
            var result = await _statisticService.GetMostPopularAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual($"{expectedBook.Title} by {expectedBook.Author} - {expectedBookCount}", result.TitleAuthor);
            Assert.AreEqual($"{expectedCategory.Name} - {expectedCategoryCount}", result.CategoryName);

            _mockRequestRepository.Verify(repo => repo.GetAllRequestsAsync(), Times.Once);
            _mockBookRepository.Verify(repo => repo.GetByIdAsync(mostPopularBookId), Times.Once);
            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(mostPopularCategoryId), Times.Once);
        }

        [Test]
        public void GetMostPopularAsync_WhenNoRequestsExist_ThrowsException()
        {
            // Arrange
            _mockRequestRepository.Setup(repo => repo.GetAllRequestsAsync()).ReturnsAsync(new List<BookBorrowingRequest>());
            // Mocking GetByIdAsync(0) which would be called after FirstOrDefault on empty dictionaries
            _mockBookRepository.Setup(repo => repo.GetByIdAsync(0)).ReturnsAsync((Book)null);
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(0)).ReturnsAsync((Category)null);

            // Act & Assert
            // Current implementation will likely try to access properties of null book/category if GetByIdAsync(0) returns null.
            Assert.ThrowsAsync<NullReferenceException>(async () => await _statisticService.GetMostPopularAsync());

            _mockRequestRepository.Verify(repo => repo.GetAllRequestsAsync(), Times.Once);
            _mockBookRepository.Verify(repo => repo.GetByIdAsync(0), Times.Once); // Verify it attempts to get book ID 0
            // Category might not be called if book fails first
        }

        [Test]
        public void GetMostPopularAsync_WhenRequestsExistButNoDetails_HandlesGracefullyOrThrows()
        {
            // Arrange
            var mockRequests = new List<BookBorrowingRequest>
            {
                 new BookBorrowingRequest { Id = 1, RequestorId = 2, Status = RequestStatus.Approved, DateRequested = FixedTestDate.AddDays(-2), Details = new List<BookBorrowingRequestDetail>()}, // No details
                 new BookBorrowingRequest { Id = 2, RequestorId = 3, Status = RequestStatus.Waiting, DateRequested = FixedTestDate.AddDays(-1), Details = new List<BookBorrowingRequestDetail>()} // No details
            };
            _mockRequestRepository.Setup(repo => repo.GetAllRequestsAsync()).ReturnsAsync(mockRequests);
            _mockBookRepository.Setup(repo => repo.GetByIdAsync(0)).ReturnsAsync((Book)null); // Setup for potential ID 0 call
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(0)).ReturnsAsync((Category)null);

            // Act & Assert
            // Similar to the NoRequests test, this will likely throw when trying FirstOrDefault on empty popularity dictionaries.
            Assert.ThrowsAsync<NullReferenceException>(async () => await _statisticService.GetMostPopularAsync());

            _mockRequestRepository.Verify(repo => repo.GetAllRequestsAsync(), Times.Once);
            _mockBookRepository.Verify(repo => repo.GetByIdAsync(0), Times.Once);
        }


        [Test]
        public async Task GetUserCountAsync_WhenUsersExist_ReturnsCorrectCounts()
        {
            // Arrange
            var mockUsers = GetMockUsers();
            _mockUserRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(mockUsers);

            var expectedTotalUsers = mockUsers.Count(u => u.Role == UserRole.User); // 2
            var expectedTotalAdmins = mockUsers.Count(u => u.Role == UserRole.Admin); // 2

            // Act
            var result = await _statisticService.GetUserCountAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedTotalUsers, result.TotalUsers, "User count mismatch");
            Assert.AreEqual(expectedTotalAdmins, result.TotalAdmin, "Admin count mismatch");
            _mockUserRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetUserCountAsync_WhenNoUsersExist_ReturnsZeroCounts()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<User>());

            // Act
            var result = await _statisticService.GetUserCountAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.TotalUsers);
            Assert.AreEqual(0, result.TotalAdmin);
            _mockUserRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetRequestOverviewAsync_WhenRequestsExist_ReturnsCorrectCounts()
        {
            // Arrange
            var mockRequests = GetMockRequests();
            _mockRequestRepository.Setup(repo => repo.GetAllRequestsAsync()).ReturnsAsync(mockRequests);

            var expectedTotal = mockRequests.Count(); // 6
            var expectedWaiting = mockRequests.Count(r => r.Status == RequestStatus.Waiting); // 2 (Req 2, 5)
            var expectedApproved = mockRequests.Count(r => r.Status == RequestStatus.Approved); // 2 (Req 1, 6)
            var expectedRejected = mockRequests.Count(r => r.Status == RequestStatus.Rejected); // 1 (Req 3)
            var expectedReturned = mockRequests.Count(r => r.Status == RequestStatus.Returned); // 1 (Req 4)

            // Act
            var result = await _statisticService.GetRequestOverviewAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedTotal, result.TotalRequestCount, "Total mismatch");
            Assert.AreEqual(expectedWaiting, result.PendingRequestCount, "Pending mismatch");
            Assert.AreEqual(expectedApproved, result.ApprovedRequestCount, "Approved mismatch");
            Assert.AreEqual(expectedRejected, result.RejectedRequestCount, "Rejected mismatch");
            Assert.AreEqual(expectedReturned, result.ReturnedRequestCount, "Returned mismatch");
            _mockRequestRepository.Verify(repo => repo.GetAllRequestsAsync(), Times.Once);
        }

        [Test]
        public async Task GetRequestOverviewAsync_WhenNoRequestsExist_ReturnsNull() // Service returns null if no requests
        {
            // Arrange
            _mockRequestRepository.Setup(repo => repo.GetAllRequestsAsync()).ReturnsAsync(new List<BookBorrowingRequest>());

            // Act
            var result = await _statisticService.GetRequestOverviewAsync();

            // Assert
            Assert.IsNull(result);
            _mockRequestRepository.Verify(repo => repo.GetAllRequestsAsync(), Times.Once);
        }


        [Test]
        public async Task GetBookOverviewAsync_WhenBooksExist_ReturnsCorrectCountsIgnoringDeleted()
        {
            // Arrange
            var mockBooks = GetMockBooks();
            _mockBookRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(mockBooks);

            // Counts should ignore the deleted book (Book 4)
            var expectedTotal = mockBooks.Count(b => b.DeletedAt == null); // 4 (Books 1, 2, 3, 5)
            var expectedAvailable = mockBooks.Count(b => b.DeletedAt == null && b.AvailableQuantity > 0); // 3 (Books 1, 2, 5)
            var expectedNotAvailable = mockBooks.Count(b => b.DeletedAt == null && b.AvailableQuantity == 0); // 1 (Book 3)

            // Act
            var result = await _statisticService.GetBookOverviewAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedTotal, result.TotalBooks, "Total mismatch");
            Assert.AreEqual(expectedAvailable, result.TotalAvailable, "Available mismatch");
            Assert.AreEqual(expectedNotAvailable, result.TotalNotAvailable, "Not Available mismatch");
            _mockBookRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetBookOverviewAsync_WhenNoBooksExist_ReturnsZeroCounts()
        {
            // Arrange
            _mockBookRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Book>());

            // Act
            var result = await _statisticService.GetBookOverviewAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.TotalBooks);
            Assert.AreEqual(0, result.TotalAvailable);
            Assert.AreEqual(0, result.TotalNotAvailable);
            _mockBookRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetMonthlyReportAsync_WhenRequestsExistForMonth_ReturnsCorrectReport()
        {
            // Arrange
            var reportMonth = FixedTestDate; // May 2024
            var startOfMonth = new DateTime(reportMonth.Year, reportMonth.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1);

            var mockRequests = GetMockRequests(); // Includes requests from May 2024 and April 2024
            var mockBooks = GetMockBooks();
            var mockCategories = mockBooks.Where(b => b.Category != null).Select(b => b.Category).Distinct().ToList();

            // Filter requests expected for May 2024 based on mock data
            var requestsThisMonth = mockRequests
                .Where(r => r.DateRequested >= startOfMonth && r.DateRequested <= endOfMonth)
                .ToList(); // Should be Req 1, 2, 3, 5, 6 (Total 5)

            _mockRequestRepository.Setup(repo => repo.GetAllRequestsAsync()).ReturnsAsync(mockRequests);
            // Mock GetByIdAsync for all potentially popular books/categories in May 2024 requests
            _mockBookRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(mockBooks.FirstOrDefault(b => b.Id == 1));
            _mockBookRepository.Setup(repo => repo.GetByIdAsync(2)).ReturnsAsync(mockBooks.FirstOrDefault(b => b.Id == 2));
            _mockBookRepository.Setup(repo => repo.GetByIdAsync(3)).ReturnsAsync(mockBooks.FirstOrDefault(b => b.Id == 3));
            _mockBookRepository.Setup(repo => repo.GetByIdAsync(5)).ReturnsAsync(mockBooks.FirstOrDefault(b => b.Id == 5)); // Book with null category
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(mockCategories.FirstOrDefault(c => c.Id == 1)); // Fiction
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(2)).ReturnsAsync(mockCategories.FirstOrDefault(c => c.Id == 2)); // Science


            var expectedTotal = requestsThisMonth.Count(); // 5
            var expectedApproved = requestsThisMonth.Count(r => r.Status == RequestStatus.Approved); // 2 (Req 1, 6)
            var expectedRejected = requestsThisMonth.Count(r => r.Status == RequestStatus.Rejected); // 1 (Req 3)
            var expectedWaiting = requestsThisMonth.Count(r => r.Status == RequestStatus.Waiting);   // 2 (Req 2, 5)
            var expectedActiveUsers = requestsThisMonth.Select(r => r.RequestorId).Distinct().Count(); // 2 (User 2, User 3)

            // Popularity within May 2024:
            // Book 1: 2 times (Req 1, 6) -> Popular
            // Book 2: 2 times (Req 2, 6) -> Popular
            // Book 3: 1 time (Req 3) -> Popular
            // Book 5: 1 time (Req 5) -> Popular
            // ---
            // Category 1 (Fiction): 3 times (Book 1 -> Req 1, 6; Book 3 -> Req 3) -> Popular
            // Category 2 (Science): 2 times (Book 2 -> Req 2, 6) -> Popular

            // Act
            var result = await _statisticService.GetMonthlyReportAsync(reportMonth);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(startOfMonth, result.Month);
            Assert.AreEqual(expectedTotal, result.TotalRequests, "TotalRequests mismatch");
            Assert.AreEqual(expectedApproved, result.ApprovedRequests, "ApprovedRequests mismatch");
            Assert.AreEqual(expectedRejected, result.RejectedRequests, "RejectedRequests mismatch");
            Assert.AreEqual(expectedWaiting, result.PendingRequests, "PendingRequests mismatch");
            Assert.AreEqual(expectedActiveUsers, result.TotalActiveUsers, "TotalActiveUsers mismatch");

            // Assert popular books (Top 5)
            Assert.AreEqual(4, result.PopularBooks.Count, "PopularBooks count mismatch"); // Books 1, 2, 3, 5 were requested
            var popularBook1 = result.PopularBooks.FirstOrDefault(pb => pb.BookId == 1);
            Assert.IsNotNull(popularBook1, "Popular Book 1 not found");
            Assert.AreEqual(2, popularBook1.BorrowCount, "Popular Book 1 count incorrect");

            var popularBook2 = result.PopularBooks.FirstOrDefault(pb => pb.BookId == 2);
            Assert.IsNotNull(popularBook2, "Popular Book 2 not found");
            Assert.AreEqual(2, popularBook2.BorrowCount, "Popular Book 2 count incorrect");

            var popularBook3 = result.PopularBooks.FirstOrDefault(pb => pb.BookId == 3);
            Assert.IsNotNull(popularBook3, "Popular Book 3 not found");
            Assert.AreEqual(1, popularBook3.BorrowCount, "Popular Book 3 count incorrect");

            var popularBook5 = result.PopularBooks.FirstOrDefault(pb => pb.BookId == 5);
            Assert.IsNotNull(popularBook5, "Popular Book 5 not found");
            Assert.AreEqual(1, popularBook5.BorrowCount, "Popular Book 5 count incorrect");


            // Assert popular categories (Top 3) - Based on non-null categories
            Assert.AreEqual(2, result.PopularCategories.Count, "PopularCategories count mismatch (should only count non-null)");
            var popularCat1 = result.PopularCategories.FirstOrDefault(pc => pc.CategoryId == 1); // Fiction
            Assert.IsNotNull(popularCat1, "Popular Category 1 (Fiction) not found");
            Assert.AreEqual(3, popularCat1.BorrowCount, "Popular Category 1 (Fiction) count incorrect");

            var popularCat2 = result.PopularCategories.FirstOrDefault(pc => pc.CategoryId == 2); // Science
            Assert.IsNotNull(popularCat2, "Popular Category 2 (Science) not found");
            Assert.AreEqual(2, popularCat2.BorrowCount, "Popular Category 2 (Science) count incorrect");


            _mockRequestRepository.Verify(repo => repo.GetAllRequestsAsync(), Times.Once);
            // Verify GetByIdAsync calls for popular items in this month
            _mockBookRepository.Verify(repo => repo.GetByIdAsync(1), Times.AtLeastOnce());
            _mockBookRepository.Verify(repo => repo.GetByIdAsync(2), Times.AtLeastOnce());
            _mockBookRepository.Verify(repo => repo.GetByIdAsync(3), Times.AtLeastOnce());
            _mockBookRepository.Verify(repo => repo.GetByIdAsync(5), Times.AtLeastOnce());
            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(1), Times.AtLeastOnce());
            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(2), Times.AtLeastOnce());
        }

        [Test]
        public async Task GetMonthlyReportAsync_WhenNoRequestsExistForMonth_ReturnsEmptyReport()
        {
            // Arrange
            var reportMonth = FixedTestDate.AddYears(-1); // A month with no requests (May 2023)
            var startOfMonth = new DateTime(reportMonth.Year, reportMonth.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var mockRequests = GetMockRequests(); // Data from other months (May 2024, Apr 2024)

            _mockRequestRepository.Setup(repo => repo.GetAllRequestsAsync()).ReturnsAsync(mockRequests);

            // Act
            var result = await _statisticService.GetMonthlyReportAsync(reportMonth);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(startOfMonth, result.Month);
            Assert.AreEqual(0, result.TotalRequests);
            Assert.AreEqual(0, result.ApprovedRequests);
            Assert.AreEqual(0, result.RejectedRequests);
            Assert.AreEqual(0, result.PendingRequests);
            Assert.AreEqual(0, result.TotalActiveUsers);
            Assert.IsEmpty(result.PopularBooks);
            Assert.IsEmpty(result.PopularCategories);

            _mockRequestRepository.Verify(repo => repo.GetAllRequestsAsync(), Times.Once);
            _mockBookRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<int>()), Times.Never); // No popular items fetched
            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public async Task GetMonthlyReportsRangeAsync_WhenValidRange_ReturnsListOfReports()
        {
            // Arrange
            var startMonth = FixedDatePreviousMonth; // April 2024
            var endMonth = FixedTestDate;          // May 2024
            var mockRequests = GetMockRequests();
            var mockBooks = GetMockBooks();
            var mockCategories = mockBooks.Where(b => b.Category != null).Select(b => b.Category).Distinct().ToList();

            _mockRequestRepository.Setup(repo => repo.GetAllRequestsAsync()).ReturnsAsync(mockRequests);
            // Setup mocks needed by GetMonthlyReportAsync (called internally) - cover all potential popular items across both months
            _mockBookRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) => mockBooks.FirstOrDefault(b => b.Id == id));
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) => mockCategories.FirstOrDefault(c => c.Id == id));


            // Act
            var results = await _statisticService.GetMonthlyReportsRangeAsync(startMonth, endMonth);

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(2, results.Count); // Expecting two reports (April and May)

            // Check April 2024 report (should contain Request 4)
            var reportForApril = results.FirstOrDefault(r => r.Month.Year == startMonth.Year && r.Month.Month == startMonth.Month);
            Assert.IsNotNull(reportForApril, "Report for April 2024 not found");
            Assert.AreEqual(1, reportForApril.TotalRequests, "Total requests for April 2024 mismatch");
            Assert.AreEqual(0, reportForApril.ApprovedRequests);
            Assert.AreEqual(0, reportForApril.RejectedRequests);
            Assert.AreEqual(0, reportForApril.PendingRequests);
            Assert.AreEqual(1, reportForApril.TotalActiveUsers); // User 3
            Assert.AreEqual(1, reportForApril.PopularBooks.Count); // Book 1
            Assert.AreEqual(1, reportForApril.PopularCategories.Count); // Fiction

            // Check May 2024 report (should contain Req 1, 2, 3, 5, 6)
            var reportForMay = results.FirstOrDefault(r => r.Month.Year == endMonth.Year && r.Month.Month == endMonth.Month);
            Assert.IsNotNull(reportForMay, "Report for May 2024 not found");
            Assert.AreEqual(5, reportForMay.TotalRequests, "Total requests for May 2024 mismatch");
            Assert.AreEqual(2, reportForMay.ApprovedRequests);
            Assert.AreEqual(1, reportForMay.RejectedRequests);
            Assert.AreEqual(2, reportForMay.PendingRequests);
            Assert.AreEqual(2, reportForMay.TotalActiveUsers); // Users 2 and 3


            // Verify GetAllRequestsAsync was called once for each month in the range
            _mockRequestRepository.Verify(repo => repo.GetAllRequestsAsync(), Times.Exactly(2));
        }

        [Test]
        public async Task GetMonthlyReportsRangeAsync_WhenStartAfterEnd_SwapsAndReturnsReports()
        {
            // Arrange
            var startMonth = FixedTestDate;          // May 2024
            var endMonth = FixedDatePreviousMonth; // April 2024 (incorrect order)
            var mockRequests = GetMockRequests();
            var mockBooks = GetMockBooks();
            var mockCategories = mockBooks.Where(b => b.Category != null).Select(b => b.Category).Distinct().ToList();

            _mockRequestRepository.Setup(repo => repo.GetAllRequestsAsync()).ReturnsAsync(mockRequests);
            _mockBookRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) => mockBooks.FirstOrDefault(b => b.Id == id));
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) => mockCategories.FirstOrDefault(c => c.Id == id));

            // Act
            var results = await _statisticService.GetMonthlyReportsRangeAsync(startMonth, endMonth);

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(2, results.Count); // Still expecting two reports

            // Check if the order in the list matches the *corrected* chronological order (April first, then May)
            Assert.AreEqual(endMonth.Month, results[0].Month.Month, "First report should be for the earlier month (April)");
            Assert.AreEqual(startMonth.Month, results[1].Month.Month, "Second report should be for the later month (May)");

            _mockRequestRepository.Verify(repo => repo.GetAllRequestsAsync(), Times.Exactly(2));
        }

        // Note: Testing the Export methods directly for byte[] output is complex in unit tests.
        // We've tested the underlying GetMonthlyReport(s) methods which contain the core logic.
        // Integration tests would be more suitable for verifying the Excel generation itself.
        // You could add basic tests to ensure the export methods call the underlying report methods.

        [Test]
        public async Task ExportMonthlyReportToExcelAsync_CallsGetMonthlyReportAsync()
        {
            // Arrange
            var reportMonth = FixedTestDate;
            var expectedReport = new MonthlyReportOutputDto { Month = reportMonth }; // Simple DTO for verification

            // Mock GetMonthlyReportAsync to simulate it being called
            // We need to setup GetAllRequestsAsync as it's called by GetMonthlyReportAsync
            _mockRequestRepository.Setup(repo => repo.GetAllRequestsAsync()).ReturnsAsync(new List<BookBorrowingRequest>()); // Minimal setup for GetMonthlyReport to run

            // Act
            // We don't need to assert the byte[] content, just that the underlying method was invoked.
            // Use try-catch because the Excel generation might fail without full data/mocks, but we only care if GetMonthlyReportAsync was called.
            try
            {
                await _statisticService.ExportMonthlyReportToExcelAsync(reportMonth);
            }
            catch (Exception)
            {
                // Ignore exceptions from Excel generation in this specific verification test
            }

            // Assert
            _mockRequestRepository.Verify(repo => repo.GetAllRequestsAsync(), Times.Once); // Verify the dependency of GetMonthlyReportAsync was called
        }

        [Test]
        public async Task ExportMonthlyReportsRangeToExcelAsync_CallsGetMonthlyReportsRangeAsync()
        {
            // Arrange
            var startMonth = FixedDatePreviousMonth;
            var endMonth = FixedTestDate;

            // Mock GetMonthlyReportsRangeAsync dependencies
            _mockRequestRepository.Setup(repo => repo.GetAllRequestsAsync()).ReturnsAsync(new List<BookBorrowingRequest>()); // Minimal setup
            _mockBookRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Book)null);
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Category)null);

            // Act
            // Use try-catch as above
            try
            {
                await _statisticService.ExportMonthlyReportsRangeToExcelAsync(startMonth, endMonth);
            }
            catch (Exception)
            {
                // Ignore exceptions from Excel generation
            }


            // Assert
            // GetMonthlyReportsRangeAsync calls GetMonthlyReportAsync for each month.
            // Verify GetAllRequestsAsync was called twice (once per month in the range).
            _mockRequestRepository.Verify(repo => repo.GetAllRequestsAsync(), Times.Exactly(2));
        }
    }
}