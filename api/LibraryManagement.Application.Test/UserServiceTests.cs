using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Interfaces;
using Moq;

namespace LibraryManagement.Application.Test
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _mockUserRepository;
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserRepository.Object);
        }

        [Test]
        public async Task GetUserCountAsync_WhenNoUsersExist_ReturnsZeroCounts()
        {
            // Arrange
            var emptyUserList = new List<User>();
            _mockUserRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(emptyUserList); 

            // Act
            var result = await _userService.GetUserCountAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.TotalUsers, "TotalUsers should be 0 when repository is empty.");
            Assert.AreEqual(0, result.TotalAdmin, "TotalAdmin should be 0 when repository is empty.");
            _mockUserRepository.Verify(repo => repo.GetAllAsync(), Times.Once); // Verify the dependency was called
        }

        [Test]
        public async Task GetUserCountAsync_WhenOnlyAdminUsersExist_ReturnsZeroTotalUsersAndNegativeTotalAdmin()
        {
            // Arrange
            var adminUsers = new List<User>
        {
            new User { Id = 1, Username = "Admin One", Role = UserRole.Admin },
            new User { Id = 2, Username = "Admin Two", Role = UserRole.Admin }
        };
            _mockUserRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(adminUsers); // Mock repository returns only admins

            // Act
            var result = await _userService.GetUserCountAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.TotalUsers, "TotalUsers should be 0 when only admins exist.");
            Assert.AreEqual(2, result.TotalAdmin, "TotalAdmin should be calculated correctly based on the formula.");
            _mockUserRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetUserCountAsync_WhenOnlyStandardUsersExist_ReturnsCorrectTotalUsersAndZeroTotalAdmin()
        {
            // Arrange
            var standardUsers = new List<User>
        {
            new User { Id = 3, Username = "User One", Role = UserRole.User },
            new User { Id = 4, Username = "User Two", Role = UserRole.User },
            new User { Id = 5, Username = "User Three", Role = UserRole.User }
        };
            _mockUserRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(standardUsers);

            // Act
            var result = await _userService.GetUserCountAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.TotalUsers, "TotalUsers should be the count of standard users.");
            Assert.AreEqual(0, result.TotalAdmin, "TotalAdmin should be 0 when only standard users exist.");
            _mockUserRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetUserCountAsync_WhenMixedUserRolesExist_ReturnsCorrectCountsBasedOnFormula()
        {
            // Arrange
            var mixedUsers = new List<User>
        {
            new User { Id = 1, Username = "Admin One", Role = UserRole.Admin },
            new User { Id = 3, Username = "User One", Role = UserRole.User },
            new User { Id = 4, Username = "User Two", Role = UserRole.User },
            new User { Id = 6, Username = "Guest One", Role = UserRole.User } 
        };
            _mockUserRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(mixedUsers);

            // Act
            var result = await _userService.GetUserCountAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.TotalUsers, "TotalUsers should count only users with UserRole.User.");
            Assert.AreEqual(1, result.TotalAdmin, "TotalAdmin should be calculated correctly based on the formula with mixed roles.");
            _mockUserRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }
    }
}

