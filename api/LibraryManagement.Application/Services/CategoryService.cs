using LibraryManagement.Application.DTOs.Category;
using LibraryManagement.Application.DTOs.Common;
using LibraryManagement.Application.Extensions;
using LibraryManagement.Application.Extensions.Exceptions;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Common;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using System.Globalization;

namespace LibraryManagement.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<PaginatedOutputDto<CategoryOutputDto>> GetAllAsync(int pageNum=Constants.DefaultPageNum, int pageSize=Constants.DefaultPageSize)
        {
            var categories = await _categoryRepository.GetAllAsync();

            var categoryList = new List<CategoryOutputDto>();

            foreach (var category in categories)
            {
                var record = new CategoryOutputDto
                {
                    Id = category.Id,
                    Name = category.Name,
                };
                categoryList.Add(record);
            }

            var paginated = Pagination.Paginate<CategoryOutputDto>(categoryList, pageNum, pageSize);

            return paginated;
        }

        public async Task<CategoryOutputDto?> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category is null)
            {
                return null;
            }

            var output = new CategoryOutputDto
            {
                Id = category.Id,
                Name = category.Name,
            };

            return output;
        }

        public async Task<CategoryOutputDto> CreateAsync(CreateCategoryDto createCategoryDto)
        {
            Category? existing = await _categoryRepository.GetByNameAsync(createCategoryDto.Name);

            string processedName = CultureInfo.CurrentCulture.TextInfo
                .ToTitleCase(createCategoryDto.Name.ToLowerInvariant());

            if (existing is not null)
            {
                throw new ConflictException($"Category with name: {processedName} already exists!");
            }

            var category = new Category
            {
                Name = processedName,
            };

            var created = await _categoryRepository.CreateAsync(category);

            var output = new CategoryOutputDto
            {
                Id = created.Id,
                Name = created.Name,
            };

            return output;
        }

        public async Task<CategoryOutputDto?> UpdateAsync(int id, UpdateCategoryDto updateCategoryDto)
        {
            string processedName = CultureInfo.CurrentCulture.TextInfo
                .ToTitleCase(updateCategoryDto.Name.ToLowerInvariant());

            var existing = await _categoryRepository.GetByIdAsync(id);

            if (existing is null)
            {
                throw new NotFoundException($"There is no category entry with Id: {id}!");
            }

            var duplicate = await _categoryRepository.GetByNameAsync(processedName);

            if (duplicate is not null)
            {
                throw new ConflictException($"Category with name: {processedName} already exists!");
            }

            existing.Name = processedName;

            var updated = await _categoryRepository.UpdateAsync(existing);

            var output = new CategoryOutputDto
            {
                Id = updated.Id,
                Name = updated.Name,
            };

            return output;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _categoryRepository.GetByIdAsync(id);

            if (existing is null)
            {
                return false;
            }

            await _categoryRepository.DeleteAsync(id);

            return true;
        }
    }
}
