using BlogAdmin.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace BlogAdmin.Tests.Setup
{
    public static class TestDbContextFactory
    {
        // Create a new instance of BlogContext with an in-memory database
        public static BlogContext Create()
        {
            var options = new DbContextOptionsBuilder<BlogContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new BlogContext(options);
            context.Database.EnsureCreated();

            return context;
        }

        // Clean up and delete the in-memory database
        public static void Destroy(BlogContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
