using BlogApi.Controllers;
using BlogApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xunit;

namespace BlogApi.Tests
{
    public class PostControllerTests : IDisposable
    {
        private readonly BlogContext _context;
        private readonly PostController _controller;

        public PostControllerTests()
        {
            var options = new DbContextOptionsBuilder<BlogContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new BlogContext(options);
            _controller = new PostController(_context);
            ResetDatabase();
        }

        public void Dispose()
        {
            ResetDatabase();
            _context.Dispose();
        }

        private void ResetDatabase()
        {
            _context.Posts.RemoveRange(_context.Posts);
            _context.Categories.RemoveRange(_context.Categories);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetPostById_ReturnsNotFound_WhenPostDoesNotExist()
        {
            // Act
            var result = await _controller.GetPostById(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreatePost_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var post = new Post { Title = "", CategoryId = 1, PublicationDate = System.DateTime.Now, Content = "" };
            _controller.ModelState.AddModelError("Title", "Required");

            // Act
            var result = await _controller.CreatePost(post);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var error = Assert.IsType<SerializableError>(badRequestResult.Value);
            Assert.Equal("Required", ((string[])error["Title"])[0]);
        }

        [Fact]
        public async Task CreatePost_ReturnsCreatedAtAction_WhenPostIsCreated()
        {
            // Arrange
            var category = new Category { Title = "Category1" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var post = new Post { Title = "Post1", CategoryId = category.Id, PublicationDate = System.DateTime.Now, Content = "Content1" };

            // Act
            var result = await _controller.CreatePost(post);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Post>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<Post>(createdAtActionResult.Value);
            Assert.Equal(post.Title, returnValue.Title);
        }

        [Fact]
        public async Task UpdatePost_ReturnsBadRequest_WhenPostIdMismatch()
        {
            // Arrange
            var post = new Post { Id = 1, Title = "Post1", CategoryId = 1, PublicationDate = System.DateTime.Now, Content = "Content1" };

            // Act
            var result = await _controller.UpdatePost(2, post);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task UpdatePost_ReturnsNotFound_WhenPostDoesNotExist()
        {
            // Arrange
            var post = new Post { Id = 1, Title = "Post1", CategoryId = 1, PublicationDate = System.DateTime.Now, Content = "Content1" };

            // Act
            var result = await _controller.UpdatePost(1, post);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeletePost_ReturnsNotFound_WhenPostDoesNotExist()
        {
            // Act
            var result = await _controller.DeletePost(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeletePost_DeletesPost_WhenPostExists()
        {
            // Arrange
            var category = new Category { Title = "Category1" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var post = new Post { Title = "Post1", CategoryId = category.Id, PublicationDate = System.DateTime.Now, Content = "Content1" };
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeletePost(post.Id);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Verify post is deleted
            var deletedPost = await _context.Posts.FindAsync(post.Id);
            Assert.Null(deletedPost);
        }

        [Fact]
        public void CreatePost_ValidationChecks()
        {
            // Arrange
            var post = new Post();
            var validationContext = new ValidationContext(post, null, null);
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(post, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, v => v.MemberNames != null && v.MemberNames.Contains("Title") && v.ErrorMessage != null && v.ErrorMessage.Contains("Title is required"));
            Assert.Contains(validationResults, v => v.MemberNames != null && v.MemberNames.Contains("Content") && v.ErrorMessage != null && v.ErrorMessage.Contains("Content is required"));
        }
    }
}
