using LibraryManagement.Application.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Interfaces
{
    public interface ICategoryService
    {
        public Task<PaginatedCategoryOutputDto> GetAllAsync(int pageNum=1, int pageSize=5);
        Task<CategoryOutputDto?> GetByIdAsync(int id);
        Task<CategoryOutputDto> CreateAsync(CreateCategoryDto createCategoryDto);
        Task<CategoryOutputDto?> UpdateAsync(int id, UpdateCategoryDto updateCategoryDto);
        Task<bool> DeleteAsync(int id);
    }
}
