using Microsoft.AspNetCore.Mvc;
using AppleStore_MVC.Data;
using Microsoft.EntityFrameworkCore;

namespace AppleStore_MVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppleStoreContext _context;

        public CategoryController(AppleStoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }
    }
}
