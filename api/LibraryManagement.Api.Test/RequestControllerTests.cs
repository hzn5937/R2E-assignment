using Moq;
using LibraryManagement.Api.Controllers;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.DTOs.Request;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Application.DTOs.Common;
using LibraryManagement.Application.Extensions.Exceptions;
using Microsoft.AspNetCore.Http; 

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
        public async Task GetAvailableRequests_UserExists_ReturnsOkResult()
        {
            // Arrange
            int userId = 1;
            var availableDto = new AvailableRequestOutputDto { AvailableRequests = 2 };
            _mockRequestService.Setup(s => s.GetAvailableRequestsAsync(userId)).ReturnsAsync(availableDto);

            // Act
            var result = await _requestController.GetAvailableRequests(userId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(availableDto, okResult.Value);
            _mockRequestService.Verify(s => s.GetAvailableRequestsAsync(userId), Times.Once);
        }

        [Test]
        public void GetAvailableRequests_UserNotFound_ThrowsNotFoundException()
        {
            // Arrange
            int userId = 99;
            _mockRequestService.Setup(s => s.GetAvailableRequestsAsync(userId))
                              .ThrowsAsync(new NotFoundException($"User with ID {userId} not found."));

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(async () => await _requestController.GetAvailableRequests(userId));
            _mockRequestService.Verify(s => s.GetAvailableRequestsAsync(userId), Times.Once);
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
        public async Task GetAllRequests_ReturnsOkResult()
        {
            // Arrange
            var paginatedRequests = new PaginatedOutputDto<RequestDetailOutputDto> { Items = new List<RequestDetailOutputDto>() };
            _mockRequestService.Setup(s => s.GetAllRequestDetailsAsync(1, 5)).ReturnsAsync(paginatedRequests);

            // Act
            var result = await _requestController.GetAllRequests(1, 5);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(paginatedRequests, (result as OkObjectResult).Value);
            _mockRequestService.Verify(s => s.GetAllRequestDetailsAsync(1, 5), Times.Once);
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
            _mockRequestService.Setup(s => s.CreateRequestAsync(createDto)).ReturnsAsync((RequestDetailOutputDto)null); // Service returns null if limit reached

            // Act
            var result = await _requestController.CreateRequest(createDto);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.AreEqual(StatusCodes.Status429TooManyRequests, objectResult.StatusCode); // Check for 429
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
    }
}