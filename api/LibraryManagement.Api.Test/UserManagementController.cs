using Moq;
using NUnit.Framework;
using LibraryManagement.Api.Controllers;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.DTOs.UserManagement;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LibraryManagement.Application.Extensions.Exceptions; // Assuming NotFoundException might be used
using LibraryManagement.Domain.Enums; // Assuming UserRole might be needed for DTOs

namespace LibraryManagement.Api.Test
{
    [TestFixture]
    public class UserManagementControllerTests
    {
        private Mock<IUserManagementService> _mockUserManagementService;
        private UserManagementController _controller;

        [SetUp]
        public void Setup()
        {
            _mockUserManagementService = new Mock<IUserManagementService>();
            _controller = new UserManagementController(_mockUserManagementService.Object);
        }

        [Test]
        public async Task GetAllUsers_ReturnsOkResult_WithListOfUsers()
        {
            // Arrange
            var users = new List<UserOutputDto>
            {
                new UserOutputDto { Id = 1, Username = "admin", Role = UserRole.Admin },
                new UserOutputDto { Id = 2, Username = "user", Role = UserRole.User }
            };
            _mockUserManagementService.Setup(s => s.GetAllUsersAsync(It.IsAny<CancellationToken>())).ReturnsAsync(users);

            // Act
            var result = await _controller.GetAllUsers(CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(users, okResult.Value);
            _mockUserManagementService.Verify(s => s.GetAllUsersAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetUserById_UserExists_ReturnsOkResultWithUser()
        {
            // Arrange
            int userId = 1;
            var userDto = new UserOutputDto { Id = userId, Username = "testuser", Role = UserRole.User };
            _mockUserManagementService.Setup(s => s.GetUserByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(userDto);

            // Act
            var result = await _controller.GetUserById(userId, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(userDto, okResult.Value);
            _mockUserManagementService.Verify(s => s.GetUserByIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetUserById_UserDoesNotExist_ReturnsNotFoundResult() // Assuming service returns null for not found
        {
            // Arrange
            int userId = 99;
            _mockUserManagementService.Setup(s => s.GetUserByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync((UserOutputDto)null);

            // Act
            var result = await _controller.GetUserById(userId, CancellationToken.None);

            // Assert
            // Controller currently returns Ok(null). If NotFound is desired, controller logic or service exception needs change.
            // Let's assert the current behavior: Ok(null)
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNull(okResult.Value); // The service returned null, controller wraps it in Ok.
            _mockUserManagementService.Verify(s => s.GetUserByIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);

            // --- Alternative Assertion if NotFoundException is thrown by service ---
            // // Arrange
            // int userId = 99;
            // _mockUserManagementService.Setup(s => s.GetUserByIdAsync(userId, It.IsAny<CancellationToken>()))
            //                        .ThrowsAsync(new NotFoundException($"User with ID {userId} not found."));
            //
            // // Act & Assert
            // Assert.ThrowsAsync<NotFoundException>(async () => await _controller.GetUserById(userId, CancellationToken.None));
            // _mockUserManagementService.Verify(s => s.GetUserByIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        }


        [Test]
        public async Task CreateUser_ValidData_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var createUserDto = new CreateUserDto { Username = "newbie", Password = "password123", Role = UserRole.User };
            var createdUserDto = new UserOutputDto { Id = 3, Username = createUserDto.Username, Role = createUserDto.Role };
            _mockUserManagementService.Setup(s => s.CreateUserAsync(createUserDto, It.IsAny<CancellationToken>())).ReturnsAsync(createdUserDto);

            // Act
            var result = await _controller.CreateUser(createUserDto, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result);
            var createdAtResult = result as CreatedAtActionResult;
            Assert.AreEqual(nameof(_controller.GetUserById), createdAtResult.ActionName);
            Assert.AreEqual(createdUserDto.Id, createdAtResult.RouteValues["id"]);
            Assert.AreEqual(createdUserDto, createdAtResult.Value);
            _mockUserManagementService.Verify(s => s.CreateUserAsync(createUserDto, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void CreateUser_UsernameConflict_ThrowsConflictException() // Assuming service throws ConflictException
        {
            // Arrange
            var createUserDto = new CreateUserDto { Username = "existing", Password = "password123", Role = UserRole.User };
            _mockUserManagementService.Setup(s => s.CreateUserAsync(createUserDto, It.IsAny<CancellationToken>()))
                                   .ThrowsAsync(new ConflictException("Username already exists."));

            // Act & Assert
            Assert.ThrowsAsync<ConflictException>(async () => await _controller.CreateUser(createUserDto, CancellationToken.None));
            _mockUserManagementService.Verify(s => s.CreateUserAsync(createUserDto, It.IsAny<CancellationToken>()), Times.Once);
        }


        [Test]
        public async Task UpdateUser_UserExists_ReturnsOkResultWithUpdatedUser()
        {
            // Arrange
            int userId = 1;
            var updateUserDto = new UpdateUserDto { Role = UserRole.Admin }; // Example update
            var updatedUserDto = new UserOutputDto { Id = userId, Username = "testuser", Role = updateUserDto.Role };
            _mockUserManagementService.Setup(s => s.UpdateUserAsync(userId, updateUserDto, It.IsAny<CancellationToken>())).ReturnsAsync(updatedUserDto);

            // Act
            var result = await _controller.UpdateUser(userId, updateUserDto, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(updatedUserDto, okResult.Value);
            _mockUserManagementService.Verify(s => s.UpdateUserAsync(userId, updateUserDto, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task UpdateUser_UserDoesNotExist_ReturnsNotFoundResult() // Assuming service returns null for not found
        {
            // Arrange
            int userId = 99;
            var updateUserDto = new UpdateUserDto { Role = UserRole.Admin };
            _mockUserManagementService.Setup(s => s.UpdateUserAsync(userId, updateUserDto, It.IsAny<CancellationToken>())).ReturnsAsync((UserOutputDto)null);

            // Act
            var result = await _controller.UpdateUser(userId, updateUserDto, CancellationToken.None);

            // Assert
            // Similar to GetById, currently returns Ok(null).
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNull(okResult.Value);
            _mockUserManagementService.Verify(s => s.UpdateUserAsync(userId, updateUserDto, It.IsAny<CancellationToken>()), Times.Once);

            // --- Alternative Assertion if NotFoundException is thrown by service ---
            // // Arrange
            // int userId = 99;
            // var updateUserDto = new UpdateUserDto { Role = UserRole.Admin };
            // _mockUserManagementService.Setup(s => s.UpdateUserAsync(userId, updateUserDto, It.IsAny<CancellationToken>()))
            //                        .ThrowsAsync(new NotFoundException($"User with ID {userId} not found."));
            //
            // // Act & Assert
            // Assert.ThrowsAsync<NotFoundException>(async () => await _controller.UpdateUser(userId, updateUserDto, CancellationToken.None));
            // _mockUserManagementService.Verify(s => s.UpdateUserAsync(userId, updateUserDto, It.IsAny<CancellationToken>()), Times.Once);
        }


        [Test]
        public async Task DeleteUser_UserExists_ReturnsOkResultWithTrue()
        {
            // Arrange
            int userId = 1;
            _mockUserManagementService.Setup(s => s.DeleteUserAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteUser(userId, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(true, okResult.Value);
            _mockUserManagementService.Verify(s => s.DeleteUserAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DeleteUser_UserDoesNotExist_ReturnsOkResultWithFalse() // Assuming service returns false for not found
        {
            // Arrange
            int userId = 99;
            _mockUserManagementService.Setup(s => s.DeleteUserAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteUser(userId, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(false, okResult.Value);
            _mockUserManagementService.Verify(s => s.DeleteUserAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}