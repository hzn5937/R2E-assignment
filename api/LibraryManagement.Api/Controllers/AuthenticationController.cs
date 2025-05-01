using LibraryManagement.Application.DTOs.Authentication;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers
{
    [Route("api/[controller]")]
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

            return Ok(new { token });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto refreshRequest, CancellationToken ct)
        {
            var refreshedToken = await _authService.RefreshAsync(refreshRequest, ct);

            if (refreshedToken == null)
            {
                return Unauthorized("Invalid or expired refresh token");
            }

            return Ok(new { token = refreshedToken });
        }

    }
}
