using LibraryManagement.Api.Controllers;
using LibraryManagement.Application.DTOs.User;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryManagement.Api.Tests
{
    [TestFixture]
    public class UserControllerTests
    {
        private Mock<IUserService> _mockUserService;
        private UserController _controller;

        [SetUp]
        public void Setup()
        {
            // Arrange: Create a mock instance of IUserService
            _mockUserService = new Mock<IUserService>();

            // Arrange: Create an instance of UserController, injecting the mock service
            _controller = new UserController(_mockUserService.Object);
        }

        [Test]
        public async Task GetUserCount_ReturnsOkResult_WithUserCountData()
        {
            // Arrange: Define the expected output from the service
            var expectedUserCount = new UserCountOutputDto
            {
                TotalUsers = 10,
                TotalAdmin = 2
            };

            // Arrange: Set up the mock service's GetUserCountAsync method
            // to return the expected output when called
            _mockUserService.Setup(service => service.GetUserCountAsync())
                            .ReturnsAsync(expectedUserCount);

            // Act: Call the GetUserCount method on the controller
            var result = await _controller.GetUserCount();

            // Assert: Check if the result is an OkObjectResult
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;

            // Assert: Check if the status code is 200 (OK)
            Assert.AreEqual(200, okResult.StatusCode);

            // Assert: Check if the value returned in the result is the expected UserCountOutputDto
            Assert.IsInstanceOf<UserCountOutputDto>(okResult.Value);
            var actualUserCount = okResult.Value as UserCountOutputDto;

            // Assert: Check if the values within the returned DTO match the expected values
            Assert.AreEqual(expectedUserCount.TotalUsers, actualUserCount.TotalUsers);
            Assert.AreEqual(expectedUserCount.TotalAdmin, actualUserCount.TotalAdmin);

            // Assert: Verify that the GetUserCountAsync method on the mock service was called exactly once
            _mockUserService.Verify(service => service.GetUserCountAsync(), Times.Once);
        }

        [Test]
        public async Task GetUserCount_ServiceReturnsNull_ReturnsOkResultWithNull()
        {
            // Arrange: Set up the mock service to return null
            _mockUserService.Setup(service => service.GetUserCountAsync())
                            .ReturnsAsync((UserCountOutputDto)null); // Explicitly cast null

            // Act: Call the GetUserCount method
            var result = await _controller.GetUserCount();

            // Assert: Check if the result is an OkObjectResult
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;

            // Assert: Check if the status code is 200 (OK)
            Assert.AreEqual(200, okResult.StatusCode);

            // Assert: Check if the value returned is null
            Assert.IsNull(okResult.Value);

            // Assert: Verify service method was called
            _mockUserService.Verify(service => service.GetUserCountAsync(), Times.Once);
        }
    }
}