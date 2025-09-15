using Microsoft.AspNetCore.Mvc;
using AppleStore_MVC.Helper;
using AppleStore_MVC.Data;
using AppleStore_MVC.DataAccess;
using AppleStore_MVC.Models;


namespace AppleStore_MVC.Controllers
{
    public class CartController : BaseController
    {
        const string CART_KEY = "MYCART";
        public List<CartItemViewModel> Cart => HttpContext.Session.Get <List<CartItemViewModel>>(
            CART_KEY) ?? new List<CartItemViewModel>();
        private readonly ProductDao _productDao;
        private readonly CategoryDao _categoryDao;
        public CartController(CategoryDao category, ProductDao productDao)
        {
            _categoryDao = category;
            _productDao = productDao;
        }
        public IActionResult Index()
        {
            ViewBag.totalAll = Cart.Sum(x => x.totalPrice);
            ViewBag.Categories = _categoryDao.GetAllCategoriesAsync().Result;
            return View(Cart);
        }

        public IActionResult AddToCart()
        {
            int id = int.Parse(HttpContext.Request.Query["productId"]);
            int quantity = int.Parse(HttpContext.Request.Query["amount"]);
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(x => x.productId == id);
            if (item == null)
            {
                var hangHoa = _productDao.GetProductByIdAsync(id).Result;
                if (hangHoa != null)
                {
                    item = new CartItemViewModel
                    {
                        productId = hangHoa.ProductId,
                        imageLink = hangHoa.ImageLink,
                        productName = hangHoa.ProductName,
                        price = hangHoa.Price,
                        amount = quantity,
                    };
                    gioHang.Add(item);
                }
            }
            else
            {
                item.amount += quantity;
            }
            HttpContext.Session.Set(CART_KEY, gioHang);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult UpdateAmount(int productId, int amount)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(x => x.productId == productId);
            if (item != null)
            {
                item.amount = amount;
                if (item.amount <= 0)
                {
                    gioHang.Remove(item);
                }
            }
            HttpContext.Session.Set(CART_KEY, gioHang);

            return Json(new { success = true, message = "Cập nhật thành công" });
        }

        public IActionResult DeleteItem(int productId)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(x => x.productId == productId);
            if (item != null)
            {
                gioHang.Remove(item);
            }
            HttpContext.Session.Set(CART_KEY, gioHang);
            return Json(new { success = true, message = "Xóa thành công" });
        }

    }
}
