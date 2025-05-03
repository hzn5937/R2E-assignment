using LibraryManagement.Application.DTOs.Category;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCategories([FromQuery] int pageNum = 1, [FromQuery] int pageSize = 5)
        {
            var books = await _categoryService.GetAllAsync(pageNum, pageSize);

            return Ok(books);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category is null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            return Ok(category);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            var createdCategory = await _categoryService.CreateAsync(createCategoryDto);
            return Ok(createdCategory);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto updateCategoryDto)
        {
            var updatedCategory = await _categoryService.UpdateAsync(id, updateCategoryDto);
            if (updatedCategory is null)
            {
                return NotFound($"Category with ID {id} not found.");
            }
            return Ok(updatedCategory);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var deleted = await _categoryService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            return NoContent();
        }
    }
}
