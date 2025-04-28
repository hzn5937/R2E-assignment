using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.DTOs.Category;
using LibraryManagement.Application.Extensions;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<PaginatedCategoryOutputDto> GetAllAsync(int pageNum=1, int pageSize=5)
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

            int totalCount = categoryList.Count;
            int totalPage = (int)Math.Ceiling(totalCount / (double)pageSize);

            var paginatedCategories = categoryList.Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();
                
            var output = new PaginatedCategoryOutputDto()
            {
                Categories = paginatedCategories,
                PageSize = pageSize,
                PageNumber = pageNum,
                TotalPage = totalPage,
                TotalCount = totalCount,
                HasNext = pageNum < totalPage,
                HasPrev = pageNum > 1,
            };

            return output;
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

            if (existing is not null)
            {
                throw new ConflictException($"Category with name: {createCategoryDto.Name} already exists!");
            }

            var category = new Category
            {
                Name = createCategoryDto.Name,
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
            var existing = await _categoryRepository.GetByIdAsync(id);

            if (existing is null)
            {
                throw new NotFoundException($"There is no category entry with Id: {id}!");
            }

            var duplicate = await _categoryRepository.GetByNameAsync(updateCategoryDto.Name);

            if (duplicate is not null)
            {
                throw new ConflictException($"Category with name: {updateCategoryDto.Name} already exists!");
            }

            existing.Name = updateCategoryDto.Name;

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
