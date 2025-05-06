using Moq;
using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.DTOs.UserManagement;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Application.Extensions.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagement.Application.Test
{
    [TestFixture]
    public class UserManagementServiceTests
    {
        private Mock<IUserRepository> _mockUserRepository;
        private UserManagementService _userManagementService;

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userManagementService = new UserManagementService(_mockUserRepository.Object);
        }

        [Test]
        public async Task GetAllUsersAsync_WhenUsersExist_ReturnsPaginatedUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Username = "user1", Email = "user1@example.com", Role = UserRole.User },
                new User { Id = 2, Username = "user2", Email = "user2@example.com", Role = UserRole.Admin }
            };
            _mockUserRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);

            // Act
            var result = await _userManagementService.GetAllUsersAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(users.Count, result.TotalCount);
            Assert.AreEqual(users.Count, result.Items.Count());
            Assert.AreEqual(users[0].Username, result.Items.First().Username);
            _mockUserRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetAllUsersAsync_WhenNoUsersExist_ReturnsEmptyPaginatedResult()
        {
            // Arrange
            var users = new List<User>();
            _mockUserRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);

            // Act
            var result = await _userManagementService.GetAllUsersAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.TotalCount);
            Assert.IsEmpty(result.Items);
            _mockUserRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetAllUsersAsync_WithPagination_ReturnsCorrectPage()
        {
            // Arrange
            var users = Enumerable.Range(1, 15)
                                  .Select(i => new User { Id = i, Username = $"user{i}", Email = $"user{i}@example.com", Role = UserRole.User })
                                  .ToList();
            _mockUserRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);
            var pageNum = 2;
            var pageSize = 5;

            // Act
            var result = await _userManagementService.GetAllUsersAsync(pageNum, pageSize);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(users.Count, result.TotalCount);
            Assert.AreEqual(pageSize, result.Items.Count());
            Assert.AreEqual($"user{pageSize + 1}", result.Items.First().Username);
            Assert.AreEqual(pageNum, result.PageNum);
            Assert.AreEqual(pageSize, result.PageSize);
        }

        [Test]
        public async Task GetUserByIdAsync_WhenUserExists_ReturnsUserOutputDto()
        {
            // Arrange
            var userId = 1;
            var user = new User { Id = userId, Username = "testuser", Email = "test@example.com", Role = UserRole.User };
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _userManagementService.GetUserByIdAsync(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(user.Id, result.Id);
            Assert.AreEqual(user.Username, result.Username);
            Assert.AreEqual(user.Email, result.Email);
            Assert.AreEqual(user.Role, result.Role);
            _mockUserRepository.Verify(repo => repo.GetUserByIdAsync(userId), Times.Once);
        }

        [Test]
        public void GetUserByIdAsync_WhenUserDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var userId = 1;
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(async () => await _userManagementService.GetUserByIdAsync(userId));
            Assert.AreEqual($"User with ID {userId} not found", ex.Message);
            _mockUserRepository.Verify(repo => repo.GetUserByIdAsync(userId), Times.Once);
        }

        [Test]
        public async Task CreateUserAsync_WithValidData_CreatesAndReturnsUser()
        {
            // Arrange
            var createUserDto = new CreateUserDto
            {
                Username = "newuser",
                Email = "newuser@example.com",
                Password = "password123",
                Role = UserRole.User
            };
            _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(createUserDto.Username, It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync((User)null);
            _mockUserRepository.Setup(repo => repo.GetByEmailAsync(createUserDto.Email, It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync((User)null);
            _mockUserRepository.Setup(repo => repo.AddAsync(It.IsAny<User>(), It.IsAny<System.Threading.CancellationToken>()))
                .Callback<User, System.Threading.CancellationToken>((u, ct) => u.Id = 1) // Simulate ID generation
                .Returns(Task.CompletedTask);

            // Act
            var result = await _userManagementService.CreateUserAsync(createUserDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual(createUserDto.Username, result.Username);
            Assert.AreEqual(createUserDto.Email, result.Email);
            Assert.AreEqual(createUserDto.Role, result.Role);
            _mockUserRepository.Verify(repo => repo.GetByUsernameAsync(createUserDto.Username, It.IsAny<System.Threading.CancellationToken>()), Times.Once);
            _mockUserRepository.Verify(repo => repo.GetByEmailAsync(createUserDto.Email, It.IsAny<System.Threading.CancellationToken>()), Times.Once);
            _mockUserRepository.Verify(repo => repo.AddAsync(It.Is<User>(u =>
                u.Username == createUserDto.Username &&
                u.Email == createUserDto.Email &&
                u.Role == createUserDto.Role &&
                !string.IsNullOrEmpty(u.PasswordHash) 
            ), It.IsAny<System.Threading.CancellationToken>()), Times.Once);
        }

        [Test]
        public void CreateUserAsync_WhenUsernameIsTaken_ThrowsConflictException()
        {
            // Arrange
            var createUserDto = new CreateUserDto { Username = "existinguser", Email = "new@example.com", Password = "password", Role = UserRole.User };
            var existingUser = new User { Id = 1, Username = "existinguser", Email = "old@example.com" };
            _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(createUserDto.Username, It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(existingUser);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ConflictException>(async () => await _userManagementService.CreateUserAsync(createUserDto));
            Assert.AreEqual("Username is already taken", ex.Message);
            _mockUserRepository.Verify(repo => repo.GetByUsernameAsync(createUserDto.Username, It.IsAny<System.Threading.CancellationToken>()), Times.Once);
            _mockUserRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<string>(), It.IsAny<System.Threading.CancellationToken>()), Times.Never);
            _mockUserRepository.Verify(repo => repo.AddAsync(It.IsAny<User>(), It.IsAny<System.Threading.CancellationToken>()), Times.Never);
        }

        [Test]
        public void CreateUserAsync_WhenEmailIsTaken_ThrowsConflictException()
        {
            // Arrange
            var createUserDto = new CreateUserDto { Username = "newuser", Email = "existing@example.com", Password = "password", Role = UserRole.User };
            var existingUser = new User { Id = 1, Username = "otheruser", Email = "existing@example.com" };
            _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(createUserDto.Username, It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync((User)null);
            _mockUserRepository.Setup(repo => repo.GetByEmailAsync(createUserDto.Email, It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(existingUser);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ConflictException>(async () => await _userManagementService.CreateUserAsync(createUserDto));
            Assert.AreEqual("Email is already registered", ex.Message);
            _mockUserRepository.Verify(repo => repo.GetByUsernameAsync(createUserDto.Username, It.IsAny<System.Threading.CancellationToken>()), Times.Once);
            _mockUserRepository.Verify(repo => repo.GetByEmailAsync(createUserDto.Email, It.IsAny<System.Threading.CancellationToken>()), Times.Once);
            _mockUserRepository.Verify(repo => repo.AddAsync(It.IsAny<User>(), It.IsAny<System.Threading.CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task UpdateUserAsync_WhenUserExistsAndDataIsValid_UpdatesAndReturnsUser()
        {
            // Arrange
            var userId = 1;
            var existingUser = new User { Id = userId, Username = "olduser", Email = "old@example.com", Role = UserRole.User, PasswordHash = "oldhash" };
            var updateUserDto = new UpdateUserDto
            {
                Username = "updateduser",
                Email = "updated@example.com",
                Role = UserRole.Admin,
                Password = "newpassword123" // Password change
            };

            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(existingUser);
            _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(updateUserDto.Username, It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync((User)null); 
            _mockUserRepository.Setup(repo => repo.UpdateAsync(It.IsAny<User>(), It.IsAny<System.Threading.CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            var result = await _userManagementService.UpdateUserAsync(userId, updateUserDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.Id);
            Assert.AreEqual(updateUserDto.Username, result.Username);
            Assert.AreEqual(updateUserDto.Email, result.Email);
            Assert.AreEqual(updateUserDto.Role, result.Role);
            _mockUserRepository.Verify(repo => repo.GetUserByIdAsync(userId), Times.Once);
            _mockUserRepository.Verify(repo => repo.GetByUsernameAsync(updateUserDto.Username, It.IsAny<System.Threading.CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task UpdateUserAsync_WithoutPasswordChange_KeepsOldPasswordHash()
        {
            // Arrange
            var userId = 1;
            var oldPasswordHash = new PasswordHasher<User>().HashPassword(new User(), "oldPassword");
            var existingUser = new User { Id = userId, Username = "currentuser", Email = "current@example.com", Role = UserRole.User, PasswordHash = oldPasswordHash };
            var updateUserDto = new UpdateUserDto
            {
                Username = "newusername",
                Email = "newemail@example.com",
                Role = UserRole.Admin,
                Password = null 
            };
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(existingUser);
            _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(updateUserDto.Username, It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync((User)null);
            _mockUserRepository.Setup(repo => repo.UpdateAsync(It.IsAny<User>(), It.IsAny<System.Threading.CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            var result = await _userManagementService.UpdateUserAsync(userId, updateUserDto);

            // Assert
            Assert.IsNotNull(result);
            _mockUserRepository.Verify(repo => repo.UpdateAsync(It.Is<User>(u =>
                u.Id == userId &&
                u.PasswordHash == oldPasswordHash
            ), It.IsAny<System.Threading.CancellationToken>()), Times.Once);
        }


        [Test]
        public async Task UpdateUserAsync_WhenUsernameNotChanged_SkipsUsernameAvailabilityCheck()
        {
            // Arrange
            var userId = 1;
            var username = "sameuser";
            var existingUser = new User { Id = userId, Username = username, Email = "old@example.com", Role = UserRole.User, PasswordHash = "oldhash" };
            var updateUserDto = new UpdateUserDto
            {
                Username = username, 
                Email = "updated@example.com",
                Role = UserRole.Admin,
                Password = "newpassword123"
            };
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(existingUser);
            _mockUserRepository.Setup(repo => repo.UpdateAsync(It.IsAny<User>(), It.IsAny<System.Threading.CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            var result = await _userManagementService.UpdateUserAsync(userId, updateUserDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.Id);
            Assert.AreEqual(updateUserDto.Username, result.Username);
            _mockUserRepository.Verify(repo => repo.GetUserByIdAsync(userId), Times.Once);
            _mockUserRepository.Verify(repo => repo.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<System.Threading.CancellationToken>()), Times.Never); 
            _mockUserRepository.Verify(repo => repo.UpdateAsync(It.Is<User>(u => u.Id == userId && u.Email == updateUserDto.Email), It.IsAny<System.Threading.CancellationToken>()), Times.Once);
        }


        [Test]
        public void UpdateUserAsync_WhenUserDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var userId = 1;
            var updateUserDto = new UpdateUserDto { Username = "any", Email = "any@example.com", Role = UserRole.User };
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(async () => await _userManagementService.UpdateUserAsync(userId, updateUserDto));
            Assert.AreEqual($"User with ID {userId} not found", ex.Message);
            _mockUserRepository.Verify(repo => repo.GetUserByIdAsync(userId), Times.Once);
            _mockUserRepository.Verify(repo => repo.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<System.Threading.CancellationToken>()), Times.Never);
            _mockUserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>(), It.IsAny<System.Threading.CancellationToken>()), Times.Never);
        }

        [Test]
        public void UpdateUserAsync_WhenNewUsernameIsTakenByAnotherUser_ThrowsConflictException()
        {
            // Arrange
            var userIdToUpdate = 1;
            var existingUserToUpdate = new User { Id = userIdToUpdate, Username = "user1", Email = "user1@example.com" };
            var updateUserDto = new UpdateUserDto { Username = "takenuser", Email = "newemail@example.com", Role = UserRole.User };
            var otherUserWithTakenUsername = new User { Id = 2, Username = "takenuser", Email = "other@example.com" }; // Different ID

            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userIdToUpdate)).ReturnsAsync(existingUserToUpdate);
            _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(updateUserDto.Username, It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(otherUserWithTakenUsername);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ConflictException>(async () => await _userManagementService.UpdateUserAsync(userIdToUpdate, updateUserDto));
            Assert.AreEqual("Username is already taken", ex.Message);
            _mockUserRepository.Verify(repo => repo.GetUserByIdAsync(userIdToUpdate), Times.Once);
            _mockUserRepository.Verify(repo => repo.GetByUsernameAsync(updateUserDto.Username, It.IsAny<System.Threading.CancellationToken>()), Times.Once);
            _mockUserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>(), It.IsAny<System.Threading.CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task UpdateUserAsync_WhenNewUsernameIsTakenBySameUser_UpdatesSuccessfully()
        {
            // Arrange
            var userId = 1;
            var originalUsername = "originalUser";
            var newUsernameAttempt = "newUsername"; // This will be "taken" by the same user.
            var userBeingUpdated = new User { Id = userId, Username = originalUsername, Email = "original@example.com", Role = UserRole.User };
            var userReturnedByUsernameCheck = new User { Id = userId, Username = newUsernameAttempt, Email = "original@example.com", Role = UserRole.User };
            var updateUserDto = new UpdateUserDto
            {
                Username = newUsernameAttempt, 
                Email = "updated@example.com",
                Role = UserRole.Admin
            };
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(userBeingUpdated);
            _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(newUsernameAttempt, It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(userReturnedByUsernameCheck);
            _mockUserRepository.Setup(repo => repo.UpdateAsync(It.IsAny<User>(), It.IsAny<System.Threading.CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            var result = await _userManagementService.UpdateUserAsync(userId, updateUserDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.Id);
            Assert.AreEqual(newUsernameAttempt, result.Username); 
            Assert.AreEqual(updateUserDto.Email, result.Email);
            Assert.AreEqual(updateUserDto.Role, result.Role);
            _mockUserRepository.Verify(repo => repo.GetUserByIdAsync(userId), Times.Once);
            _mockUserRepository.Verify(repo => repo.GetByUsernameAsync(newUsernameAttempt, It.IsAny<System.Threading.CancellationToken>()), Times.Once);
            _mockUserRepository.Verify(repo => repo.UpdateAsync(It.Is<User>(u =>
                u.Id == userId &&
                u.Username == newUsernameAttempt &&
                u.Email == updateUserDto.Email
            ), It.IsAny<System.Threading.CancellationToken>()), Times.Once);
        }


        [Test]
        public async Task DeleteUserAsync_WhenUserExists_DeletesUserAndReturnsTrue()
        {
            // Arrange
            var userId = 1;
            _mockUserRepository.Setup(repo => repo.DeleteAsync(userId, It.IsAny<System.Threading.CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            var result = await _userManagementService.DeleteUserAsync(userId);

            // Assert
            Assert.IsTrue(result);
            _mockUserRepository.Verify(repo => repo.DeleteAsync(userId, It.IsAny<System.Threading.CancellationToken>()), Times.Once);
        }

        [Test]
        public void DeleteUserAsync_WhenRepositoryThrowsException_PropagatesException()
        {
            // Arrange
            var userId = 1;
            var dbException = new System.InvalidOperationException("Database error during delete");
            _mockUserRepository.Setup(repo => repo.DeleteAsync(userId, It.IsAny<System.Threading.CancellationToken>())).ThrowsAsync(dbException);

            // Act & Assert
            var ex = Assert.ThrowsAsync<System.InvalidOperationException>(async () => await _userManagementService.DeleteUserAsync(userId));
            Assert.AreEqual(dbException.Message, ex.Message); 
            _mockUserRepository.Verify(repo => repo.DeleteAsync(userId, It.IsAny<System.Threading.CancellationToken>()), Times.Once);
        }

        [Test]
        public void UpdateUserAsync_WhenEmailIsTakenByAnotherUser_ThrowsConflictException()
        {
            // Arrange  
            var userIdToUpdate = 1;
            var existingUserToUpdate = new User { Id = userIdToUpdate, Username = "user1", Email = "user1@example.com" };
            var updateUserDto = new UpdateUserDto { Username = "user1", Email = "takenemail@example.com", Role = UserRole.User };
            var otherUserWithTakenEmail = new User { Id = 2, Username = "user2", Email = "takenemail@example.com" }; // Different ID  

            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userIdToUpdate)).ReturnsAsync(existingUserToUpdate);
            _mockUserRepository.Setup(repo => repo.GetByEmailAsync(updateUserDto.Email, It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(otherUserWithTakenEmail);

            // Act & Assert  
            var ex = Assert.ThrowsAsync<ConflictException>(async () => await _userManagementService.UpdateUserAsync(userIdToUpdate, updateUserDto));
            Assert.AreEqual("Email is already registered", ex.Message);
            _mockUserRepository.Verify(repo => repo.GetUserByIdAsync(userIdToUpdate), Times.Once);
            _mockUserRepository.Verify(repo => repo.GetByEmailAsync(updateUserDto.Email, It.IsAny<System.Threading.CancellationToken>()), Times.Once);
            _mockUserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>(), It.IsAny<System.Threading.CancellationToken>()), Times.Never);
        }
    }
}