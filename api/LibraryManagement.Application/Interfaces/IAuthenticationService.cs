using LibraryManagement.Application.DTOs.Authentication;

namespace LibraryManagement.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<LoginOutputDto?> VerifyUserAsync(LoginDto loginDto, CancellationToken ct = default);
        Task<LoginOutputDto?> RefreshAsync(RefreshRequestDto refreshRequestDto, CancellationToken ct = default);
        Task<LoginOutputDto?> RegisterAsync(RegisterRequestDto registerRequestDto, CancellationToken ct = default);
    }
}
    