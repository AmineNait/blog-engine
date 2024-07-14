using BlogAdmin.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace BlogAdmin.Tests.Setup
{
    public static class TestDbContextFactory
    {
        public static BlogContext Create()
        {
            var options = new DbContextOptionsBuilder<BlogContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new BlogContext(options);

            context.Database.EnsureCreated();

            return context;
        }

        public static void Destroy(BlogContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
