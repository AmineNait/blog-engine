using BlogAdmin.Controllers;
using BlogAdmin.Models;
using BlogAdmin.Tests.Setup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BlogAdmin.Tests.Views
{
    public class CategoryViewTests
    {
        private readonly BlogContext _context;
        private readonly CategoryController _controller;

        public CategoryViewTests()
        {
            _context = TestDbContextFactory.Create();
            var loggerMock = new Mock<ILogger<CategoryController>>();
            _controller = new CategoryController(_context, loggerMock.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithCategoryList()
        {
            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<List<Category>>(viewResult.ViewData.Model);
            await Task.CompletedTask; // Ajout de cette ligne pour éviter l'avertissement
        }

        [Fact]
        public async Task Create_ReturnsViewResult()
        {
            // Act
            var result = _controller.Create();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            await Task.CompletedTask; // Ajout de cette ligne pour éviter l'avertissement
        }

        [Fact]
        public async Task Edit_ReturnsViewResult_WithCategory()
        {
            // Arrange
            var category = new Category { Title = "Category1" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Edit(category.Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<Category>(viewResult.ViewData.Model);
            await Task.CompletedTask; // Ajout de cette ligne pour éviter l'avertissement
        }

        [Fact]
        public async Task Delete_ReturnsViewResult_WithCategory()
        {
            // Arrange
            var category = new Category { Title = "Category1" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Delete(category.Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<Category>(viewResult.ViewData.Model);
            await Task.CompletedTask; // Ajout de cette ligne pour éviter l'avertissement
        }
    }
}
