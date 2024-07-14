using BlogAdmin.Controllers;
using BlogAdmin.Models;
using BlogAdmin.Tests.Setup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BlogAdmin.Tests.Controllers
{
    public class CategoryControllerTests
    {
        private readonly BlogContext _context;
        private readonly CategoryController _controller;

        public CategoryControllerTests()
        {
            _context = TestDbContextFactory.Create();
            var loggerMock = new Mock<ILogger<CategoryController>>();
            _controller = new CategoryController(_context, loggerMock.Object);
        }

        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfCategories()
        {
            // Arrange
            _context.Categories.Add(new Category { Title = "Category1" });
            _context.Categories.Add(new Category { Title = "Category2" });
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Category>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task Create_Post_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var category = new Category { Title = "New Category" };

            // Act
            var result = await _controller.Create(category);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal(1, await _context.Categories.CountAsync());
        }

        [Fact]
        public async Task Create_Post_InvalidModel_ReturnsView()
        {
            // Arrange
            var category = new Category { Title = "" };
            _controller.ModelState.AddModelError("Title", "Required");

            // Act
            var result = await _controller.Create(category);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Category>(viewResult.ViewData.Model);
            Assert.Equal(category, model);
        }

        [Fact]
        public async Task Edit_Get_ReturnsNotFound_ForInvalidId()
        {
            // Act
            var result = await _controller.Edit(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Post_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var category = new Category { Title = "Category1" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            category.Title = "Updated Category";

            // Act
            var result = await _controller.Edit(category.Id, category);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            var updatedCategory = await _context.Categories.FindAsync(category.Id);
            Assert.Equal("Updated Category", updatedCategory?.Title);
        }

        [Fact]
        public async Task Edit_Post_InvalidModel_ReturnsView()
        {
            // Arrange
            var category = new Category { Title = "Category1" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            _controller.ModelState.AddModelError("Title", "Required");
            category.Title = "";

            // Act
            var result = await _controller.Edit(category.Id, category);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Category>(viewResult.ViewData.Model);
            Assert.Equal(category, model);
        }

        [Fact]
        public async Task Delete_Get_ReturnsNotFound_ForInvalidId()
        {
            // Act
            var result = await _controller.Delete(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Get_ReturnsViewResult_WithCategory()
        {
            // Arrange
            var category = new Category { Title = "Category1" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Delete(category.Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Category>(viewResult.ViewData.Model);
            Assert.Equal(category.Id, model.Id);
        }

        [Fact]
        public async Task DeleteConfirmed_Post_DeletesCategory_AndRedirectsToIndex()
        {
            // Arrange
            var category = new Category { Title = "Category1" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteConfirmed(category.Id);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal(0, await _context.Categories.CountAsync());
        }
    }
}
