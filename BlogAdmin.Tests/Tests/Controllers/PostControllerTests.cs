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
    public class PostControllerTests
    {
        private readonly BlogContext _context;
        private readonly PostController _controller;

        public PostControllerTests()
        {
            _context = TestDbContextFactory.Create();
            var loggerMock = new Mock<ILogger<PostController>>();
            _controller = new PostController(_context, loggerMock.Object);
        }

        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfPosts()
        {
            // Arrange
            var category = new Category { Title = "Category1" };
            _context.Categories.Add(category);
            _context.Posts.Add(new Post { Title = "Post1", CategoryId = category.Id, PublicationDate = System.DateTime.Now, Content = "Content1" });
            _context.Posts.Add(new Post { Title = "Post2", CategoryId = category.Id, PublicationDate = System.DateTime.Now, Content = "Content2" });
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Post>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task Create_Post_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var category = new Category { Title = "Category1" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            var post = new Post { Title = "New Post", CategoryId = category.Id, PublicationDate = System.DateTime.Now, Content = "Content" };

            // Act
            var result = await _controller.Create(post);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal(1, await _context.Posts.CountAsync());
        }

        [Fact]
        public async Task Create_Post_InvalidModel_ReturnsView()
        {
            // Arrange
            var post = new Post { Title = "", CategoryId = 1, PublicationDate = System.DateTime.Now, Content = "" };
            _controller.ModelState.AddModelError("Title", "Required");

            // Act
            var result = await _controller.Create(post);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Post>(viewResult.ViewData.Model);
            Assert.Equal(post, model);
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
            var post = new Post { Title = "Post1", CategoryId = category.Id, PublicationDate = System.DateTime.Now, Content = "Content1" };
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            post.Title = "Updated Post";

            // Act
            var result = await _controller.Edit(post.Id, post);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            var updatedPost = await _context.Posts.FindAsync(post.Id);
            Assert.NotNull(updatedPost);
            Assert.Equal("Updated Post", updatedPost?.Title);
        }

        [Fact]
        public async Task Edit_Post_InvalidModel_ReturnsView()
        {
            // Arrange
            var category = new Category { Title = "Category1" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            var post = new Post { Title = "Post1", CategoryId = category.Id, PublicationDate = System.DateTime.Now, Content = "Content1" };
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            _controller.ModelState.AddModelError("Title", "Required");
            post.Title = "";

            // Act
            var result = await _controller.Edit(post.Id, post);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Post>(viewResult.ViewData.Model);
            Assert.Equal(post, model);
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
        public async Task Delete_Get_ReturnsViewResult_WithPost()
        {
            // Arrange
            var category = new Category { Title = "Category1" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            var post = new Post { Title = "Post1", CategoryId = category.Id, PublicationDate = System.DateTime.Now, Content = "Content1" };
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Delete(post.Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Post>(viewResult.ViewData.Model);
            Assert.Equal(post.Id, model.Id);
        }

        [Fact]
        public async Task DeleteConfirmed_Post_DeletesPost_AndRedirectsToIndex()
        {
            // Arrange
            var category = new Category { Title = "Category1" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            var post = new Post { Title = "Post1", CategoryId = category.Id, PublicationDate = System.DateTime.Now, Content = "Content1" };
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteConfirmed(post.Id);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal(0, await _context.Posts.CountAsync());
        }
    }
}
