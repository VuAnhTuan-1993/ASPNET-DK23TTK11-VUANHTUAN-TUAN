using System.Reflection.Metadata;
using AppleStore_MVC.Data;
using AppleStore_MVC.DataAccess;
using AppleStore_MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppleStore_MVC.Controllers
{
    public class ProductDetailController : BaseController
    {
        private readonly CategoryDao _cateDao;
        private readonly ProductDao _productDao;
        public ProductDetailController(CategoryDao cateDao, ProductDao productDao)
        {
            _cateDao = cateDao;
            _productDao = productDao;
        }
        public IActionResult Index()
        {
            var categories = _cateDao.GetAllCategoriesAsync().Result;
            String productId = HttpContext.Request.Query["productId"];
            var product = _productDao.GetProductByIdAsync(int.Parse(productId)).Result;

            var viewModel = new ProductDetailViewModel
            {
                Product = product
            };
            ViewBag.Categories = categories;
            return View(viewModel);
        }
    }
}
