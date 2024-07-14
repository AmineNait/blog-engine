using BlogAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BlogAdmin.Controllers
{
    public class HomeController : Controller
    {
        private readonly BlogContext _context;

        public HomeController(BlogContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeViewModel
            {
                Categories = await _context.Categories.ToListAsync(),
                Posts = await _context.Posts.Include(p => p.Category).ToListAsync()
            };
            return View(viewModel);
        }
    }
}
