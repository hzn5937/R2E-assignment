using Moq;
using NUnit.Framework;
using LibraryManagement.Api.Controllers;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.DTOs.UserManagement;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using LibraryManagement.Domain.Common;
using LibraryManagement.Application.DTOs.Common;
using System.Collections.Generic;
using LibraryManagement.Application.Extensions.Exceptions;
using LibraryManagement.Domain.Enums; // Required for ConflictException and NotFoundException

namespace LibraryManagement.Api.Test
{
    [TestFixture]
    public class UserManagementControllerTests
    {
        private Mock<IUserManagementService> _mockUserManagementService;
        private UserManagementController _userManagementController;

        [SetUp]
        public void Setup()
        {
            _mockUserManagementService = new Mock<IUserManagementService>();
            _userManagementController = new UserManagementController(_mockUserManagementService.Object);
        }

        [Test]
        public async Task GetAllUsers_ReturnsOkResult_WithPaginatedUsers()
        {
            // Arrange
            var pageNum = 1;
            var pageSize = 10;
            var cancellationToken = CancellationToken.None;
            var usersList = new List<UserOutputDto>
            {
                new UserOutputDto { Id = 1, Username = "user1", Email = "user1@example.com", Role = UserRole.User },
                new UserOutputDto { Id = 2, Username = "user2", Email = "user2@example.com", Role = UserRole.Admin }
            };
            var paginatedUsers = new PaginatedOutputDto<UserOutputDto>
            {
                Items = usersList,
                TotalCount = usersList.Count,
                PageNum = pageNum,
                PageSize = pageSize,
                TotalPage = 1
            };
            _mockUserManagementService.Setup(s => s.GetAllUsersAsync(pageNum, pageSize, cancellationToken)).ReturnsAsync(paginatedUsers);

            // Act
            var result = await _userManagementController.GetAllUsers(pageNum, pageSize, cancellationToken);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(paginatedUsers, okResult.Value);
            _mockUserManagementService.Verify(s => s.GetAllUsersAsync(pageNum, pageSize, cancellationToken), Times.Once);
        }

        [Test]
        public async Task GetAllUsers_WithDefaultParameters_ReturnsOkResult_WithPaginatedUsers()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var usersList = new List<UserOutputDto>
            {
                new UserOutputDto { Id = 1, Username = "user1", Email = "user1@example.com", Role = UserRole.User }
            };
            var paginatedUsers = new PaginatedOutputDto<UserOutputDto>
            {
                Items = usersList,
                TotalCount = usersList.Count,
                PageNum = Constants.DefaultPageNum,
                PageSize = Constants.DefaultPageSize,
                TotalPage = 1
            };
            _mockUserManagementService.Setup(s => s.GetAllUsersAsync(Constants.DefaultPageNum, Constants.DefaultPageSize, cancellationToken)).ReturnsAsync(paginatedUsers);

            // Act
            var result = await _userManagementController.GetAllUsers(); // Using default pageNum and pageSize

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(paginatedUsers, okResult.Value);
            _mockUserManagementService.Verify(s => s.GetAllUsersAsync(Constants.DefaultPageNum, Constants.DefaultPageSize, cancellationToken), Times.Once);
        }


        [Test]
        public async Task GetUserById_WhenUserExists_ReturnsOkResult_WithUser()
        {
            // Arrange
            var userId = 1;
            var cancellationToken = CancellationToken.None;
            var userDto = new UserOutputDto { Id = userId, Username = "testuser", Email = "test@example.com", Role = UserRole.User };
            _mockUserManagementService.Setup(s => s.GetUserByIdAsync(userId, cancellationToken)).ReturnsAsync(userDto);

            // Act
            var result = await _userManagementController.GetUserById(userId, cancellationToken);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(userDto, okResult.Value);
            _mockUserManagementService.Verify(s => s.GetUserByIdAsync(userId, cancellationToken), Times.Once);
        }

        [Test]
        public async Task GetUserById_WhenUserDoesNotExist_ReturnsOkResult_WithNull()
        {
            // Arrange
            var userId = 99;
            var cancellationToken = CancellationToken.None;
            _mockUserManagementService.Setup(s => s.GetUserByIdAsync(userId, cancellationToken)).ReturnsAsync((UserOutputDto)null);

            // Act
            var result = await _userManagementController.GetUserById(userId, cancellationToken);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNull(okResult.Value); // Service returns null, controller wraps it in Ok. Consider if NotFound is more appropriate.
            _mockUserManagementService.Verify(s => s.GetUserByIdAsync(userId, cancellationToken), Times.Once);
        }

