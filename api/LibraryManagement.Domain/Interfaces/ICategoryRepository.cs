using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category> CreateAsync(Category category);
        Task<Category> UpdateAsync(Category category);
        Task DeleteAsync(int id);
        Task<Category?> GetByNameAsync(string name);
    }
}
