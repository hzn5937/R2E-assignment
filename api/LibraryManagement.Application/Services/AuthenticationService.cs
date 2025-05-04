using LibraryManagement.Application.DTOs.Authentication;
using LibraryManagement.Application.Extensions.Exceptions;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Common;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LibraryManagement.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly PasswordHasher<User> _hasher = new();
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IOptions<JwtSettings> options, IUserRepository userRepository)
        {
            _jwtSettings = options.Value;
            _userRepository = userRepository;
        }

        public async Task<LoginOutputDto?> RegisterAsync(RegisterRequestDto registerRequestDto, CancellationToken ct = default)
        {
            var existing = await _userRepository.GetByUsernameAsync(registerRequestDto.Username, ct);

            if (existing is not null)
            {
                throw new ConflictException("Username is already taken");
            }

            var user = new User
            {
                Username = registerRequestDto.Username,
                Email = registerRequestDto.Email,
                Role = registerRequestDto.Role
            };

            user.PasswordHash = _hasher.HashPassword(user, registerRequestDto.Password);

            string refreshToken = GenerateRefreshToken();
            DateTime refreshTokenExpiry = DateTime.UtcNow.AddDays(Constants.RefreshTokenExpirationDays);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiry;

            await _userRepository.AddAsync(user, ct);

            var output = new LoginOutputDto()
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role,
                AccessToken = GenerateAccessToken(user),
                RefreshToken = refreshToken,
                RefreshTokenExpires = refreshTokenExpiry
            };

            return output;
        }

        public async Task<LoginOutputDto?> VerifyUserAsync(LoginDto loginDto, CancellationToken ct = default)
        {
            User? user = await _userRepository.GetByUsernameAsync(loginDto.Username);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            bool verified = _hasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password) == PasswordVerificationResult.Success;

            if (!verified)
            {
                return null;
            }

            // mapping then update
            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiry = DateTime.UtcNow.AddDays(Constants.RefreshTokenExpirationDays);

            var output = new LoginOutputDto()
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role,
                AccessToken = GenerateAccessToken(user),
                AccessTokenExpires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                RefreshToken = refreshToken,
                RefreshTokenExpires = refreshTokenExpiry
            };

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiry;

            await _userRepository.UpdateAsync(user, ct);

            return output;
        }

        public async Task<LoginOutputDto?> RefreshAsync(RefreshRequestDto refreshRequestDto, CancellationToken ct = default)
        {
            var principal = GetTokenPrincipal(refreshRequestDto.AccessToken);

            // The token is invalid or missing identity information
            if (principal?.Identity?.Name is null)
            {
                return null;
            }

            var user = await _userRepository.GetByUsernameAsync(principal.Identity.Name, ct);

            // Check if user exists
            if (user == null)
            {
                return null;
            }
            
            // Check if refresh token is valid
            bool isTokenMismatch = user.RefreshToken != refreshRequestDto.RefreshToken;
            bool isTokenExpired = user.RefreshTokenExpiryTime <= DateTime.UtcNow;
            
            if (isTokenMismatch || isTokenExpired)
            {
                // Invalidate the refresh token if it's expired or doesn't match
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(-1); // Set to past time
                await _userRepository.UpdateAsync(user, ct);
                return null;
            }

            // Generate new tokens
            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiry = DateTime.UtcNow.AddDays(Constants.RefreshTokenExpirationDays);

            // Update the user with new refresh token
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiry;
            await _userRepository.UpdateAsync(user, ct);

            // Create output DTO
            var output = new LoginOutputDto()
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role,
                AccessToken = GenerateAccessToken(user),
                AccessTokenExpires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                RefreshToken = refreshToken,
                RefreshTokenExpires = refreshTokenExpiry
            };

            return output;
        }

        public async Task<bool> LogoutAsync(string username, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByUsernameAsync(username, ct);
            
            if (user == null)
            {
                return false;
            }
            
            // Invalidate the refresh token
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(-1); // Set to past time
            
            await _userRepository.UpdateAsync(user, ct);
            return true;
        }

        // helper functions
        public ClaimsPrincipal? GetTokenPrincipal(string jwtToken)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            var validation = new TokenValidationParameters
            {
                IssuerSigningKey = securityKey,
                ValidateLifetime = false,
                ValidateActor = false,
                ValidateIssuer = false,
                ValidateAudience = false,
            };

            // decode jwt token and get the principal (information inside the token)
            return new JwtSecurityTokenHandler().ValidateToken(jwtToken, validation, out _);
        }

        public string GenerateAccessToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, user.Username),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }

            return Convert.ToBase64String(randomNumber);

        }
    }
}
