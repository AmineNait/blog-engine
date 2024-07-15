using BlogAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BlogAdmin.Controllers
{
    public class HomeController : Controller
    {
        private readonly BlogContext _context;

        // Constructor to initialize the BlogContext
        public HomeController(BlogContext context)
        {
            _context = context;
        }

        // Action to render the home page with categories and posts
        public async Task<IActionResult> Index()
        {
            // Creating a view model with categories and posts
            var viewModel = new HomeViewModel
            {
                Categories = await _context.Categories.ToListAsync(),
                Posts = await _context.Posts.Include(p => p.Category).ToListAsync()
            };

            // Passing the view model to the view
            return View(viewModel);
        }
    }
}
