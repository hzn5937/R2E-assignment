using LibraryManagement.Application.DTOs.Authentication;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthenticationController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequest)
        {
            if (registerRequest == null)
            {
                return BadRequest("Invalid registration request");
            }

            // Here you would typically save the user to the database
            var created = await _authService.RegisterAsync(registerRequest, CancellationToken.None);

            if (created == null)
            {
                return BadRequest("User registration failed");
            }

            return Ok(created);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto, CancellationToken ct)
        {
            var token = await _authService.VerifyUserAsync(loginDto, ct);

            if (token == null)
            {
                return Unauthorized("");
            }

            return Ok(token);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto refreshRequest, CancellationToken ct)
        {
            var refreshedToken = await _authService.RefreshAsync(refreshRequest, ct);

            if (refreshedToken == null)
            {
                return Unauthorized("Invalid or expired refresh token");
            }

            return Ok(refreshedToken);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(CancellationToken ct)
        {
            // Get username from the token claims
            var username = User.Identity?.Name;
            
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Not authenticated");
            }
            
            var result = await _authService.LogoutAsync(username, ct);
            
            if (!result)
            {
                return NotFound("User not found");
            }
            
            return Ok(new { message = "Logged out successfully" });
        }
    }
}
