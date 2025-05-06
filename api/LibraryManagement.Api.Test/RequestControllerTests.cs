using Moq;
using LibraryManagement.Api.Controllers;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.DTOs.Request;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Application.DTOs.Common;
using LibraryManagement.Application.Extensions.Exceptions;
using Microsoft.AspNetCore.Http;
using LibraryManagement.Domain.Common;

namespace LibraryManagement.Api.Test
{
    [TestFixture]
    public class RequestControllerTests
    {
        private Mock<IRequestService> _mockRequestService;
        private RequestController _requestController;

        [SetUp]
        public void Setup()
        {
            _mockRequestService = new Mock<IRequestService>();
            _requestController = new RequestController(_mockRequestService.Object);
        }

        [Test]
        public async Task GetRequestDetailById_RequestExists_ReturnsOkResult()
        {
            // Arrange
            int requestId = 1;
            var detailDto = new RequestDetailOutputDto { Id = requestId, Requestor = "user" };
            _mockRequestService.Setup(s => s.GetRequestDetailByIdAsync(requestId)).ReturnsAsync(detailDto);

            // Act
            var result = await _requestController.GetRequestDetailById(requestId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(detailDto, (result as OkObjectResult).Value);
            _mockRequestService.Verify(s => s.GetRequestDetailByIdAsync(requestId), Times.Once);
        }

        [Test]
        public async Task GetRequestDetailById_RequestNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            int requestId = 99;
            _mockRequestService.Setup(s => s.GetRequestDetailByIdAsync(requestId)).ReturnsAsync((RequestDetailOutputDto)null);

            // Act
            var result = await _requestController.GetRequestDetailById(requestId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            Assert.AreEqual($"Request with ID {requestId} not found.", (result as NotFoundObjectResult).Value);
            _mockRequestService.Verify(s => s.GetRequestDetailByIdAsync(requestId), Times.Once);
        }

        [Test]
        public async Task GetAllUserRequests_UserExists_ReturnsOkResult()
        {
            // Arrange
            int userId = 1;
            var paginatedRequests = new PaginatedOutputDto<RequestOutputDto> { Items = new List<RequestOutputDto>() };
            _mockRequestService.Setup(s => s.GetAllUserRequestsAsync(userId, 1, 5)).ReturnsAsync(paginatedRequests);

            // Act
            var result = await _requestController.GetAllUserRequests(userId, 1, 5);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(paginatedRequests, (result as OkObjectResult).Value);
            _mockRequestService.Verify(s => s.GetAllUserRequestsAsync(userId, 1, 5), Times.Once);
        }

        [Test]
        public async Task GetAllUserRequests_UserNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            int userId = 99;
            _mockRequestService.Setup(s => s.GetAllUserRequestsAsync(userId, 1, 5)).ReturnsAsync((PaginatedOutputDto<RequestOutputDto>)null); 

            // Act
            var result = await _requestController.GetAllUserRequests(userId, 1, 5);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            Assert.AreEqual($"User with ID {userId} not found.", (result as NotFoundObjectResult).Value);
            _mockRequestService.Verify(s => s.GetAllUserRequestsAsync(userId, 1, 5), Times.Once);
        }

        [Test]
        public async Task CreateRequest_Successful_ReturnsOkResult()
        {
            // Arrange
            var createDto = new CreateRequestDto { UserId = 1, BookIds = new List<int> { 1 } };
            var createdDto = new RequestDetailOutputDto { Id = 10, Requestor = "user" };
            _mockRequestService.Setup(s => s.CreateRequestAsync(createDto)).ReturnsAsync(createdDto);

            // Act
            var result = await _requestController.CreateRequest(createDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(createdDto, (result as OkObjectResult).Value);
            _mockRequestService.Verify(s => s.CreateRequestAsync(createDto), Times.Once);
        }

        [Test]
        public async Task CreateRequest_MonthlyLimitReached_ReturnsStatusCode429()
        {
            // Arrange
            var createDto = new CreateRequestDto { UserId = 1, BookIds = new List<int> { 1 } };
            _mockRequestService.Setup(s => s.CreateRequestAsync(createDto)).ReturnsAsync((RequestDetailOutputDto)null); 

            // Act
            var result = await _requestController.CreateRequest(createDto);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.AreEqual(StatusCodes.Status429TooManyRequests, objectResult.StatusCode); 
            Assert.AreEqual($"User with ID {createDto.UserId} has reached the maximum number of requests for this month!", objectResult.Value);
            _mockRequestService.Verify(s => s.CreateRequestAsync(createDto), Times.Once);
        }

        [Test]
        public void CreateRequest_BookNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var createDto = new CreateRequestDto { UserId = 1, BookIds = new List<int> { 99 } };
            _mockRequestService.Setup(s => s.CreateRequestAsync(createDto)).ThrowsAsync(new NotFoundException("Book not found."));

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(async () => await _requestController.CreateRequest(createDto));
            _mockRequestService.Verify(s => s.CreateRequestAsync(createDto), Times.Once);
        }

