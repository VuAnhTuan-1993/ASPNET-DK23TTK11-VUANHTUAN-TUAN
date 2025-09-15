using AppleStore_MVC.Data;
using AppleStore_MVC.DataAccess;
using AppleStore_MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppleStore_MVC.Controllers
{
    public class ProductController : BaseController
    {
        private readonly CategoryDao _cateDao;
        private readonly ProductDao _productDao;

        public ProductController(CategoryDao cateDao, ProductDao productDao)
        {
            _cateDao = cateDao;
            _productDao = productDao;
        }
        public IActionResult Index()
        {
            var categories = _cateDao.GetAllCategoriesAsync().Result;
            List<Data.Product> products = new List<Data.Product>();
            var bestSellerProduct = _productDao.GetBestSellerAsync().Result;

            int count = _productDao.CountAllAsync().Result;
            int endPage = count / 6;
            if (count % 6 != 0)
                endPage++;

            string cid = HttpContext.Request.Query["cid"];
            string page = HttpContext.Request.Query["page"];
            string name = HttpContext.Request.Query["name"];

            int cidValue = 0;
            int pageValue = 1;


            if (!string.IsNullOrEmpty(cid) && int.TryParse(cid, out int parsedCid))
                cidValue = parsedCid;

            if (!string.IsNullOrEmpty(page) && int.TryParse(page, out int parsedPage))
                pageValue = parsedPage;

            if (!string.Equals(name?.Trim(), "none", StringComparison.OrdinalIgnoreCase))
            {
                products = _productDao.GetListProductByNameAsync(name, pageValue).Result;
                count = _productDao.CountListProductByName(name);
                endPage = count / 6;
                Console.WriteLine($"[DEBUG] count = {count}");
                if (count % 6 != 0)
                    endPage++;
            }
            else if (cidValue == 0)
                products = _productDao.GetListProductByIndexAsync(pageValue).Result;

            else
            {
                count = _productDao.CountByCategoryIdAsync(cidValue).Result;
                endPage = count / 6;
                if (count % 6 != 0)
                    endPage++;
                products = _productDao.GetListProductByCategoryIdAsync(cidValue, pageValue).Result;
            }    

                // Gán vào ViewModel
                var viewModel = new ProductViewModel
                {
                    Products = products,
                    BestSellerProduct = bestSellerProduct,
                    EndPage = endPage,
                    CurrentPage = pageValue,
                    Cid = cidValue,
                    name = name != null ? name : "none"
                };

            ViewBag.Categories = categories;
            return View(viewModel);
        }
    }
}
