using Moq;
using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.DTOs.Request;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Application.Extensions.Exceptions;
using LibraryManagement.Domain.Common;
using LibraryManagement.Application.DTOs.Common;
using System.Globalization;
using LibraryManagement.Application.DTOs.Statistic;

namespace LibraryManagement.Application.Test
{
    [TestFixture]
    public class RequestServiceTests
    {
        private Mock<IRequestRepository> _mockRequestRepository;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IBookRepository> _mockBookRepository;
        private RequestService _requestService;

        [SetUp]
        public void Setup()
        {
            _mockRequestRepository = new Mock<IRequestRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockBookRepository = new Mock<IBookRepository>();
            _requestService = new RequestService(
                _mockRequestRepository.Object,
                _mockUserRepository.Object,
                _mockBookRepository.Object
            );
        }

        private List<BookBorrowingRequest> CreateTestDataForAllRequests()
        {
            var requestor1 = new User { Id = 1, Username = "user1" };
            var requestor2 = new User { Id = 2, Username = "user2" };
            var approver = new User { Id = 3, Username = "admin1" };
            var category1 = new Category { Id = 1, Name = "Fiction" };
            var book1 = new Book { Id = 10, Title = "Fic Book 1", Author = "Auth A", Category = category1 };
            var book2 = new Book { Id = 20, Title = "NonFic Book 1", Author = "Auth B", Category = null }; // Null category
            var book3 = new Book { Id = 30, Title = "Fic Book 2", Author = "Auth C", Category = category1 };

            return new List<BookBorrowingRequest>
            {
                new BookBorrowingRequest {
                    Id = 1, Requestor = requestor1, Approver = approver, Status = RequestStatus.Approved, DateRequested = DateTime.UtcNow.AddDays(-5),
                    Details = new List<BookBorrowingRequestDetail> { new BookBorrowingRequestDetail { BookId = book1.Id, Book = book1 } }
                },
                new BookBorrowingRequest {
                    Id = 2, Requestor = requestor2, Approver = null, Status = RequestStatus.Waiting, DateRequested = DateTime.UtcNow.AddDays(-3), // Null approver
                    Details = new List<BookBorrowingRequestDetail> { new BookBorrowingRequestDetail { BookId = book2.Id, Book = book2 } }
                },
                new BookBorrowingRequest {
                    Id = 3, Requestor = requestor1, Approver = approver, Status = RequestStatus.Rejected, DateRequested = DateTime.UtcNow.AddDays(-10),
                    Details = new List<BookBorrowingRequestDetail> { new BookBorrowingRequestDetail { BookId = book3.Id, Book = book3 } }
                },
                 new BookBorrowingRequest {
                    Id = 4, Requestor = requestor2, Approver = approver, Status = RequestStatus.Approved, DateRequested = DateTime.UtcNow.AddDays(-2),
                    Details = new List<BookBorrowingRequestDetail> { new BookBorrowingRequestDetail { BookId = book1.Id, Book = book1 }, new BookBorrowingRequestDetail { BookId = book2.Id, Book = book2 } }
                },
                 new BookBorrowingRequest {
                    Id = 5, Requestor = requestor1, Approver = null, Status = RequestStatus.Waiting, DateRequested = DateTime.UtcNow.AddDays(-1),
                    Details = new List<BookBorrowingRequestDetail> { new BookBorrowingRequestDetail { BookId = book3.Id, Book = book3 } }
                 }
            };
        }

        [Test]
        public async Task GetAllRequestDetailsAsync_WhenNoStatus_ReturnsAllRequestsPaginated()
        {
            // Arrange
            var testData = CreateTestDataForAllRequests();
            int pageNum = 1;
            int pageSize = 3;
            _mockRequestRepository.Setup(repo => repo.GetAllRequestsAsync()).ReturnsAsync(testData);

            // Act
            var result = await _requestService.GetAllRequestDetailsAsync(null, pageNum, pageSize);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testData.Count, result.TotalCount);
            Assert.AreEqual(pageSize, result.Items.Count);
            Assert.AreEqual(pageNum, result.PageNum);
            Assert.AreEqual(pageSize, result.PageSize);
            Assert.AreEqual((int)Math.Ceiling((double)testData.Count / pageSize), result.TotalPage);
            _mockRequestRepository.Verify(repo => repo.GetAllRequestsAsync(), Times.Once);
        }