        [Test]
        public void CreateRequest_BookOutOfStock_ThrowsConflictException()
        {
            // Arrange
            var createDto = new CreateRequestDto { UserId = 1, BookIds = new List<int> { 1 } };
            _mockRequestService.Setup(s => s.CreateRequestAsync(createDto)).ThrowsAsync(new ConflictException("Book out of stock."));

            // Act & Assert
            Assert.ThrowsAsync<ConflictException>(async () => await _requestController.CreateRequest(createDto));
            _mockRequestService.Verify(s => s.CreateRequestAsync(createDto), Times.Once);
        }

        [Test]
        public async Task UpdateRequest_Successful_ReturnsOkResult()
        {
            // Arrange
            int requestId = 1;
            var updateDto = new UpdateRequestDto { AdminId = 2, Status = RequestStatus.Approved };
            var updatedDto = new RequestDetailOutputDto { Id = requestId, Status = RequestStatus.Approved.ToString() };
            _mockRequestService.Setup(s => s.UpdateRequestAsync(requestId, updateDto)).ReturnsAsync(updatedDto);

            // Act
            var result = await _requestController.UpdateRequest(requestId, updateDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(updatedDto, (result as OkObjectResult).Value);
            _mockRequestService.Verify(s => s.UpdateRequestAsync(requestId, updateDto), Times.Once);
        }

        [Test]
        public async Task UpdateRequest_StatusIsWaiting_ReturnsBadRequest()
        {
            // Arrange
            int requestId = 1;
            var updateDto = new UpdateRequestDto { AdminId = 2, Status = RequestStatus.Waiting }; 

            // Act
            var result = await _requestController.UpdateRequest(requestId, updateDto);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.AreEqual(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.AreEqual("You are not suppose to update any request to Waiting state.", objectResult.Value);
            _mockRequestService.Verify(s => s.UpdateRequestAsync(It.IsAny<int>(), It.IsAny<UpdateRequestDto>()), Times.Never); 
        }

        [Test]
        public async Task UpdateRequest_UpdaterNotAdmin_ReturnsUnauthorizedStatus() 
        {
            // Arrange
            int requestId = 1;
            var updateDto = new UpdateRequestDto { AdminId = 5, Status = RequestStatus.Approved }; 
            _mockRequestService.Setup(s => s.UpdateRequestAsync(requestId, updateDto)).ReturnsAsync((RequestDetailOutputDto)null); 

            // Act
            var result = await _requestController.UpdateRequest(requestId, updateDto);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.AreEqual(StatusCodes.Status401Unauthorized, objectResult.StatusCode); 
            Assert.AreEqual($"{updateDto.AdminId} doesn't have permission to update this request!", objectResult.Value);
            _mockRequestService.Verify(s => s.UpdateRequestAsync(requestId, updateDto), Times.Once);
        }

        [Test]
        public void UpdateRequest_RequestNotFound_ThrowsNotFoundException()
        {
            // Arrange
            int requestId = 99;
            var updateDto = new UpdateRequestDto { AdminId = 1, Status = RequestStatus.Approved };
            _mockRequestService.Setup(s => s.UpdateRequestAsync(requestId, updateDto))
                .ThrowsAsync(new NotFoundException("Request not found."));

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(async () => await _requestController.UpdateRequest(requestId, updateDto));
            _mockRequestService.Verify(s => s.UpdateRequestAsync(requestId, updateDto), Times.Once);
        }

        [Test]
        public void UpdateRequest_RequestAlreadyProcessed_ThrowsConflictException()
        {
            // Arrange
            int requestId = 1;
            var updateDto = new UpdateRequestDto { AdminId = 1, Status = RequestStatus.Approved };
            _mockRequestService.Setup(s => s.UpdateRequestAsync(requestId, updateDto))
                .ThrowsAsync(new ConflictException("Request already processed."));

            // Act & Assert
            Assert.ThrowsAsync<ConflictException>(async () => await _requestController.UpdateRequest(requestId, updateDto));
            _mockRequestService.Verify(s => s.UpdateRequestAsync(requestId, updateDto), Times.Once);
        }

        [Test]
        public async Task GetAllRequests_RequestsFound_ReturnsOkResultWithPaginatedData()
        {
            // Arrange
            string status = RequestStatus.Waiting.ToString(); 
            int pageNum = 1;
            int pageSize = 10;
            var requestDetails = new List<RequestDetailOutputDto> 
            {
                new RequestDetailOutputDto { Id = 1, Requestor = "User One", Status = status, RequestedDate = DateTime.UtcNow, Books = new List<BookInformation>() },
                new RequestDetailOutputDto { Id = 2, Requestor = "User Two", Status = status, RequestedDate = DateTime.UtcNow.AddDays(-1), Books = new List<BookInformation>() }
            };
            var paginatedResult = new PaginatedOutputDto<RequestDetailOutputDto> 
            {
                Items = requestDetails,
                PageNum = pageNum,
                PageSize = pageSize,
                TotalCount = 50, 
                TotalPage = 5
            };

            _mockRequestService.Setup(s => s.GetAllRequestDetailsAsync(status, pageNum, pageSize)) 
                .ReturnsAsync(paginatedResult);

            // Act
            var result = await _requestController.GetAllRequests(status, pageNum, pageSize); 

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(paginatedResult, okResult.Value);
            _mockRequestService.Verify(s => s.GetAllRequestDetailsAsync(status, pageNum, pageSize), Times.Once);
        }

        [Test]
        public async Task GetAllRequests_RequestsFoundWithDefaultParams_ReturnsOkResultWithPaginatedData()
        {
            // Arrange
            string? status = null; 
            int pageNum = Constants.DefaultPageNum; 
            int pageSize = Constants.DefaultPageSize;
            var requestDetails = new List<RequestDetailOutputDto>
            {
                new RequestDetailOutputDto { Id = 3, Requestor = "User Three", Status = RequestStatus.Approved.ToString(), RequestedDate = DateTime.UtcNow, Books = new List<BookInformation>() }
            };
            var paginatedResult = new PaginatedOutputDto<RequestDetailOutputDto>
            {
                Items = requestDetails,
                PageNum = pageNum,
                PageSize = pageSize,
                TotalCount = 1,
                TotalPage = 1
            };

            _mockRequestService.Setup(s => s.GetAllRequestDetailsAsync(status, pageNum, pageSize))
                .ReturnsAsync(paginatedResult);

            // Act
            var result = await _requestController.GetAllRequests(status); 

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(paginatedResult, okResult.Value);
            _mockRequestService.Verify(s => s.GetAllRequestDetailsAsync(status, pageNum, pageSize), Times.Once);
        }

        [Test]
        public async Task GetAllRequests_NoRequestsFound_ReturnsNotFoundResult()
        {
            // Arrange
            string? status = RequestStatus.Rejected.ToString();
            int pageNum = 1;
            int pageSize = 5;

            _mockRequestService.Setup(s => s.GetAllRequestDetailsAsync(status, pageNum, pageSize))
                .ReturnsAsync((PaginatedOutputDto<RequestDetailOutputDto>)null); 

            // Act
            var result = await _requestController.GetAllRequests(status, pageNum, pageSize); 

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult.Value);
            Assert.AreEqual("Failed to load request details.", notFoundResult.Value); 
            _mockRequestService.Verify(s => s.GetAllRequestDetailsAsync(status, pageNum, pageSize), Times.Once);
        }

        [Test]
        public async Task GetAllRequests_WithZeroResults_ReturnsNotFound()
        {
            // Arrange
            string status = "Pending";
            int pageNum = 1;
            int pageSize = 10;

            var emptyResult = new PaginatedOutputDto<RequestDetailOutputDto>
            {
                Items = new List<RequestDetailOutputDto>(),
                TotalCount = 0,
                PageNum = pageNum,
                PageSize = pageSize,
                TotalPage = 0,
            };

            _mockRequestService.Setup(service =>
                service.GetAllRequestDetailsAsync(status, pageNum, pageSize))
                .ReturnsAsync(emptyResult);

            // Act
            var result = await _requestController.GetAllRequests(status, pageNum, pageSize);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual($"No requests found with status: {status}", notFoundResult.Value);
            _mockRequestService.Verify(s => s.GetAllRequestDetailsAsync(status, pageNum, pageSize), Times.Once);
        }

        [Test]
        public async Task ReturnBooks_WithMismatchingIds_ReturnsBadRequest()
        {
            // Arrange
            int requestId = 1;
            var returnBookRequestDto = new ReturnBookRequestDto
            {
                RequestId = 2 
            };

            // Act
            var result = await _requestController.ReturnBooks(requestId, returnBookRequestDto);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual("Request ID in the URL does not match the request ID in the body.", badRequestResult.Value);
            _mockRequestService.Verify(s => s.ReturnBooksAsync(It.IsAny<ReturnBookRequestDto>()), Times.Never);
        }

        [Test]
        public async Task ReturnBooks_WhenServiceReturnsNull_ReturnsNotFound()
        {
            // Arrange
            int requestId = 1;
            var returnBookRequestDto = new ReturnBookRequestDto
            {
                RequestId = 1,
                ProcessedById = 2
            };

            _mockRequestService.Setup(service =>
                service.ReturnBooksAsync(returnBookRequestDto))
                .ReturnsAsync((RequestDetailOutputDto)null);

            // Act
            var result = await _requestController.ReturnBooks(requestId, returnBookRequestDto);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual($"Failed to process return for request with ID {requestId}.", notFoundResult.Value);
            _mockRequestService.Verify(s => s.ReturnBooksAsync(returnBookRequestDto), Times.Once);
        }

        [Test]
        public async Task ReturnBooks_WithValidRequest_ReturnsOkResult()
        {
            // Arrange
            int requestId = 1;
            var returnBookRequestDto = new ReturnBookRequestDto
            {
                RequestId = 1,
                ProcessedById = 2
            };

            var returnedRequest = new RequestDetailOutputDto
            {
                Id = 1,
                Status = "Returned",
                Requestor = "Test User",
                DateRequested = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"),
                DateReturned = DateTime.Now.ToString("yyyy-MM-dd"),
                Books = new List<BookInformation>()
            };

            _mockRequestService.Setup(service =>
                service.ReturnBooksAsync(returnBookRequestDto))
                .ReturnsAsync(returnedRequest);

            // Act
            var result = await _requestController.ReturnBooks(requestId, returnBookRequestDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(returnedRequest, okResult.Value);
            _mockRequestService.Verify(s => s.ReturnBooksAsync(returnBookRequestDto), Times.Once);
        }

        [Test]
        public async Task GetNumberOfAvailableRequests_UserExists_ReturnsOkResultWithAvailableRequests()
        {
            // Arrange
            int userId = 1;
            var availableRequestDto = new AvailableRequestOutputDto { AvailableRequests = 3 };
            _mockRequestService.Setup(s => s.GetAvailableRequestsAsync(userId)).ReturnsAsync(availableRequestDto);

            // Act
            var result = await _requestController.GetNumberOfAvailableRequests(userId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(availableRequestDto, okResult.Value);
            _mockRequestService.Verify(s => s.GetAvailableRequestsAsync(userId), Times.Once);
        }

        [Test]
        public void GetNumberOfAvailableRequests_UserNotFound_ThrowsNotFoundException()
        {
            // Arrange
            int userId = 99;
            _mockRequestService.Setup(s => s.GetAvailableRequestsAsync(userId))
                .ThrowsAsync(new NotFoundException($"User with ID {userId} not found."));

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(async () => await _requestController.GetNumberOfAvailableRequests(userId));
            _mockRequestService.Verify(s => s.GetAvailableRequestsAsync(userId), Times.Once);
        }
    }
}