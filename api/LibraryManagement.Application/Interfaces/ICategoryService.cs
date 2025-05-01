using LibraryManagement.Application.DTOs.Category;
using LibraryManagement.Application.DTOs.Common;
using LibraryManagement.Domain.Common;

namespace LibraryManagement.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<PaginatedOutputDto<CategoryOutputDto>> GetAllAsync(int pageNum = Constants.DefaultPageNum, int pageSize = Constants.DefaultPageSize);
        Task<CategoryOutputDto?> GetByIdAsync(int id);
        Task<CategoryOutputDto> CreateAsync(CreateCategoryDto createCategoryDto);
        Task<CategoryOutputDto?> UpdateAsync(int id, UpdateCategoryDto updateCategoryDto);
        Task<bool> DeleteAsync(int id);
    }
}