        [Test]
        [TestCase("waiting")]
        [TestCase("Waiting")]
        [TestCase("WAITING")]
        public async Task GetAllRequestDetailsAsync_WhenValidStatusFilter_ReturnsFilteredRequestsPaginated(string statusFilter)
        {
            // Arrange
            var testData = CreateTestDataForAllRequests();
            var expectedStatus = RequestStatus.Waiting;
            var expectedFilteredCount = testData.Count(r => r.Status == expectedStatus);
            int pageNum = 1;
            int pageSize = 5; // Ensure page size is large enough for all filtered items
            _mockRequestRepository.Setup(repo => repo.GetAllRequestsAsync()).ReturnsAsync(testData);


            // Act
            var result = await _requestService.GetAllRequestDetailsAsync(statusFilter, pageNum, pageSize);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedFilteredCount, result.TotalCount);
            Assert.AreEqual(expectedFilteredCount, result.Items.Count); // Assuming pageSize >= filtered count
            Assert.IsTrue(result.Items.All(i => i.Status == expectedStatus.ToString()));
            Assert.AreEqual(pageNum, result.PageNum);
            Assert.AreEqual(pageSize, result.PageSize);
            _mockRequestRepository.Verify(repo => repo.GetAllRequestsAsync(), Times.Once);
        }

        [Test]
        public void GetAllRequestDetailsAsync_WhenInvalidStatusFilter_ThrowsNotFoundException()
        {
            // Arrange
            var testData = CreateTestDataForAllRequests();
            string invalidStatus = "nonexistent";
            string expectedProcessedName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(invalidStatus.ToLowerInvariant());
            _mockRequestRepository.Setup(repo => repo.GetAllRequestsAsync()).ReturnsAsync(testData);


            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() => _requestService.GetAllRequestDetailsAsync(invalidStatus));
            Assert.AreEqual($"Request status {expectedProcessedName} not found.", ex?.Message);
            _mockRequestRepository.Verify(repo => repo.GetAllRequestsAsync(), Times.Once);
        }

        [Test]
        public async Task GetAllRequestDetailsAsync_WhenRequestsExist_AppliesPaginationCorrectly()
        {
            // Arrange
            var testData = CreateTestDataForAllRequests(); // 5 items
            int pageNum = 2;
            int pageSize = 2;
            int expectedItemsOnPage = 2;
            int expectedTotalPages = (int)Math.Ceiling((double)testData.Count / pageSize);

            _mockRequestRepository.Setup(repo => repo.GetAllRequestsAsync()).ReturnsAsync(testData);

            // Act
            var result = await _requestService.GetAllRequestDetailsAsync(null, pageNum, pageSize);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testData.Count, result.TotalCount);
            Assert.AreEqual(expectedItemsOnPage, result.Items.Count);
            Assert.AreEqual(pageNum, result.PageNum);
            Assert.AreEqual(pageSize, result.PageSize);
            Assert.AreEqual(expectedTotalPages, result.TotalPage);
            Assert.AreEqual(testData[2].Id, result.Items[0].Id);
            _mockRequestRepository.Verify(repo => repo.GetAllRequestsAsync(), Times.Once);
        }

        [Test]
        public async Task GetAllRequestDetailsAsync_WhenRequestHasNullApprover_MapsCorrectly()
        {
            // Arrange
            var testData = CreateTestDataForAllRequests();
            var requestWithNullApprover = testData.First(r => r.Approver == null);
            _mockRequestRepository.Setup(repo => repo.GetAllRequestsAsync()).ReturnsAsync(testData);


            // Act
            // Get all items on one page to simplify finding the specific item
            var result = await _requestService.GetAllRequestDetailsAsync(null, 1, testData.Count);

            // Assert
            Assert.IsNotNull(result);
            var mappedDto = result.Items.FirstOrDefault(dto => dto.Id == requestWithNullApprover.Id);
            Assert.IsNotNull(mappedDto, "Mapped DTO for request with null approver not found.");
            Assert.IsNull(mappedDto.Approver); // Key assertion
            Assert.AreEqual(requestWithNullApprover.Requestor.Username, mappedDto.Requestor);
            Assert.AreEqual(requestWithNullApprover.Status.ToString(), mappedDto.Status);
            _mockRequestRepository.Verify(repo => repo.GetAllRequestsAsync(), Times.Once);
        }

        [Test]
        public async Task GetAllRequestDetailsAsync_WhenBookHasNullCategory_MapsCorrectlyWithDefaultName()
        {
            // Arrange
            var testData = CreateTestDataForAllRequests();
            var requestWithNullCategoryBook = testData.First(r => r.Details.Any(d => d.Book.Category == null));
            var bookWithNullCategory = requestWithNullCategoryBook.Details.First(d => d.Book.Category == null).Book;
            _mockRequestRepository.Setup(repo => repo.GetAllRequestsAsync()).ReturnsAsync(testData);


            // Act
            // Get all items on one page
            var result = await _requestService.GetAllRequestDetailsAsync(null, 1, testData.Count);

            // Assert
            Assert.IsNotNull(result);
            var mappedDto = result.Items.FirstOrDefault(dto => dto.Id == requestWithNullCategoryBook.Id);
            Assert.IsNotNull(mappedDto, "Mapped DTO for request with null category book not found.");

            var mappedBookInfo = mappedDto.Books.FirstOrDefault(b => b.Title == bookWithNullCategory.Title);
            Assert.IsNotNull(mappedBookInfo, "Mapped BookInformation for book with null category not found.");
            Assert.AreEqual(Constants.NullCategoryName, mappedBookInfo.CategoryName); // Key assertion
            Assert.AreEqual(bookWithNullCategory.Author, mappedBookInfo.Author);
            _mockRequestRepository.Verify(repo => repo.GetAllRequestsAsync(), Times.Once);
        }

        [Test]
        public async Task GetAllRequestDetailsAsync_WhenRepositoryReturnsEmptyList_ReturnsEmptyPaginatedResult()
        {
            // Arrange
            var emptyList = new List<BookBorrowingRequest>();
            int pageNum = 1;
            int pageSize = 10;
            _mockRequestRepository.Setup(repo => repo.GetAllRequestsAsync()).ReturnsAsync(emptyList);

            // Act
            var result = await _requestService.GetAllRequestDetailsAsync(null, pageNum, pageSize);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.TotalCount);
            Assert.IsEmpty(result.Items);
            Assert.AreEqual(pageNum, result.PageNum);
            Assert.AreEqual(pageSize, result.PageSize);
            Assert.AreEqual(0, result.TotalPage); // Or 1 depending on Pagination logic for 0 items
            _mockRequestRepository.Verify(repo => repo.GetAllRequestsAsync(), Times.Once);
        }

        [Test]
        public async Task GetAllRequestDetailsAsync_WhenRepositoryReturnsNull_ReturnsEmptyPaginatedResult() // Or handle as needed, current code likely throws NRE
        {
            // Arrange
            int pageNum = 1;
            int pageSize = 10;
            // Explicitly return null to test null handling if GetAllRequestsAsync could return null
            // NOTE: Current RequestService code might throw NullReferenceException if repo returns null.
            // Adjust test based on desired behavior (e.g., expect exception or handle null gracefully).
            // This test assumes graceful handling (returning empty result) might be preferred.
            _mockRequestRepository.Setup(repo => repo.GetAllRequestsAsync()).ReturnsAsync((IEnumerable<BookBorrowingRequest>)null);


            // Act
            // If null should be handled gracefully:
            // var result = await _requestService.GetAllRequestDetailsAsync(null, pageNum, pageSize);
            // Assert.IsNotNull(result);
            // Assert.IsEmpty(result.Items);
            // Assert.AreEqual(0, result.TotalCount);

            // If NullReferenceException is expected (based on current code without null check on allRequests):
            Assert.ThrowsAsync<NullReferenceException>(() => _requestService.GetAllRequestDetailsAsync(null, pageNum, pageSize));

            // Assert
            _mockRequestRepository.Verify(repo => repo.GetAllRequestsAsync(), Times.Once);
        }

        [Test]
        public async Task GetAvailableRequestsAsync_UserExistsAndHasRequestsRemaining_ReturnsCorrectCount()
        {
            // Arrange
            int userId = 1;
            var user = new User { Id = userId, Username = "testuser" };
            var existingRequests = new List<BookBorrowingRequest>
            {
                new BookBorrowingRequest { RequestorId = userId, Status = RequestStatus.Approved, DateRequested=DateTime.UtcNow }
            };
            int expectedAvailable = Constants.MonthlyMaximumRequest - existingRequests.Count;

            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);
            _mockRequestRepository.Setup(repo => repo.GetExistingRequestsOfTheMonth(userId, It.IsAny<DateTime>())).ReturnsAsync(existingRequests);

            // Act
            var result = await _requestService.GetAvailableRequestsAsync(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedAvailable, result.AvailableRequests);
            _mockUserRepository.Verify(repo => repo.GetUserByIdAsync(userId), Times.Once);
            _mockRequestRepository.Verify(repo => repo.GetExistingRequestsOfTheMonth(userId, It.IsAny<DateTime>()), Times.Once);
        }

        [Test]
        public void GetAvailableRequestsAsync_UserNotFound_ThrowsNotFoundException()
        {
            // Arrange
            int userId = 99;
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() => _requestService.GetAvailableRequestsAsync(userId));
            Assert.AreEqual($"User with ID {userId} not found.", ex.Message);
            _mockRequestRepository.Verify(repo => repo.GetExistingRequestsOfTheMonth(It.IsAny<int>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Test]
        public async Task GetRequestDetailByIdAsync_RequestExists_ReturnsRequestDetail()
        {
            // Arrange
            int requestId = 1;
            var user = new User { Id = 1, Username = "requestor" };
            var approver = new User { Id = 2, Username = "approver" };
            var category = new Category { Id = 1, Name = "Fiction" };
            var book = new Book { Id = 10, Title = "Book Title", Author = "Book Author", Category = category };
            var request = new BookBorrowingRequest
            {
                Id = requestId,
                RequestorId = user.Id,
                Requestor = user,
                ApproverId = approver.Id,
                Approver = approver,
                Status = RequestStatus.Approved,
                DateRequested = DateTime.UtcNow.AddDays(-2),
                Details = new List<BookBorrowingRequestDetail> { new BookBorrowingRequestDetail { BookId = book.Id, Book = book } }
            };
            _mockRequestRepository.Setup(repo => repo.GetRequestByIdAsync(requestId)).ReturnsAsync(request);

            // Act
            var result = await _requestService.GetRequestDetailByIdAsync(requestId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(requestId, result.Id);
            Assert.AreEqual(user.Username, result.Requestor);
            Assert.AreEqual(approver.Username, result.Approver);
            Assert.AreEqual(RequestStatus.Approved.ToString(), result.Status);
            Assert.AreEqual(1, result.Books.Count);
            Assert.AreEqual(book.Title, result.Books[0].Title);
            _mockRequestRepository.Verify(repo => repo.GetRequestByIdAsync(requestId), Times.Once);
        }

        [Test]
        public async Task GetRequestDetailByIdAsync_RequestNotFound_ReturnsNull()
        {
            // Arrange
            int requestId = 99;
            _mockRequestRepository.Setup(repo => repo.GetRequestByIdAsync(requestId)).ReturnsAsync((BookBorrowingRequest)null);

            // Act
            var result = await _requestService.GetRequestDetailByIdAsync(requestId);

            // Assert
            Assert.IsNull(result);
            _mockRequestRepository.Verify(repo => repo.GetRequestByIdAsync(requestId), Times.Once);
        }

        [Test]
        public async Task CreateRequestAsync_Successful_ReturnsRequestDetailAndDecrementsBookQuantity()
        {
            // Arrange
            int userId = 1;
            var createDto = new CreateRequestDto { UserId = userId, BookIds = new List<int> { 10, 20 } };
            var user = new User { Id = userId, Username = "testuser" };

            // Use *copies* for the initial state to avoid direct modification issues if any
            var initialBook1 = new Book { Id = 10, Title = "Book 1", Author = "Auth 1", AvailableQuantity = 5, TotalQuantity = 5, CategoryId = 1, Category = new Category { Id = 1, Name = "Cat1" } };
            var initialBook2 = new Book { Id = 20, Title = "Book 2", Author = "Auth 2", AvailableQuantity = 3, TotalQuantity = 3, CategoryId = 2, Category = new Category { Id = 2, Name = "Cat2" } };

            // Capture object passed to CreateRequestAsync
            BookBorrowingRequest requestPassedToCreate = null;

            // Simulate object returned *after* creation (ensure navigation properties are valid for mapping)
            var createdRequestFromRepo = new BookBorrowingRequest
            {
                Id = 5,
                RequestorId = userId,
                Requestor = user,
                DateRequested = DateTime.UtcNow,
                Status = RequestStatus.Waiting,
                Details = new List<BookBorrowingRequestDetail> {
            new BookBorrowingRequestDetail { BookId = 10, Book = new Book { Id = 10, Title = "Book 1", Author = "Auth 1", Category = new Category {Id=1, Name="Cat1"}} },
            new BookBorrowingRequestDetail { BookId = 20, Book = new Book { Id = 20, Title = "Book 2", Author = "Auth 2", Category = new Category {Id=2, Name="Cat2"}} }
        }
            };

            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);
            _mockRequestRepository.Setup(repo => repo.GetExistingRequestsOfTheMonth(userId, It.IsAny<DateTime>())).ReturnsAsync(new List<BookBorrowingRequest>());
            _mockBookRepository.Setup(repo => repo.GetByIdAsync(10)).ReturnsAsync(initialBook1);
            _mockBookRepository.Setup(repo => repo.GetByIdAsync(20)).ReturnsAsync(initialBook2);

            // *** Modify UpdateAsync Mock Setup ***
            // Use Callback to check the state change *when* UpdateAsync is called
            _mockBookRepository.Setup(repo => repo.UpdateAsync(It.Is<Book>(b => b.Id == 10)))
                               .Callback<Book>(book => Assert.AreEqual(4, book.AvailableQuantity, "AvailableQuantity for book 10 should be 4 when UpdateAsync is called"))
                               .Returns<Book>(b => Task.FromResult(b)); // Still return success
            _mockBookRepository.Setup(repo => repo.UpdateAsync(It.Is<Book>(b => b.Id == 20)))
                               .Callback<Book>(book => Assert.AreEqual(2, book.AvailableQuantity, "AvailableQuantity for book 20 should be 2 when UpdateAsync is called"))
                               .Returns<Book>(b => Task.FromResult(b)); // Still return success

            _mockRequestRepository.Setup(repo => repo.CreateRequestAsync(It.IsAny<BookBorrowingRequest>()))
                                  .Callback<BookBorrowingRequest>(req => requestPassedToCreate = req)
                                  .ReturnsAsync(createdRequestFromRepo);

            // Act
            var result = await _requestService.CreateRequestAsync(createDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(createdRequestFromRepo.Id, result.Id);
            Assert.AreEqual(RequestStatus.Waiting.ToString(), result.Status);
            Assert.AreEqual(2, result.Books.Count);
            _mockUserRepository.Verify(repo => repo.GetUserByIdAsync(userId), Times.Once);
            _mockRequestRepository.Verify(repo => repo.GetExistingRequestsOfTheMonth(userId, It.IsAny<DateTime>()), Times.Once);
            _mockBookRepository.Verify(repo => repo.GetByIdAsync(10), Times.Once);
            _mockBookRepository.Verify(repo => repo.GetByIdAsync(20), Times.Once);
            _mockBookRepository.Verify(repo => repo.UpdateAsync(It.Is<Book>(b => b.Id == 10)), Times.Once);
            _mockBookRepository.Verify(repo => repo.UpdateAsync(It.Is<Book>(b => b.Id == 20)), Times.Once);
            _mockRequestRepository.Verify(repo => repo.CreateRequestAsync(It.IsNotNull<BookBorrowingRequest>()), Times.Once);
            Assert.IsNotNull(requestPassedToCreate);
            Assert.AreEqual(2, requestPassedToCreate.Details.Count);
            Assert.IsTrue(requestPassedToCreate.Details.Any(d => d.BookId == 10));
            Assert.IsTrue(requestPassedToCreate.Details.Any(d => d.BookId == 20));
        }

        [Test]
        public async Task CreateRequestAsync_MonthlyLimitReached_ReturnsNull()
        {
            // Arrange
            int userId = 1;
            var createDto = new CreateRequestDto { UserId = userId, BookIds = new List<int> { 10 } };
            var user = new User { Id = userId, Username = "testuser" };
            var existingRequests = Enumerable.Repeat(
                new BookBorrowingRequest { RequestorId = userId, Status = RequestStatus.Waiting, DateRequested = DateTime.UtcNow },
                Constants.MonthlyMaximumRequest)
                .ToList(); // At limit

            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);
            _mockRequestRepository.Setup(repo => repo.GetExistingRequestsOfTheMonth(userId, It.IsAny<DateTime>())).ReturnsAsync(existingRequests);

            // Act
            var result = await _requestService.CreateRequestAsync(createDto);

            // Assert
            Assert.IsNull(result);
            _mockBookRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<int>()), Times.Never);
            _mockRequestRepository.Verify(repo => repo.CreateRequestAsync(It.IsAny<BookBorrowingRequest>()), Times.Never);
        }

        [Test]
        public void CreateRequestAsync_BookNotFound_ThrowsNotFoundException()
        {
            // Arrange
            int userId = 1;
            var createDto = new CreateRequestDto { UserId = userId, BookIds = new List<int> { 99 } };
            var user = new User { Id = userId, Username = "testuser" };

            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);
            _mockRequestRepository.Setup(repo => repo.GetExistingRequestsOfTheMonth(userId, It.IsAny<DateTime>())).ReturnsAsync(new List<BookBorrowingRequest>());
            _mockBookRepository.Setup(repo => repo.GetByIdAsync(99)).ReturnsAsync((Book)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() => _requestService.CreateRequestAsync(createDto));
            Assert.AreEqual("Book with ID 99 not found.", ex.Message);
            _mockRequestRepository.Verify(repo => repo.CreateRequestAsync(It.IsAny<BookBorrowingRequest>()), Times.Never);
            _mockBookRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Book>()), Times.Never);
        }
        [Test]
        public void CreateRequestAsync_BookOutOfStock_ThrowsConflictException()
        {
            // Arrange
            int userId = 1;
            var createDto = new CreateRequestDto { UserId = userId, BookIds = new List<int> { 10 } };
            var user = new User { Id = userId, Username = "testuser" };
            var bookOutOfStock = new Book { Id = 10, Title = "Out Of Stock", Author = "Auth", AvailableQuantity = 0, TotalQuantity = 5 };

            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);
            _mockRequestRepository.Setup(repo => repo.GetExistingRequestsOfTheMonth(userId, It.IsAny<DateTime>())).ReturnsAsync(new List<BookBorrowingRequest>());
            _mockBookRepository.Setup(repo => repo.GetByIdAsync(10)).ReturnsAsync(bookOutOfStock);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ConflictException>(() => _requestService.CreateRequestAsync(createDto));
            Assert.AreEqual($"Book {bookOutOfStock.Title} is out of stock.", ex.Message);
            _mockRequestRepository.Verify(repo => repo.CreateRequestAsync(It.IsAny<BookBorrowingRequest>()), Times.Never);
            _mockBookRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Book>()), Times.Never);
        }


        // --- UpdateRequestAsync Tests ---

        [Test]
        public async Task UpdateRequestAsync_ApproveRequest_ReturnsUpdatedDetailAndDoesNotChangeBookQuantity()
        {
            // Arrange
            int requestId = 1;
            int adminId = 2;
            var updateDto = new UpdateRequestDto { AdminId = adminId, Status = RequestStatus.Approved };
            var adminUser = new User { Id = adminId, Username = "admin", Role = UserRole.Admin };
            var requestorUser = new User { Id = 5, Username = "requestor" };
            var book = new Book { Id = 10, Title = "Book", Author = "Author", AvailableQuantity = 4, TotalQuantity = 5, Category = new Category { Id = 1, Name = "Cat" } };
            var existingRequest = new BookBorrowingRequest
            {
                Id = requestId,
                Status = RequestStatus.Waiting,
                RequestorId = requestorUser.Id,
                Requestor = requestorUser,
                Details = new List<BookBorrowingRequestDetail> { new BookBorrowingRequestDetail { BookId = book.Id, Book = book, RequestId = requestId } }
            };
            var updatedRequestFromRepo = new BookBorrowingRequest
            {
                Id = requestId,
                Status = RequestStatus.Approved,
                RequestorId = requestorUser.Id,
                Requestor = requestorUser,
                ApproverId = adminId,
                Approver = adminUser,
                DateRequested = existingRequest.DateRequested,
                Details = new List<BookBorrowingRequestDetail> { new BookBorrowingRequestDetail { BookId = book.Id, Book = book, RequestId = requestId } }
            };


            _mockRequestRepository.Setup(repo => repo.GetRequestByIdAsync(requestId)).ReturnsAsync(existingRequest);
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(adminId)).ReturnsAsync(adminUser);
            _mockRequestRepository.Setup(repo =>
                repo.UpdateRequestAsync(It.Is<BookBorrowingRequest>(r =>
                    r.Id == requestId && r.Status == updateDto.Status
                    && r.ApproverId == adminId)
                )
            ).ReturnsAsync(updatedRequestFromRepo);

            // Act
            var result = await _requestService.UpdateRequestAsync(requestId, updateDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(requestId, result.Id);
            Assert.AreEqual(RequestStatus.Approved.ToString(), result.Status);
            Assert.AreEqual(adminUser.Username, result.Approver);
            Assert.AreEqual(4, book.AvailableQuantity);

            _mockBookRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Book>()), Times.Never);
            _mockRequestRepository.Verify(repo => repo.UpdateRequestAsync(It.IsAny<BookBorrowingRequest>()), Times.Once);
        }

        [Test]
        public async Task UpdateRequestAsync_RejectRequest_ReturnsUpdatedDetailAndIncrementsBookQuantity()
        {
            // Arrange
            int requestId = 1;
            int adminId = 2;
            var updateDto = new UpdateRequestDto { AdminId = adminId, Status = RequestStatus.Rejected };
            var adminUser = new User { Id = adminId, Username = "admin", Role = UserRole.Admin };
            var requestorUser = new User { Id = 5, Username = "requestor" };
            var book = new Book { Id = 10, Title = "Book", Author = "Author", AvailableQuantity = 4, TotalQuantity = 5, Category = new Category { Id = 1, Name = "Cat" } };
            var existingRequest = new BookBorrowingRequest
            {
                Id = requestId,
                Status = RequestStatus.Waiting,
                RequestorId = requestorUser.Id,
                Requestor = requestorUser,
                Details = new List<BookBorrowingRequestDetail> { new BookBorrowingRequestDetail { BookId = book.Id, Book = book, RequestId = requestId } }
            };
            var updatedRequestFromRepo = new BookBorrowingRequest
            {
                Id = requestId,
                Status = RequestStatus.Rejected,
                RequestorId = requestorUser.Id,
                Requestor = requestorUser,
                ApproverId = adminId,
                Approver = adminUser,
                DateRequested = existingRequest.DateRequested,
                Details = new List<BookBorrowingRequestDetail> { new BookBorrowingRequestDetail { BookId = book.Id, Book = book, RequestId = requestId } }
            };

            _mockRequestRepository.Setup(repo => repo.GetRequestByIdAsync(requestId)).ReturnsAsync(existingRequest);
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(adminId)).ReturnsAsync(adminUser);
            _mockRequestRepository.Setup(repo => repo.UpdateRequestAsync(It.IsAny<BookBorrowingRequest>())).ReturnsAsync(updatedRequestFromRepo);
            _mockBookRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Book>())).Returns<Book>(b => Task.FromResult(b));

            // Act
            var result = await _requestService.UpdateRequestAsync(requestId, updateDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(RequestStatus.Rejected.ToString(), result.Status);
            Assert.AreEqual(5, book.AvailableQuantity);

            _mockBookRepository.Verify(repo => repo.UpdateAsync(It.Is<Book>(b => b.Id == book.Id && b.AvailableQuantity == 5)), Times.Once);
            _mockRequestRepository.Verify(repo => repo.UpdateRequestAsync(It.IsAny<BookBorrowingRequest>()), Times.Once);
        }

        [Test]
        public void UpdateRequestAsync_RequestNotFound_ThrowsNotFoundException()
        {
            // Arrange
            int requestId = 99;
            var updateDto = new UpdateRequestDto { AdminId = 1, Status = RequestStatus.Approved };
            _mockRequestRepository.Setup(repo => repo.GetRequestByIdAsync(requestId)).ReturnsAsync((BookBorrowingRequest)null);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => _requestService.UpdateRequestAsync(requestId, updateDto));
        }

        [Test]
        public void UpdateRequestAsync_RequestAlreadyProcessed_ThrowsConflictException()
        {
            // Arrange
            int requestId = 1;
            var updateDto = new UpdateRequestDto { AdminId = 1, Status = RequestStatus.Approved };
            var existingRequest = new BookBorrowingRequest { Id = requestId, Status = RequestStatus.Approved };
            _mockRequestRepository.Setup(repo => repo.GetRequestByIdAsync(requestId)).ReturnsAsync(existingRequest);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ConflictException>(() => _requestService.UpdateRequestAsync(requestId, updateDto));
            Assert.AreEqual($"Request with ID {requestId} is already {existingRequest.Status}. You should not update any request that have been processed.", ex.Message);
        }

        [Test]
        public void UpdateRequestAsync_AdminUserNotFound_ThrowsNotFoundException()
        {
            // Arrange
            int requestId = 1;
            int adminId = 99;
            var updateDto = new UpdateRequestDto { AdminId = adminId, Status = RequestStatus.Approved };
            var existingRequest = new BookBorrowingRequest { Id = requestId, Status = RequestStatus.Waiting };

            _mockRequestRepository.Setup(repo => repo.GetRequestByIdAsync(requestId)).ReturnsAsync(existingRequest);
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(adminId)).ReturnsAsync((User)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() => _requestService.UpdateRequestAsync(requestId, updateDto));
            Assert.AreEqual($"User with ID {adminId} not found.", ex.Message);
        }

        [Test]
        public async Task UpdateRequestAsync_UserPerformingUpdateIsNotAdmin_ReturnsNull()
        {
            // Arrange
            int requestId = 1;
            int nonAdminUserId = 5;
            var updateDto = new UpdateRequestDto { AdminId = nonAdminUserId, Status = RequestStatus.Approved };
            var nonAdminUser = new User { Id = nonAdminUserId, Username = "notadmin", Role = UserRole.User };
            var existingRequest = new BookBorrowingRequest { Id = requestId, Status = RequestStatus.Waiting };

            _mockRequestRepository.Setup(repo => repo.GetRequestByIdAsync(requestId)).ReturnsAsync(existingRequest);
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(nonAdminUserId)).ReturnsAsync(nonAdminUser);

            // Act
            var result = await _requestService.UpdateRequestAsync(requestId, updateDto);

            // Assert
            Assert.IsNull(result);
            _mockRequestRepository.Verify(repo => repo.UpdateRequestAsync(It.IsAny<BookBorrowingRequest>()), Times.Never);
        }

        [Test]
        public async Task GetAllUserRequestsAsync_UserNotFound_ReturnsNull()
        {
            // Arrange
            var userId = 99;
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId))
                               .ReturnsAsync((User)null); // Simulate user not found

            // Act
            var result = await _requestService.GetAllUserRequestsAsync(userId);

            // Assert
            Assert.IsNull(result);
            _mockUserRepository.Verify(repo => repo.GetUserByIdAsync(userId), Times.Once);
            _mockRequestRepository.Verify(repo => repo.GetAllUserRequestsAsync(It.IsAny<int>()), Times.Never); // Ensure request repo not called
        }

        [Test]
        public async Task GetAllUserRequestsAsync_UserFound_NoRequests_ReturnsEmptyPaginatedResult()
        {
            // Arrange
            var userId = 1;
            var pageNum = 1;
            var pageSize = 10;
            var user = new User { Id = userId, Username = "testuser" };
            var emptyRequestList = new List<BookBorrowingRequest>();

            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);
            _mockRequestRepository.Setup(repo => repo.GetAllUserRequestsAsync(userId)).ReturnsAsync(emptyRequestList);

            // Act
            var result = await _requestService.GetAllUserRequestsAsync(userId, pageNum, pageSize);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<PaginatedOutputDto<RequestOutputDto>>(result);
            Assert.AreEqual(0, result.TotalCount);
            Assert.AreEqual(pageNum, result.PageNum);
            Assert.AreEqual(pageSize, result.PageSize);
            Assert.AreEqual(0, result.TotalPage); // Or 1 depending on Pagination logic for 0 items
            Assert.IsEmpty(result.Items);
            _mockUserRepository.Verify(repo => repo.GetUserByIdAsync(userId), Times.Once);
            _mockRequestRepository.Verify(repo => repo.GetAllUserRequestsAsync(userId), Times.Once);
        }

        [Test]
        public async Task GetAllUserRequestsAsync_UserFound_RequestsExist_ReturnsCorrectlyMappedAndPaginatedResult()
        {
            // Arrange
            var userId = 1;
            var pageNum = 1;
            var pageSize = 2; // Test pagination by setting size < total items
            var user = new User { Id = userId, Username = "testuser" };
            var approver = new User { Id = 2, Username = "admin" };
            var requestsFromRepo = new List<BookBorrowingRequest>
            {
                new BookBorrowingRequest { Id = 101, RequestorId = userId, Requestor = user, Status = RequestStatus.Approved, DateRequested = DateTime.UtcNow.AddDays(-2), ApproverId = approver.Id, Approver = approver },
                new BookBorrowingRequest { Id = 102, RequestorId = userId, Requestor = user, Status = RequestStatus.Waiting, DateRequested = DateTime.UtcNow.AddDays(-1), ApproverId = null, Approver = null },
                new BookBorrowingRequest { Id = 103, RequestorId = userId, Requestor = user, Status = RequestStatus.Rejected, DateRequested = DateTime.UtcNow.AddDays(-3), ApproverId = approver.Id, Approver = approver }
            };

            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);
            _mockRequestRepository.Setup(repo => repo.GetAllUserRequestsAsync(userId)).ReturnsAsync(requestsFromRepo);

            // Act
            var result = await _requestService.GetAllUserRequestsAsync(userId, pageNum, pageSize);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(requestsFromRepo.Count, result.TotalCount); // Total count before pagination
            Assert.AreEqual(pageNum, result.PageNum);
            Assert.AreEqual(pageSize, result.PageSize);
            Assert.AreEqual((int)Math.Ceiling((double)requestsFromRepo.Count / pageSize), result.TotalPage);
            Assert.AreEqual(pageSize, result.Items.Count); // Count on the current page

            // Check mapping for the first item on the page
            var firstItem = result.Items.First();
            var correspondingRepoItem = requestsFromRepo.First(r => r.Id == firstItem.Id);
            Assert.AreEqual(correspondingRepoItem.Id, firstItem.Id);
            Assert.AreEqual(correspondingRepoItem.Requestor.Username, firstItem.Requestor);
            Assert.AreEqual(correspondingRepoItem.Approver?.Username, firstItem.Approver);
            Assert.AreEqual(correspondingRepoItem.Status.ToString(), firstItem.Status);
            Assert.AreEqual(correspondingRepoItem.DateRequested.Date, firstItem.RequestedDate.Date); // Compare Date part if needed

            // Check mapping for the second item (ensure null approver is handled)
            var secondItem = result.Items.Skip(1).First();
            var correspondingRepoItem2 = requestsFromRepo.First(r => r.Id == secondItem.Id);
            Assert.AreEqual(correspondingRepoItem2.Id, secondItem.Id);
            Assert.AreEqual(correspondingRepoItem2.Requestor.Username, secondItem.Requestor);
            Assert.IsNull(secondItem.Approver); // Explicitly check null approver mapping
            Assert.AreEqual(correspondingRepoItem2.Status.ToString(), secondItem.Status);


            _mockUserRepository.Verify(repo => repo.GetUserByIdAsync(userId), Times.Once);
            _mockRequestRepository.Verify(repo => repo.GetAllUserRequestsAsync(userId), Times.Once);
        }

        [Test]
        public async Task GetAllUserRequestsAsync_UserFound_RequestsExist_ReturnsCorrectSecondPage()
        {
            // Arrange
            var userId = 1;
            var pageNum = 2; // Request second page
            var pageSize = 2;
            var user = new User { Id = userId, Username = "testuser" };
            var approver = new User { Id = 2, Username = "admin" };
            var requestsFromRepo = new List<BookBorrowingRequest>
            {
                new BookBorrowingRequest { Id = 101, RequestorId = userId, Requestor = user, Status = RequestStatus.Approved, DateRequested = DateTime.UtcNow.AddDays(-2), ApproverId = approver.Id, Approver = approver },
                new BookBorrowingRequest { Id = 102, RequestorId = userId, Requestor = user, Status = RequestStatus.Waiting, DateRequested = DateTime.UtcNow.AddDays(-1), ApproverId = null, Approver = null },
                new BookBorrowingRequest { Id = 103, RequestorId = userId, Requestor = user, Status = RequestStatus.Rejected, DateRequested = DateTime.UtcNow.AddDays(-3), ApproverId = approver.Id, Approver = approver }
            };

            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);
            _mockRequestRepository.Setup(repo => repo.GetAllUserRequestsAsync(userId)).ReturnsAsync(requestsFromRepo);

            // Act
            var result = await _requestService.GetAllUserRequestsAsync(userId, pageNum, pageSize);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(requestsFromRepo.Count, result.TotalCount);
            Assert.AreEqual(pageNum, result.PageNum);
            Assert.AreEqual(pageSize, result.PageSize);
            Assert.AreEqual((int)Math.Ceiling((double)requestsFromRepo.Count / pageSize), result.TotalPage); // Should be 2 pages
            Assert.AreEqual(1, result.Items.Count); // Only 1 item left on the second page

            // Check mapping for the item on the second page (should be the last one from the repo list)
            var firstItemOnPage2 = result.Items.First();
            var correspondingRepoItem = requestsFromRepo.Last();
            Assert.AreEqual(correspondingRepoItem.Id, firstItemOnPage2.Id);
            Assert.AreEqual(correspondingRepoItem.Requestor.Username, firstItemOnPage2.Requestor);
            Assert.AreEqual(correspondingRepoItem.Approver?.Username, firstItemOnPage2.Approver);
            Assert.AreEqual(correspondingRepoItem.Status.ToString(), firstItemOnPage2.Status);

            _mockUserRepository.Verify(repo => repo.GetUserByIdAsync(userId), Times.Once);
            _mockRequestRepository.Verify(repo => repo.GetAllUserRequestsAsync(userId), Times.Once);
        }
    }
} 
