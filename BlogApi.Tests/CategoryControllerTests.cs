using BlogApi.Controllers;
using BlogApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xunit;

namespace BlogApi.Tests
{
    public class CategoryControllerTests : IDisposable
    {
        private readonly BlogContext _context;
        private readonly CategoryController _controller;

        public CategoryControllerTests()
        {
            var options = new DbContextOptionsBuilder<BlogContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            _context = new BlogContext(options);
            _controller = new CategoryController(_context);
            ResetDatabase();
        }

        public void Dispose()
        {
            ResetDatabase();
            _context.Dispose();
        }

        private void ResetDatabase()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetAllCategories_ReturnsAllCategories()
        {
            // Réinitialiser la base de données
            ResetDatabase();

            // Arrange
            var category1 = new Category { Title = "Category1" };
            var category2 = new Category { Title = "Category2" };
            _context.Categories.AddRange(category1, category2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetAllCategories();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Category>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var categories = Assert.IsType<List<Category>>(okResult.Value);
            Assert.Equal(2, categories.Count);
        }

        [Fact]
        public async Task GetCategoryById_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            // Réinitialiser la base de données
            ResetDatabase();

            // Act
            var result = await _controller.GetCategoryById(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetCategoryById_ReturnsCategory_WhenCategoryExists()
        {
            // Réinitialiser la base de données
            ResetDatabase();

            // Arrange
            var category = new Category { Title = "Category1" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetCategoryById(category.Id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Category>>(result);
            var returnValue = Assert.IsType<OkObjectResult>(actionResult.Result);
            var categoryResult = Assert.IsType<Category>(returnValue.Value);
            Assert.Equal(category.Id, categoryResult.Id);
        }

        [Fact]
        public async Task CreateCategory_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Réinitialiser la base de données
            ResetDatabase();

            // Arrange
            var category = new Category { Title = "" };
            _controller.ModelState.AddModelError("Title", "Required");

            // Act
            var result = await _controller.CreateCategory(category);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var error = Assert.IsType<SerializableError>(badRequestResult.Value);
            Assert.Equal("Required", ((string[])error["Title"])[0]);
        }

        [Fact]
        public async Task CreateCategory_ReturnsCreatedAtAction_WhenCategoryIsCreated()
        {
            // Réinitialiser la base de données
            ResetDatabase();

            // Arrange
            var category = new Category { Title = "Category3" };

            // Act
            var result = await _controller.CreateCategory(category);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Category>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<Category>(createdAtActionResult.Value);
            Assert.Equal(category.Title, returnValue.Title);
            Assert.Equal(nameof(_controller.GetCategoryById), createdAtActionResult.ActionName);
            Assert.Equal(category.Id, ((Category)createdAtActionResult.Value).Id);
        }

        [Fact]
        public async Task UpdateCategory_ReturnsBadRequest_WhenCategoryIdMismatch()
        {
            // Réinitialiser la base de données
            ResetDatabase();

            // Arrange
            var category = new Category { Id = 1, Title = "Category1" };

            // Act
            var result = await _controller.UpdateCategory(2, category);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task UpdateCategory_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            // Réinitialiser la base de données
            ResetDatabase();

            // Arrange
            var category = new Category { Id = 1, Title = "Category1" };

            // Act
            var result = await _controller.UpdateCategory(1, category);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteCategoryById_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            // Réinitialiser la base de données
            ResetDatabase();

            // Act
            var result = await _controller.DeleteCategoryById(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteCategoryById_DeletesCategory_WhenCategoryExists()
        {
            // Réinitialiser la base de données
            ResetDatabase();

            // Arrange
            var category = new Category { Title = "Category1" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteCategoryById(category.Id);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Verify category is deleted
            var deletedCategory = await _context.Categories.FindAsync(category.Id);
            Assert.Null(deletedCategory);
        }

        [Fact]
        public void CreateCategory_ValidationChecks()
        {
            // Arrange
            var category = new Category();
            var validationContext = new ValidationContext(category, null, null);
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(category, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, v => v.MemberNames != null && v.MemberNames.Contains("Title") && v.ErrorMessage != null && v.ErrorMessage.Contains("Title is required"));
        }
    }
}
