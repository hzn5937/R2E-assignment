using LibraryManagement.Application.DTOs.UserManagement;
using LibraryManagement.Application.DTOs.Common;
using LibraryManagement.Domain.Common;

namespace LibraryManagement.Application.Interfaces
{
    public interface IUserManagementService
    {
        Task<PaginatedOutputDto<UserOutputDto>> GetAllUsersAsync(int pageNum = Constants.DefaultPageNum, int pageSize = Constants.DefaultPageSize, CancellationToken ct = default);
        Task<UserOutputDto> GetUserByIdAsync(int id, CancellationToken ct = default);
        Task<UserOutputDto> CreateUserAsync(CreateUserDto createUserDto, CancellationToken ct = default);
        Task<UserOutputDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto, CancellationToken ct = default);
        Task<bool> DeleteUserAsync(int id, CancellationToken ct = default);
    }
}