        [Test]
        public async Task CreateUser_WhenSuccessful_ReturnsCreatedAtActionResult_WithUser()
        {
            // Arrange
            var createUserDto = new CreateUserDto { Username = "newuser", Password = "password", Email = "new@example.com", Role = Domain.Enums.UserRole.User };
            var createdUserDto = new UserOutputDto { Id = 1, Username = createUserDto.Username, Email = createUserDto.Email, Role = createUserDto.Role };
            var cancellationToken = CancellationToken.None;
            _mockUserManagementService.Setup(s => s.CreateUserAsync(createUserDto, cancellationToken)).ReturnsAsync(createdUserDto);

            // Act
            var result = await _userManagementController.CreateUser(createUserDto, cancellationToken);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result);
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.AreEqual(nameof(_userManagementController.GetUserById), createdAtActionResult.ActionName);
            Assert.AreEqual(createdUserDto.Id, createdAtActionResult.RouteValues["id"]);
            Assert.AreEqual(createdUserDto, createdAtActionResult.Value);
            _mockUserManagementService.Verify(s => s.CreateUserAsync(createUserDto, cancellationToken), Times.Once);
        }

        [Test]
        public void CreateUser_WhenUsernameExists_ThrowsConflictException()
        {
            // Arrange
            var createUserDto = new CreateUserDto { Username = "existinguser", Password = "password", Email = "new@example.com", Role = Domain.Enums.UserRole.User };
            var cancellationToken = CancellationToken.None;
            _mockUserManagementService.Setup(s => s.CreateUserAsync(createUserDto, cancellationToken))
                                     .ThrowsAsync(new ConflictException("Username already exists."));

            // Act & Assert
            Assert.ThrowsAsync<ConflictException>(async () => await _userManagementController.CreateUser(createUserDto, cancellationToken));
            _mockUserManagementService.Verify(s => s.CreateUserAsync(createUserDto, cancellationToken), Times.Once);
        }

        [Test]
        public async Task UpdateUser_WhenSuccessful_ReturnsOkResult_WithUpdatedUser()
        {
            // Arrange
            var userId = 1;
            var updateUserDto = new UpdateUserDto { Email = "updated@example.com", Role = Domain.Enums.UserRole.Admin };
            var updatedUserDto = new UserOutputDto { Id = userId, Username = "testuser", Email = updateUserDto.Email, Role = updateUserDto.Role };
            var cancellationToken = CancellationToken.None;
            _mockUserManagementService.Setup(s => s.UpdateUserAsync(userId, updateUserDto, cancellationToken)).ReturnsAsync(updatedUserDto);

            // Act
            var result = await _userManagementController.UpdateUser(userId, updateUserDto, cancellationToken);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(updatedUserDto, okResult.Value);
            _mockUserManagementService.Verify(s => s.UpdateUserAsync(userId, updateUserDto, cancellationToken), Times.Once);
        }

        [Test]
        public void UpdateUser_WhenUserNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var userId = 99;
            var updateUserDto = new UpdateUserDto { Email = "updated@example.com", Role = Domain.Enums.UserRole.Admin };
            var cancellationToken = CancellationToken.None;
            _mockUserManagementService.Setup(s => s.UpdateUserAsync(userId, updateUserDto, cancellationToken))
                                     .ThrowsAsync(new NotFoundException($"User with ID {userId} not found."));

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(async () => await _userManagementController.UpdateUser(userId, updateUserDto, cancellationToken));
            _mockUserManagementService.Verify(s => s.UpdateUserAsync(userId, updateUserDto, cancellationToken), Times.Once);
        }


        [Test]
        public async Task DeleteUser_WhenSuccessful_ReturnsOkResult_WithTrue()
        {
            // Arrange
            var userId = 1;
            var cancellationToken = CancellationToken.None;
            _mockUserManagementService.Setup(s => s.DeleteUserAsync(userId, cancellationToken)).ReturnsAsync(true);

            // Act
            var result = await _userManagementController.DeleteUser(userId, cancellationToken);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(true, okResult.Value);
            _mockUserManagementService.Verify(s => s.DeleteUserAsync(userId, cancellationToken), Times.Once);
        }

        [Test]
        public async Task DeleteUser_WhenUserNotFound_ReturnsOkResult_WithFalse()
        {
            // Arrange
            var userId = 99;
            var cancellationToken = CancellationToken.None;
            _mockUserManagementService.Setup(s => s.DeleteUserAsync(userId, cancellationToken)).ReturnsAsync(false);

            // Act
            var result = await _userManagementController.DeleteUser(userId, cancellationToken);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(false, okResult.Value); // Service returns false, controller wraps it in Ok. Consider if NotFound is more appropriate.
            _mockUserManagementService.Verify(s => s.DeleteUserAsync(userId, cancellationToken), Times.Once);
        }
    }
}