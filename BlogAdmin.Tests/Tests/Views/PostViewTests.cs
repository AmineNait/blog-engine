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
    public class PostViewTests
    {
        private readonly BlogContext _context;
        private readonly PostController _controller;

        public PostViewTests()
        {
            _context = TestDbContextFactory.Create();
            var loggerMock = new Mock<ILogger<PostController>>();
            _controller = new PostController(_context, loggerMock.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithPostList()
        {
            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<List<Post>>(viewResult.ViewData.Model);
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
        public async Task Edit_ReturnsViewResult_WithPost()
        {
            // Arrange
            var category = new Category { Title = "Category1" };
            _context.Categories.Add(category);
            var post = new Post { Title = "Post1", CategoryId = category.Id, PublicationDate = System.DateTime.Now, Content = "Content1" };
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Edit(post.Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<Post>(viewResult.ViewData.Model);
            await Task.CompletedTask; // Ajout de cette ligne pour éviter l'avertissement
        }

        [Fact]
        public async Task Delete_ReturnsViewResult_WithPost()
        {
            // Arrange
            var category = new Category { Title = "Category1" };
            _context.Categories.Add(category);
            var post = new Post { Title = "Post1", CategoryId = category.Id, PublicationDate = System.DateTime.Now, Content = "Content1" };
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Delete(post.Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<Post>(viewResult.ViewData.Model);
            await Task.CompletedTask; // Ajout de cette ligne pour éviter l'avertissement
        }
    }
}
