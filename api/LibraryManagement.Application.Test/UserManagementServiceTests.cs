using Moq;
using LibraryManagement.Application.DTOs.UserManagement;
using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
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
        private PasswordHasher<User> _hasher; 

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userManagementService = new UserManagementService(_mockUserRepository.Object);
            _hasher = new PasswordHasher<User>();x
        }

        private List<User> GetMockUsers()
        {
            return new List<User>
            {
                new User { Id = 1, Username = "user1", Email = "user1@test.com", Role = UserRole.User, PasswordHash = "hashedpassword1" },
                new User { Id = 2, Username = "admin1", Email = "admin1@test.com", Role = UserRole.Admin, PasswordHash = "hashedpassword2" },
                new User { Id = 3, Username = "user2", Email = "user2@test.com", Role = UserRole.User, PasswordHash = "hashedpassword3" }
            };
        }

        [Test]
        public async Task GetAllUsersAsync_WhenUsersExist_ReturnsAllUserDtos()
        {
            // Arrange
            var mockUsers = GetMockUsers();
            _mockUserRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(mockUsers);

            // Act
            var result = await _userManagementService.GetAllUsersAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(mockUsers.Count, result.Count());
            Assert.IsTrue(result.All(dto => mockUsers.Any(u => u.Id == dto.Id && u.Username == dto.Username && u.Email == dto.Email && u.Role == dto.Role)));
            _mockUserRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetAllUsersAsync_WhenNoUsersExist_ReturnsEmptyList()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<User>());

            // Act
            var result = await _userManagementService.GetAllUsersAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
            _mockUserRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetUserByIdAsync_WhenUserExists_ReturnsUserDto()
        {
            // Arrange
            int userId = 1;
            var mockUser = GetMockUsers().First(u => u.Id == userId);
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(mockUser);

            // Act
            var result = await _userManagementService.GetUserByIdAsync(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(mockUser.Id, result.Id);
            Assert.AreEqual(mockUser.Username, result.Username);
            Assert.AreEqual(mockUser.Email, result.Email);
            Assert.AreEqual(mockUser.Role, result.Role);
            _mockUserRepository.Verify(repo => repo.GetUserByIdAsync(userId), Times.Once);
        }

        [Test]
        public void GetUserByIdAsync_WhenUserDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            int userId = 99;
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() => _userManagementService.GetUserByIdAsync(userId));
            Assert.That(ex.Message, Is.EqualTo($"User with ID {userId} not found"));
            _mockUserRepository.Verify(repo => repo.GetUserByIdAsync(userId), Times.Once);
        }

        [Test]
        public async Task CreateUserAsync_WhenUsernameAndEmailAreUnique_CreatesUserAndReturnsDto()
        {
            // Arrange
            var createUserDto = new CreateUserDto
            {
                Username = "newuser",
                Email = "new@test.com",
                Password = "password123",
                Role = UserRole.User
            };
            _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(createUserDto.Username, It.IsAny<CancellationToken>())).ReturnsAsync((User)null);
            _mockUserRepository.Setup(repo => repo.GetByEmailAsync(createUserDto.Email, It.IsAny<CancellationToken>())).ReturnsAsync((User)null);
            _mockUserRepository.Setup(repo => repo.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Callback<User, CancellationToken>((user, ct) => user.Id = 10) 
                .Returns(Task.CompletedTask);

            // Act
            var result = await _userManagementService.CreateUserAsync(createUserDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.Id); 
            Assert.AreEqual(createUserDto.Username, result.Username);
            Assert.AreEqual(createUserDto.Email, result.Email);
            Assert.AreEqual(createUserDto.Role, result.Role);
            _mockUserRepository.Verify(repo => repo.GetByUsernameAsync(createUserDto.Username, It.IsAny<CancellationToken>()), Times.Once);
            _mockUserRepository.Verify(repo => repo.GetByEmailAsync(createUserDto.Email, It.IsAny<CancellationToken>()), Times.Once);
            _mockUserRepository.Verify(repo => repo.AddAsync(
                It.Is<User>(u =>
                    u.Username == createUserDto.Username &&
                    u.Email == createUserDto.Email &&
                    u.Role == createUserDto.Role &&
                    !string.IsNullOrEmpty(u.PasswordHash) 
                ),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void CreateUserAsync_WhenUsernameExists_ThrowsConflictException()
        {
            // Arrange
            var createUserDto = new CreateUserDto { Username = "existinguser", Email = "new@test.com", Password = "password123", Role = UserRole.User };
            var existingUser = new User { Id = 1, Username = "existinguser", Email = "old@test.com" };
            _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(createUserDto.Username, It.IsAny<CancellationToken>())).ReturnsAsync(existingUser);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ConflictException>(() => _userManagementService.CreateUserAsync(createUserDto));
            Assert.That(ex.Message, Is.EqualTo("Username is already taken"));
            _mockUserRepository.Verify(repo => repo.GetByUsernameAsync(createUserDto.Username, It.IsAny<CancellationToken>()), Times.Once);
            _mockUserRepository.Verify(repo => repo.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            _mockUserRepository.Verify(repo => repo.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public void CreateUserAsync_WhenEmailExists_ThrowsConflictException()
        {
            // Arrange
            var createUserDto = new CreateUserDto { Username = "newuser", Email = "existing@test.com", Password = "password123", Role = UserRole.User };
            var existingUser = new User { Id = 1, Username = "anotheruser", Email = "existing@test.com" };
            _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(createUserDto.Username, It.IsAny<CancellationToken>())).ReturnsAsync((User)null);
            _mockUserRepository.Setup(repo => repo.GetByEmailAsync(createUserDto.Email, It.IsAny<CancellationToken>())).ReturnsAsync(existingUser);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ConflictException>(() => _userManagementService.CreateUserAsync(createUserDto));
            Assert.That(ex.Message, Is.EqualTo("Email is already registered"));
            _mockUserRepository.Verify(repo => repo.GetByUsernameAsync(createUserDto.Username, It.IsAny<CancellationToken>()), Times.Once);
            _mockUserRepository.Verify(repo => repo.GetByEmailAsync(createUserDto.Email, It.IsAny<CancellationToken>()), Times.Once);
            _mockUserRepository.Verify(repo => repo.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task UpdateUserAsync_WhenUserExistsAndValidData_UpdatesUserAndReturnsDto()
        {
            // Arrange
            int userId = 1;
            var updateUserDto = new UpdateUserDto
            {
                Username = "updateduser",
                Email = "updated@test.com",
                Role = UserRole.Admin,
                Password = null 
            };
            var existingUser = GetMockUsers().First(u => u.Id == userId);
            var originalPasswordHash = existingUser.PasswordHash;
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(existingUser);
            _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(updateUserDto.Username, It.IsAny<CancellationToken>())).ReturnsAsync((User)null);
            _mockUserRepository.Setup(repo => repo.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            var result = await _userManagementService.UpdateUserAsync(userId, updateUserDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.Id);
            Assert.AreEqual(updateUserDto.Username, result.Username);
            Assert.AreEqual(updateUserDto.Email, result.Email);
            Assert.AreEqual(updateUserDto.Role, result.Role);
            _mockUserRepository.Verify(repo => repo.GetUserByIdAsync(userId), Times.Once);
            _mockUserRepository.Verify(repo => repo.GetByUsernameAsync(updateUserDto.Username, It.IsAny<CancellationToken>()), Times.Once); 
            _mockUserRepository.Verify(repo => repo.UpdateAsync(
                It.Is<User>(u =>
                    u.Id == userId &&
                    u.Username == updateUserDto.Username &&
                    u.Email == updateUserDto.Email &&
                    u.Role == updateUserDto.Role &&
                    u.PasswordHash == originalPasswordHash 
                ),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task UpdateUserAsync_WhenUserExistsAndPasswordChanged_UpdatesUserWithNewHashAndReturnsDto()
        {
            // Arrange
            int userId = 1;
            var updateUserDto = new UpdateUserDto
            {
                Username = "updateduser",
                Email = "updated@test.com",
                Role = UserRole.Admin,
                Password = "newpassword123"
            };
            var existingUser = GetMockUsers().First(u => u.Id == userId);
            var originalPasswordHash = existingUser.PasswordHash;
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(existingUser);
            _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(updateUserDto.Username, It.IsAny<CancellationToken>())).ReturnsAsync((User)null);
            _mockUserRepository.Setup(repo => repo.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            var result = await _userManagementService.UpdateUserAsync(userId, updateUserDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.Id);
            Assert.AreEqual(updateUserDto.Username, result.Username);
            Assert.AreEqual(updateUserDto.Email, result.Email);
            Assert.AreEqual(updateUserDto.Role, result.Role);
            _mockUserRepository.Verify(repo => repo.GetUserByIdAsync(userId), Times.Once);
            _mockUserRepository.Verify(repo => repo.GetByUsernameAsync(updateUserDto.Username, It.IsAny<CancellationToken>()), Times.Once);
            _mockUserRepository.Verify(repo => repo.UpdateAsync(
                It.Is<User>(u =>
                    u.Id == userId &&
                    u.Username == updateUserDto.Username &&
                    u.Email == updateUserDto.Email &&
                    u.Role == updateUserDto.Role &&
                    u.PasswordHash != originalPasswordHash && 
                    _hasher.VerifyHashedPassword(u, u.PasswordHash, updateUserDto.Password) == PasswordVerificationResult.Success 
                ),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task UpdateUserAsync_WhenUsernameIsSame_DoesNotCheckForUsernameConflict()
        {
            // Arrange
            int userId = 1;
            var existingUser = GetMockUsers().First(u => u.Id == userId);
            var updateUserDto = new UpdateUserDto
            {
                Username = existingUser.Username, 
                Email = "updated@test.com",
                Role = UserRole.Admin,
                Password = null
            };
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(existingUser);
            _mockUserRepository.Setup(repo => repo.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            await _userManagementService.UpdateUserAsync(userId, updateUserDto);

            // Assert
            _mockUserRepository.Verify(repo => repo.GetUserByIdAsync(userId), Times.Once);
            _mockUserRepository.Verify(repo => repo.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            _mockUserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        }


        [Test]
        public void UpdateUserAsync_WhenUserDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            int userId = 99;
            var updateUserDto = new UpdateUserDto { Username = "anyuser", Email = "any@test.com", Role = UserRole.User };
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() => _userManagementService.UpdateUserAsync(userId, updateUserDto));
            Assert.That(ex.Message, Is.EqualTo($"User with ID {userId} not found"));
            _mockUserRepository.Verify(repo => repo.GetUserByIdAsync(userId), Times.Once);
            _mockUserRepository.Verify(repo => repo.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            _mockUserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public void UpdateUserAsync_WhenNewUsernameConflictsWithAnotherUser_ThrowsConflictException()
        {
            // Arrange
            int userIdToUpdate = 1;
            int conflictingUserId = 2;
            var updateUserDto = new UpdateUserDto { Username = "conflictinguser", Email = "updated@test.com", Role = UserRole.Admin };
            var userToUpdate = GetMockUsers().First(u => u.Id == userIdToUpdate); 
            var conflictingUser = GetMockUsers().First(u => u.Id == conflictingUserId); 
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userIdToUpdate)).ReturnsAsync(userToUpdate);
            _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(updateUserDto.Username, It.IsAny<CancellationToken>())).ReturnsAsync(conflictingUser);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ConflictException>(() => _userManagementService.UpdateUserAsync(userIdToUpdate, updateUserDto));
            Assert.That(ex.Message, Is.EqualTo("Username is already taken"));
            _mockUserRepository.Verify(repo => repo.GetUserByIdAsync(userIdToUpdate), Times.Once);
            _mockUserRepository.Verify(repo => repo.GetByUsernameAsync(updateUserDto.Username, It.IsAny<CancellationToken>()), Times.Once);
            _mockUserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task DeleteUserAsync_WhenCalled_CallsRepositoryDelete()
        {
            // Arrange
            int userId = 1;
            _mockUserRepository.Setup(repo => repo.DeleteAsync(userId, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            var result = await _userManagementService.DeleteUserAsync(userId);

            // Assert
            Assert.IsTrue(result); 
            _mockUserRepository.Verify(repo => repo.DeleteAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void DeleteUserAsync_WhenRepositoryThrows_ExceptionPropagates()
        {
            // Arrange
            int userId = 1;
            var testException = new InvalidOperationException("Database error");
            _mockUserRepository.Setup(repo => repo.DeleteAsync(userId, It.IsAny<CancellationToken>())).ThrowsAsync(testException);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => _userManagementService.DeleteUserAsync(userId));
            _mockUserRepository.Verify(repo => repo.DeleteAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}