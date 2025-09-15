using AppleStore_MVC.Data;
using Microsoft.EntityFrameworkCore;

namespace AppleStore_MVC.DataAccess
{
    public class ProductDao
    {
        private readonly AppleStoreContext _context;

        public ProductDao(AppleStoreContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetLast4ProductsAsync()
        {
            return await _context.Products
                .OrderByDescending(p => p.ProductId)
                .Take(4)
                .ToListAsync();
        }

        public async Task<Product?> GetBestSellerAsync()
        {
            return await _context.Products
                .OrderByDescending(p => p.SoldQuantity)
                .FirstOrDefaultAsync();
        }


        public async Task<List<Product>> GetTop4BestSellersAsync()
        {
            return await _context.Products
                .OrderByDescending(p => p.SoldQuantity)
                .Take(4)
                .ToListAsync();
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<int> CountAllAsync()
        {
            return await _context.Products.CountAsync();
        }

        public async Task<List<Product>> GetListProductByIndexAsync(int indexPage)
        {
            int pageSize = 6;
            int offset = (indexPage - 1) * pageSize;

            return await _context.Products
                                 .OrderBy(p => p.ProductId)
                                 .Skip(offset)
                                 .Take(pageSize)
                                 .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _context.Products
                                 .FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public async Task<int> CountByCategoryIdAsync(int categoryId)
        {
            return await _context.Products
                                 .Where(p => p.CategoryId == categoryId)
                                 .CountAsync();
        }

        public async Task<List<Product>> GetListProductByCategoryIdAsync(int categoryId, int pageIndex)
        {
            int pageSize = 6;
            int offset = (pageIndex - 1) * pageSize;

            return await _context.Products
                                 .Where(p => p.CategoryId == categoryId)
                                 .OrderBy(p => p.ProductId)
                                 .Skip(offset)
                                 .Take(pageSize)
                                 .ToListAsync();
        }

        public async Task<List<Product>> GetListProductByNameAsync(string name, int pageIndex)
        {
            int pageSize = 6;
            int offset = (pageIndex - 1) * pageSize;

            return await _context.Products
                                 .Where(p => p.ProductName.Contains(name))
                                 .OrderBy(p => p.ProductId)
                                 .Skip(offset)
                                 .Take(pageSize)
                                 .ToListAsync();
        }

        public int CountListProductByName(string name)
        {
            return _context.Products
                           .Where(p => p.ProductName.Contains(name))
                           .Count();
        }

    }
}
