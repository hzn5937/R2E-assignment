using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);
        Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
        Task UpdateAsync(User user, CancellationToken ct = default);
        Task AddAsync(User user, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
