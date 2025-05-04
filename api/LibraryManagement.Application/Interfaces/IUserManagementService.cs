using LibraryManagement.Application.DTOs.UserManagement;

namespace LibraryManagement.Application.Interfaces
{
    public interface IUserManagementService
    {
        Task<IEnumerable<UserOutputDto>> GetAllUsersAsync(CancellationToken ct = default);
        Task<UserOutputDto> GetUserByIdAsync(int id, CancellationToken ct = default);
        Task<UserOutputDto> CreateUserAsync(CreateUserDto createUserDto, CancellationToken ct = default);
        Task<UserOutputDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto, CancellationToken ct = default);
        Task<bool> DeleteUserAsync(int id, CancellationToken ct = default);
    }
}