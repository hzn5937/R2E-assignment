using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);
        Task UpdateAsync(User user, CancellationToken ct = default);
        Task AddAsync(User user, CancellationToken ct = default);
    }
}
