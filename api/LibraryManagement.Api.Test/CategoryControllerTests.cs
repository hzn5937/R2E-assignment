using Moq;
using LibraryManagement.Api.Controllers;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.DTOs.Category;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Application.DTOs.Common; 
using LibraryManagement.Application.Extensions.Exceptions;

namespace LibraryManagement.Api.Test
{
    [TestFixture]
    public class CategoryControllerTests
    {
        private Mock<ICategoryService> _mockCategoryService;
        private CategoryController _categoryController;

        [SetUp]
        public void Setup()
        {
            _mockCategoryService = new Mock<ICategoryService>();
            _categoryController = new CategoryController(_mockCategoryService.Object);
        }

        [Test]
        public async Task GetCategories_ReturnsOkResult_WithPaginatedCategories()
        {
            // Arrange
            var paginatedCategories = new PaginatedOutputDto<CategoryOutputDto>
            {
                Items = new List<CategoryOutputDto> { new CategoryOutputDto { Id = 1, Name = "Fiction" } },
                TotalCount = 1,
                PageNum = 1,
                PageSize = 5,
                TotalPage = 1
            };
            _mockCategoryService.Setup(service => service.GetAllAsync(1, 5)).ReturnsAsync(paginatedCategories);

            // Act
            var result = await _categoryController.GetCategories(1, 5);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(paginatedCategories, okResult.Value);
            _mockCategoryService.Verify(s => s.GetAllAsync(1, 5), Times.Once);
        }

        [Test]
        public async Task GetCategoryById_WhenCategoryExists_ReturnsOkResult()
        {
            // Arrange
            int categoryId = 1;
            var categoryDto = new CategoryOutputDto { Id = categoryId, Name = "Fiction" };
            _mockCategoryService.Setup(service => service.GetByIdAsync(categoryId)).ReturnsAsync(categoryDto);

            // Act
            var result = await _categoryController.GetCategoryById(categoryId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(categoryDto, okResult.Value);
            _mockCategoryService.Verify(s => s.GetByIdAsync(categoryId), Times.Once);
        }

        [Test]
        public async Task GetCategoryById_WhenCategoryDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            int categoryId = 99;
            _mockCategoryService.Setup(service => service.GetByIdAsync(categoryId)).ReturnsAsync((CategoryOutputDto)null);

            // Act
            var result = await _categoryController.GetCategoryById(categoryId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual($"Category with ID {categoryId} not found.", notFoundResult.Value);
            _mockCategoryService.Verify(s => s.GetByIdAsync(categoryId), Times.Once);
        }

        [Test]
        public async Task CreateCategory_WhenSuccessful_ReturnsOkResult()
        {
            // Arrange
            var createDto = new CreateCategoryDto { Name = "New Category" };
            var createdDto = new CategoryOutputDto { Id = 10, Name = createDto.Name };
            _mockCategoryService.Setup(service => service.CreateAsync(createDto)).ReturnsAsync(createdDto);

            // Act
            var result = await _categoryController.CreateCategory(createDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(createdDto, okResult.Value);
            _mockCategoryService.Verify(s => s.CreateAsync(createDto), Times.Once);
        }

        [Test]
        public void CreateCategory_WhenServiceThrowsConflict_ThrowsConflictException()
        {
            // Arrange
            var createDto = new CreateCategoryDto { Name = "Existing" };
            _mockCategoryService.Setup(service => service.CreateAsync(createDto)).ThrowsAsync(new ConflictException("Category already exists"));

            // Act & Assert
            Assert.ThrowsAsync<ConflictException>(async () => await _categoryController.CreateCategory(createDto));
            _mockCategoryService.Verify(s => s.CreateAsync(createDto), Times.Once);
        }

        [Test]
        public async Task UpdateCategory_WhenSuccessful_ReturnsOkResult()
        {
            // Arrange
            int categoryId = 1;
            var updateDto = new UpdateCategoryDto { Name = "Updated Name" };
            var updatedDto = new CategoryOutputDto { Id = categoryId, Name = updateDto.Name };
            _mockCategoryService.Setup(service => service.UpdateAsync(categoryId, updateDto)).ReturnsAsync(updatedDto);

            // Act
            var result = await _categoryController.UpdateCategory(categoryId, updateDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(updatedDto, okResult.Value);
            _mockCategoryService.Verify(s => s.UpdateAsync(categoryId, updateDto), Times.Once);
        }

        [Test]
        public void UpdateCategory_WhenServiceThrowsNotFound_ThrowsNotFoundException()
        {
            // Arrange
            int categoryId = 99;
            var updateDto = new UpdateCategoryDto { Name = "Update Name" };
            var expectedMessage = $"Category with ID {categoryId} not found.";
            _mockCategoryService.Setup(service => service.UpdateAsync(categoryId, updateDto)).ThrowsAsync(new NotFoundException(expectedMessage));

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(async () => await _categoryController.UpdateCategory(categoryId, updateDto));
            Assert.AreEqual(expectedMessage, ex.Message);
            _mockCategoryService.Verify(s => s.UpdateAsync(categoryId, updateDto), Times.Once);
        }

        [Test]
        public void UpdateCategory_WhenServiceThrowsConflict_ThrowsConflictException()
        {
            // Arrange
            int categoryId = 1;
            var updateDto = new UpdateCategoryDto { Name = "Existing Name" };
            _mockCategoryService.Setup(service => service.UpdateAsync(categoryId, updateDto))
                                .ThrowsAsync(new ConflictException("Name conflict"));

            // Act & Assert
            Assert.ThrowsAsync<ConflictException>(async () => await _categoryController.UpdateCategory(categoryId, updateDto));
            _mockCategoryService.Verify(s => s.UpdateAsync(categoryId, updateDto), Times.Once);
        }

        [Test]
        public async Task DeleteCategory_WhenSuccessful_ReturnsNoContentResult()
        {
            // Arrange
            int categoryId = 1;
            _mockCategoryService.Setup(service => service.DeleteAsync(categoryId)).ReturnsAsync(true);

            // Act
            var result = await _categoryController.DeleteCategory(categoryId);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
            _mockCategoryService.Verify(s => s.DeleteAsync(categoryId), Times.Once);
        }

        [Test]
        public async Task DeleteCategory_WhenCategoryNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            int categoryId = 99;
            _mockCategoryService.Setup(service => service.DeleteAsync(categoryId)).ReturnsAsync(false); // Service indicates not found

            // Act
            var result = await _categoryController.DeleteCategory(categoryId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual($"Category with ID {categoryId} not found.", notFoundResult.Value);
            _mockCategoryService.Verify(s => s.DeleteAsync(categoryId), Times.Once);
        }

        [Test]
        public async Task UpdateCategory_ServiceReturnsNull_ReturnsNotFoundResult()
        {
            // Arrange
            var categoryId = 99; 
            var updateDto = new UpdateCategoryDto { Name = "NonExistent Category Update" };

            _mockCategoryService.Setup(s => s.UpdateAsync(categoryId, It.IsAny<UpdateCategoryDto>()))
                .ReturnsAsync((CategoryOutputDto)null); 

            // Act
            var result = await _categoryController.UpdateCategory(categoryId, updateDto);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual($"Category with ID {categoryId} not found.", notFoundResult.Value);
            _mockCategoryService.Verify(s => s.UpdateAsync(categoryId, updateDto), Times.Once);
        }
    }
}