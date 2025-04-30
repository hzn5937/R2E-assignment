using NUnit.Framework;
using Moq;
using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.DTOs.Category;
using LibraryManagement.Application.Extensions.Exceptions;
using System.Threading.Tasks;
using System.Collections.Generic;
using LibraryManagement.Domain.Common;
using LibraryManagement.Application.DTOs.Common;
using System.Linq;

namespace LibraryManagement.Application.Test
{
    [TestFixture]
    public class CategoryServiceTests
    {
        private Mock<ICategoryRepository> _mockCategoryRepository;
        private CategoryService _categoryService;

        [SetUp]
        public void Setup()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _categoryService = new CategoryService(_mockCategoryRepository.Object);
        }

        [Test]
        public async Task GetAllAsync_WhenCategoriesExist_ReturnsPaginatedCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Fiction" },
                new Category { Id = 2, Name = "Science" }
            };
            var expectedDtoList = categories.Select(c => new CategoryOutputDto { Id = c.Id, Name = c.Name }).ToList();

            _mockCategoryRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(categories);

            // Act
            var result = await _categoryService.GetAllAsync(1, 10); // pageNum 1, pageSize 10

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.TotalCount);
            Assert.AreEqual(1, result.TotalPage);
            Assert.AreEqual(expectedDtoList.Count, result.Items.Count);
            Assert.AreEqual(expectedDtoList[0].Name, result.Items[0].Name);
            Assert.AreEqual(expectedDtoList[1].Id, result.Items[1].Id);
            _mockCategoryRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_WhenCategoryExists_ReturnsCategoryOutputDto()
        {
            // Arrange
            int categoryId = 1;
            var category = new Category { Id = categoryId, Name = "Fiction" };
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(category);

            // Act
            var result = await _categoryService.GetByIdAsync(categoryId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(categoryId, result.Id);
            Assert.AreEqual(category.Name, result.Name);
            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(categoryId), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_WhenCategoryDoesNotExist_ReturnsNull()
        {
            // Arrange
            int categoryId = 99;
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync((Category)null);

            // Act
            var result = await _categoryService.GetByIdAsync(categoryId);

            // Assert
            Assert.IsNull(result);
            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(categoryId), Times.Once);
        }

        [Test]
        public async Task CreateAsync_WhenNameIsUnique_CreatesAndReturnsCategory()
        {
            // Arrange
            var createDto = new CreateCategoryDto { Name = "New Category" };
            var createdCategory = new Category { Id = 10, Name = createDto.Name };

            _mockCategoryRepository.Setup(repo => repo.GetByNameAsync(createDto.Name)).ReturnsAsync((Category)null); // Name is unique
            _mockCategoryRepository.Setup(repo => repo.CreateAsync(It.Is<Category>(c => c.Name == createDto.Name))).ReturnsAsync(createdCategory);

            // Act
            var result = await _categoryService.CreateAsync(createDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(createdCategory.Id, result.Id);
            Assert.AreEqual(createdCategory.Name, result.Name);
            _mockCategoryRepository.Verify(repo => repo.CreateAsync(It.Is<Category>(c => c.Name == createDto.Name)), Times.Once);
        }

        [Test]
        public void CreateAsync_WhenNameExists_ThrowsConflictException()
        {
            // Arrange
            var createDto = new CreateCategoryDto { Name = "Existing Category" };
            var existingCategory = new Category { Id = 1, Name = createDto.Name };

            _mockCategoryRepository.Setup(repo => repo.GetByNameAsync(createDto.Name)).ReturnsAsync(existingCategory); // Name exists

            // Act & Assert
            var ex = Assert.ThrowsAsync<ConflictException>(() => _categoryService.CreateAsync(createDto));
            Assert.AreEqual($"Category with name: {createDto.Name} already exists!", ex.Message);
            _mockCategoryRepository.Verify(repo => repo.CreateAsync(It.IsAny<Category>()), Times.Never);
        }

        [Test]
        public async Task UpdateAsync_WhenCategoryExistsAndNameIsUnique_UpdatesAndReturnsCategory()
        {
            // Arrange
            int categoryId = 1;
            var updateDto = new UpdateCategoryDto { Name = "Updated Name" };
            var existingCategory = new Category { Id = categoryId, Name = "Old Name" };
            var updatedCategory = new Category { Id = categoryId, Name = updateDto.Name }; // The state after update

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(existingCategory);
            _mockCategoryRepository.Setup(repo => repo.GetByNameAsync(updateDto.Name)).ReturnsAsync((Category)null); // New name is unique
            _mockCategoryRepository.Setup(repo => repo.UpdateAsync(It.Is<Category>(c => c.Id == categoryId && c.Name == updateDto.Name))).ReturnsAsync(updatedCategory);

            // Act
            var result = await _categoryService.UpdateAsync(categoryId, updateDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(categoryId, result.Id);
            Assert.AreEqual(updateDto.Name, result.Name);
            _mockCategoryRepository.Verify(repo => repo.UpdateAsync(It.Is<Category>(c => c.Id == categoryId)), Times.Once);
        }

        [Test]
        public void UpdateAsync_WhenCategoryNotFound_ThrowsNotFoundException()
        {
            // Arrange
            int categoryId = 99;
            var updateDto = new UpdateCategoryDto { Name = "Any Name" };

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync((Category)null); // Category not found

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() => _categoryService.UpdateAsync(categoryId, updateDto));
            Assert.AreEqual($"There is no category entry with Id: {categoryId}!", ex.Message);
            _mockCategoryRepository.Verify(repo => repo.GetByNameAsync(It.IsAny<string>()), Times.Never);
            _mockCategoryRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Category>()), Times.Never);
        }

        [Test]
        public void UpdateAsync_WhenUpdatedNameConflicts_ThrowsConflictException()
        {
            // Arrange
            int categoryIdToUpdate = 1;
            var updateDto = new UpdateCategoryDto { Name = "Conflicting Name" };
            var categoryToUpdate = new Category { Id = categoryIdToUpdate, Name = "Old Name" };
            var conflictingCategory = new Category { Id = 2, Name = updateDto.Name }; // Another category with the target name

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryIdToUpdate)).ReturnsAsync(categoryToUpdate);
            _mockCategoryRepository.Setup(repo => repo.GetByNameAsync(updateDto.Name)).ReturnsAsync(conflictingCategory); // New name conflicts

            // Act & Assert
            var ex = Assert.ThrowsAsync<ConflictException>(() => _categoryService.UpdateAsync(categoryIdToUpdate, updateDto));
            Assert.AreEqual($"Category with name: {updateDto.Name} already exists!", ex.Message);
            _mockCategoryRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Category>()), Times.Never);
        }

        [Test]
        public async Task DeleteAsync_WhenCategoryExists_ReturnsTrue()
        {
            // Arrange
            int categoryId = 1;
            var existingCategory = new Category { Id = categoryId, Name = "To Be Deleted" };
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(existingCategory);
            _mockCategoryRepository.Setup(repo => repo.DeleteAsync(categoryId)).Returns(Task.CompletedTask);

            // Act
            var result = await _categoryService.DeleteAsync(categoryId);

            // Assert
            Assert.IsTrue(result);
            _mockCategoryRepository.Verify(repo => repo.DeleteAsync(categoryId), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_WhenCategoryDoesNotExist_ReturnsFalse()
        {
            // Arrange
            int categoryId = 99;
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync((Category)null); // Category not found

            // Act
            var result = await _categoryService.DeleteAsync(categoryId);

            // Assert
            Assert.IsFalse(result);
            _mockCategoryRepository.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Never);
        }
    }
}