using AppleStore_MVC.Data;
using Microsoft.EntityFrameworkCore;

namespace AppleStore_MVC.DataAccess
{
    public class CategoryDao
    {
        private readonly AppleStoreContext _context;
        public CategoryDao(AppleStoreContext context)
        {
            _context = context;
        }
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

    }
}
