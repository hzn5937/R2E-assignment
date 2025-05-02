using Moq;
using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.DTOs.Request;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Application.Extensions.Exceptions;
using LibraryManagement.Domain.Common;
using LibraryManagement.Application.DTOs.Common;

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
        public async Task GetRequestOverviewAsync_NoRequestsFound_ReturnsNull()
        {
            // Arrange
            _mockRequestRepository.Setup(repo => repo.GetAllRequestsAsync())
                                 .ReturnsAsync(new List<BookBorrowingRequest>()); // Return empty list

            // Act
            var result = await _requestService.GetRequestOverviewAsync();

            // Assert
            Assert.IsNull(result);
            _mockRequestRepository.Verify(repo => repo.GetAllRequestsAsync(), Times.Once);
        }

        [Test]
        public async Task GetRequestOverviewAsync_RepositoryReturnsNull_ReturnsNull()
        {
            // Arrange
            _mockRequestRepository.Setup(repo => repo.GetAllRequestsAsync())
                                 .ReturnsAsync((IEnumerable<BookBorrowingRequest>)null); // Return null

            // Act
            var result = await _requestService.GetRequestOverviewAsync();

            // Assert
            Assert.IsNull(result);
            _mockRequestRepository.Verify(repo => repo.GetAllRequestsAsync(), Times.Once);
        }


        [Test]
        public async Task GetRequestOverviewAsync_RequestsExist_ReturnsCorrectOverviewDto()
        {
            // Arrange
            var requests = new List<BookBorrowingRequest>
            {
                new BookBorrowingRequest { Id = 1, Status = RequestStatus.Waiting },
                new BookBorrowingRequest { Id = 2, Status = RequestStatus.Approved },
                new BookBorrowingRequest { Id = 3, Status = RequestStatus.Approved },
                new BookBorrowingRequest { Id = 4, Status = RequestStatus.Rejected },
                new BookBorrowingRequest { Id = 5, Status = RequestStatus.Waiting },
                new BookBorrowingRequest { Id = 6, Status = RequestStatus.Waiting },
            };
            _mockRequestRepository.Setup(repo => repo.GetAllRequestsAsync()).ReturnsAsync(requests);

            // Act
            var result = await _requestService.GetRequestOverviewAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<RequestOverviewOutputDto>(result);
            Assert.AreEqual(6, result.TotalRequestCount);
            Assert.AreEqual(3, result.PendingRequestCount); // Waiting status
            Assert.AreEqual(2, result.ApprovedRequestCount);
            Assert.AreEqual(1, result.RejectedRequestCount);
            _mockRequestRepository.Verify(repo => repo.GetAllRequestsAsync(), Times.Once);
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

        [Test]
        public async Task GetAllRequestDetailsAsync_NoRequests_ReturnsEmptyPaginatedResult()
        {
            // Arrange
            var pageNum = 1;
            var pageSize = 10;
            var emptyRequestList = new List<BookBorrowingRequest>();

            _mockRequestRepository.Setup(repo => repo.GetAllRequestsAsync()).ReturnsAsync(emptyRequestList);

            // Act
            var result = await _requestService.GetAllRequestDetailsAsync(pageNum, pageSize);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<PaginatedOutputDto<RequestDetailOutputDto>>(result);
            Assert.AreEqual(0, result.TotalCount);
            Assert.AreEqual(pageNum, result.PageNum);
            Assert.AreEqual(pageSize, result.PageSize);
            Assert.AreEqual(0, result.TotalPage); // Or 1 depending on Pagination logic
            Assert.IsEmpty(result.Items);
            _mockRequestRepository.Verify(repo => repo.GetAllRequestsAsync(), Times.Once);
        }

        [Test]
        public async Task GetAllRequestDetailsAsync_RequestsExist_ReturnsCorrectlyMappedAndPaginatedResult()
        {
            // Arrange
            var pageNum = 1;
            var pageSize = 1; // Test pagination
            var requestorUser = new User { Id = 1, Username = "reqUser" };
            var approverUser = new User { Id = 2, Username = "appUser" };
            var category1 = new Category { Id = 1, Name = "Fiction" };
            var book1 = new Book { Id = 101, Title = "Book A", Author = "Auth A", Category = category1 };
            var book2 = new Book { Id = 102, Title = "Book B", Author = "Auth B", Category = null }; // Book with null category

            var requestsFromRepo = new List<BookBorrowingRequest>
            {
                new BookBorrowingRequest
                {
                    Id = 201,
                    RequestorId = requestorUser.Id,
                    Requestor = requestorUser,
                    ApproverId = approverUser.Id,
                    Approver = approverUser,
                    Status = RequestStatus.Approved,
                    DateRequested = DateTime.UtcNow.AddDays(-5),
                    Details = new List<BookBorrowingRequestDetail>
                    {
                        new BookBorrowingRequestDetail { RequestId = 201, BookId = book1.Id, Book = book1 },
                        new BookBorrowingRequestDetail { RequestId = 201, BookId = book2.Id, Book = book2 }
                    }
                },
                new BookBorrowingRequest
                {
                    Id = 202,
                    RequestorId = requestorUser.Id, // Can be same or different user
                    Requestor = requestorUser,
                    ApproverId = null,
                    Approver = null, // Null approver case
                    Status = RequestStatus.Waiting,
                    DateRequested = DateTime.UtcNow.AddDays(-2),
                    Details = new List<BookBorrowingRequestDetail>
                    {
                        new BookBorrowingRequestDetail { RequestId = 202, BookId = book1.Id, Book = book1 }
                    }
                }
            };

            _mockRequestRepository.Setup(repo => repo.GetAllRequestsAsync()).ReturnsAsync(requestsFromRepo);

            // Act
            var result = await _requestService.GetAllRequestDetailsAsync(pageNum, pageSize);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(requestsFromRepo.Count, result.TotalCount); // Total before pagination
            Assert.AreEqual(pageNum, result.PageNum);
            Assert.AreEqual(pageSize, result.PageSize);
            Assert.AreEqual((int)Math.Ceiling((double)requestsFromRepo.Count / pageSize), result.TotalPage); // Should be 2 pages
            Assert.AreEqual(pageSize, result.Items.Count); // Should have 1 item on page 1

            // --- Verify Mapping for the First Request (Page 1) ---
            var firstDto = result.Items.First();
            var firstRepo = requestsFromRepo.First(r => r.Id == firstDto.Id);

            Assert.AreEqual(firstRepo.Id, firstDto.Id);
            Assert.AreEqual(firstRepo.Requestor.Username, firstDto.Requestor);
            Assert.AreEqual(firstRepo.Approver?.Username, firstDto.Approver); // Check approver
            Assert.AreEqual(firstRepo.Status.ToString(), firstDto.Status);
            Assert.AreEqual(firstRepo.DateRequested.Date, firstDto.RequestedDate.Date); // Optionally compare Date part

            // Verify Book Details Mapping
            Assert.IsNotNull(firstDto.Books);
            Assert.AreEqual(firstRepo.Details.Count, firstDto.Books.Count); // Should have 2 books

            // Verify Book 1 details
            var book1InfoDto = firstDto.Books.FirstOrDefault(b => b.Title == book1.Title);
            Assert.IsNotNull(book1InfoDto);
            Assert.AreEqual(book1.Author, book1InfoDto.Author);
            Assert.AreEqual(book1.Category.Name, book1InfoDto.CategoryName); // Has category

            // Verify Book 2 details (null category)
            var book2InfoDto = firstDto.Books.FirstOrDefault(b => b.Title == book2.Title);
            Assert.IsNotNull(book2InfoDto);
            Assert.AreEqual(book2.Author, book2InfoDto.Author);
            Assert.AreEqual(Constants.NullCategoryName, book2InfoDto.CategoryName); // Should use default for null category

            _mockRequestRepository.Verify(repo => repo.GetAllRequestsAsync(), Times.Once);
        }

        [Test]
        public async Task GetAllRequestDetailsAsync_RequestsExist_ReturnsCorrectSecondPage()
        {
            // Arrange (Using same data as previous test, just changing pageNum)
            var pageNum = 2; // Request second page
            var pageSize = 1;
            var requestorUser = new User { Id = 1, Username = "reqUser" };
            var approverUser = new User { Id = 2, Username = "appUser" };
            var category1 = new Category { Id = 1, Name = "Fiction" };
            var book1 = new Book { Id = 101, Title = "Book A", Author = "Auth A", Category = category1 };
            var book2 = new Book { Id = 102, Title = "Book B", Author = "Auth B", Category = null };

            var requestsFromRepo = new List<BookBorrowingRequest>
            {
                new BookBorrowingRequest { Id = 201, /* ... details ... */ Requestor = requestorUser, Approver = approverUser, Details = new List<BookBorrowingRequestDetail>{ new BookBorrowingRequestDetail { Book = book1 }, new BookBorrowingRequestDetail { Book = book2 } } },
                new BookBorrowingRequest
                {
                    Id = 202,
                    RequestorId = requestorUser.Id,
                    Requestor = requestorUser,
                    ApproverId = null,
                    Approver = null, // Null approver case
                    Status = RequestStatus.Waiting,
                    DateRequested = DateTime.UtcNow.AddDays(-2),
                    Details = new List<BookBorrowingRequestDetail>
                    {
                        // Need to ensure the Book object inside detail is populated for mapping
                        new BookBorrowingRequestDetail { RequestId = 202, BookId = book1.Id, Book = book1 }
                    }
                }
            };
            _mockRequestRepository.Setup(repo => repo.GetAllRequestsAsync()).ReturnsAsync(requestsFromRepo);

            // Act
            var result = await _requestService.GetAllRequestDetailsAsync(pageNum, pageSize);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(requestsFromRepo.Count, result.TotalCount);
            Assert.AreEqual(pageNum, result.PageNum);
            Assert.AreEqual(pageSize, result.PageSize);
            Assert.AreEqual(2, result.TotalPage); // Still 2 total pages
            Assert.AreEqual(1, result.Items.Count); // Should have 1 item on page 2

            // --- Verify Mapping for the Second Request (Page 2) ---
            var secondDto = result.Items.First();
            var secondRepo = requestsFromRepo.First(r => r.Id == secondDto.Id); // Should be request 202

            Assert.AreEqual(202, secondDto.Id); // Explicit check
            Assert.AreEqual(secondRepo.Requestor.Username, secondDto.Requestor);
            Assert.IsNull(secondDto.Approver); // Check null approver mapping
            Assert.AreEqual(secondRepo.Status.ToString(), secondDto.Status);

            // Verify Book Details Mapping for the second request
            Assert.IsNotNull(secondDto.Books);
            Assert.AreEqual(1, secondDto.Books.Count); // Only 1 book in this request

            var bookInfoDto = secondDto.Books.First();
            Assert.AreEqual(book1.Title, bookInfoDto.Title);
            Assert.AreEqual(book1.Author, bookInfoDto.Author);
            Assert.AreEqual(book1.Category.Name, bookInfoDto.CategoryName); // Book 1 has category

            _mockRequestRepository.Verify(repo => repo.GetAllRequestsAsync(), Times.Once);
        }
    }
} 
