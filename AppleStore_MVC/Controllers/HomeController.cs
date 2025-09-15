using System.Diagnostics;
using AppleStore_MVC.Data;
using AppleStore_MVC.DataAccess;
using AppleStore_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppleStore_MVC.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CategoryDao _cateDao;
        private readonly ProductDao _productDao;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _cateDao = new CategoryDao(new AppleStoreContext());
            _productDao = new ProductDao(new AppleStoreContext());
        }

        public async Task<IActionResult> Index()
        {
            // Map Category
            var categories = _cateDao.GetAllCategoriesAsync().Result;        

            // Map Product
            var newestProducts = _productDao.GetLast4ProductsAsync().Result;

            // Map Top 4 Best-selling Products
            var topSellingProducts = _productDao.GetTop4BestSellersAsync().Result;
              

            // Map Best-selling Product
            var bestSellerProduct =  _productDao.GetBestSellerAsync().Result;
            
            // Gán vào ViewModel
            var viewModel = new HomeViewModel
            { 
                NewestProducts = newestProducts,
                TopSellingProducts = topSellingProducts,
                BestSellerProduct = bestSellerProduct
            };
            ViewBag.Categories = categories;
            return View(viewModel);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
