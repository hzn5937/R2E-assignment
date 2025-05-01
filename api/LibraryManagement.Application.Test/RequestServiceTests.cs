using Moq;
using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.DTOs.Request;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Application.Extensions.Exceptions;
using LibraryManagement.Domain.Common;

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
    }
}