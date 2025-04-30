using NUnit.Framework;
using Moq;
using LibraryManagement.Api.Controllers;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.DTOs.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Application.Extensions.Exceptions;
using System;

namespace LibraryManagement.Api.Test
{
    [TestFixture]
    public class AuthenticationControllerTests
    {
        private Mock<IAuthenticationService> _mockAuthService;
        private AuthenticationController _authController;

        [SetUp]
        public void Setup()
        {
            _mockAuthService = new Mock<IAuthenticationService>();
            _authController = new AuthenticationController(_mockAuthService.Object);
        }

        // --- Register Tests ---

        [Test]
        public async Task Register_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var registerDto = new RegisterRequestDto { Username = "newuser", Password = "password", Role = UserRole.User };
            var loginOutputDto = new LoginOutputDto { Username = registerDto.Username, Role = registerDto.Role, AccessToken = "abc", RefreshToken = "xyz" };
            _mockAuthService.Setup(s => s.RegisterAsync(registerDto, It.IsAny<CancellationToken>())).ReturnsAsync(loginOutputDto);

            // Act
            var result = await _authController.Register(registerDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(loginOutputDto, okResult.Value);
            _mockAuthService.Verify(s => s.RegisterAsync(registerDto, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Register_WithNullData_ReturnsBadRequest()
        {
            // Arrange
            RegisterRequestDto registerDto = null;

            // Act
            var result = await _authController.Register(registerDto);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual("Invalid registration request", (result as BadRequestObjectResult).Value);
            _mockAuthService.Verify(s => s.RegisterAsync(It.IsAny<RegisterRequestDto>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task Register_WhenServiceReturnsNull_ReturnsBadRequest() // Assuming null return indicates failure other than conflict
        {
            // Arrange
            var registerDto = new RegisterRequestDto { Username = "faileduser", Password = "password", Role = UserRole.User };
            _mockAuthService.Setup(s => s.RegisterAsync(registerDto, It.IsAny<CancellationToken>())).ReturnsAsync((LoginOutputDto)null);

            // Act
            var result = await _authController.Register(registerDto);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual("User registration failed", (result as BadRequestObjectResult).Value);
            _mockAuthService.Verify(s => s.RegisterAsync(registerDto, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void Register_WhenServiceThrowsConflictException_ThrowsConflictException()
        {
            // Arrange
            var registerDto = new RegisterRequestDto { Username = "existing", Password = "password", Role = UserRole.User };
            _mockAuthService.Setup(s => s.RegisterAsync(registerDto, It.IsAny<CancellationToken>()))
                            .ThrowsAsync(new ConflictException("Username taken"));

            // Act & Assert
            Assert.ThrowsAsync<ConflictException>(async () => await _authController.Register(registerDto));
            _mockAuthService.Verify(s => s.RegisterAsync(registerDto, It.IsAny<CancellationToken>()), Times.Once);
        }

        // --- Login Tests ---

        [Test]
        public async Task Login_WithValidCredentials_ReturnsOkResultWithToken()
        {
            // Arrange
            var loginDto = new LoginDto { Username = "test", Password = "password" };
            var loginOutputDto = new LoginOutputDto { Username = loginDto.Username, AccessToken = "valid_token", RefreshToken = "refresh" };
            _mockAuthService.Setup(s => s.VerifyUserAsync(loginDto, It.IsAny<CancellationToken>())).ReturnsAsync(loginOutputDto);

            // Act
            var result = await _authController.Login(loginDto, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            // The controller wraps the token in an anonymous object: new { token = loginOutputDto }
            Assert.IsNotNull(okResult.Value);
            var resultValue = okResult.Value;
            var tokenProperty = resultValue.GetType().GetProperty("token");
            Assert.IsNotNull(tokenProperty);
            Assert.AreEqual(loginOutputDto, tokenProperty.GetValue(resultValue));

            _mockAuthService.Verify(s => s.VerifyUserAsync(loginDto, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Login_WithInvalidCredentials_ReturnsUnauthorizedResult()
        {
            // Arrange
            var loginDto = new LoginDto { Username = "test", Password = "wrongpassword" };
            _mockAuthService.Setup(s => s.VerifyUserAsync(loginDto, It.IsAny<CancellationToken>())).ReturnsAsync((LoginOutputDto)null); // Service returns null for invalid login

            // Act
            var result = await _authController.Login(loginDto, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
            _mockAuthService.Verify(s => s.VerifyUserAsync(loginDto, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void Login_WhenServiceThrowsNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var loginDto = new LoginDto { Username = "notfound", Password = "password" };
            _mockAuthService.Setup(s => s.VerifyUserAsync(loginDto, It.IsAny<CancellationToken>()))
                           .ThrowsAsync(new NotFoundException("User not found"));

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(async () => await _authController.Login(loginDto, CancellationToken.None));
            _mockAuthService.Verify(s => s.VerifyUserAsync(loginDto, It.IsAny<CancellationToken>()), Times.Once);
        }

        // --- Refresh Tests ---

        [Test]
        public async Task Refresh_WithValidTokens_ReturnsOkResultWithNewToken()
        {
            // Arrange
            var refreshDto = new RefreshRequestDto { AccessToken = "expired", RefreshToken = "valid_refresh" };
            var newLoginOutputDto = new LoginOutputDto { Username = "test", AccessToken = "new_valid_token", RefreshToken = "new_refresh" };
            _mockAuthService.Setup(s => s.RefreshAsync(refreshDto, It.IsAny<CancellationToken>())).ReturnsAsync(newLoginOutputDto);

            // Act
            var result = await _authController.Refresh(refreshDto, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            var resultValue = okResult.Value;
            var tokenProperty = resultValue.GetType().GetProperty("token"); // Controller wraps in new { token = ... }
            Assert.IsNotNull(tokenProperty);
            Assert.AreEqual(newLoginOutputDto, tokenProperty.GetValue(resultValue));
            _mockAuthService.Verify(s => s.RefreshAsync(refreshDto, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Refresh_WithInvalidTokens_ReturnsUnauthorizedResult()
        {
            // Arrange
            var refreshDto = new RefreshRequestDto { AccessToken = "expired", RefreshToken = "invalid_refresh" };
            _mockAuthService.Setup(s => s.RefreshAsync(refreshDto, It.IsAny<CancellationToken>())).ReturnsAsync((LoginOutputDto)null); // Service returns null for invalid refresh

            // Act
            var result = await _authController.Refresh(refreshDto, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
            var unauthorizedResult = result as UnauthorizedObjectResult;
            Assert.AreEqual("Invalid or expired refresh token", unauthorizedResult.Value);
            _mockAuthService.Verify(s => s.RefreshAsync(refreshDto, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}