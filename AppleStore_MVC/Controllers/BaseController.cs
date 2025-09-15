using AppleStore_MVC.Data;
using AppleStore_MVC.Helper;
using AppleStore_MVC.Models; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AppleStore_MVC.Controllers
{
    public class BaseController : Controller
    {
        protected const string CART_KEY = "MYCART";

        protected List<CartItemViewModel> Cart => HttpContext.Session.Get<List<CartItemViewModel>>(
            CART_KEY) ?? new List<CartItemViewModel>();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Tính tổng số lượng sản phẩm trong cart  
            ViewBag.CartCount = Cart.Count;

            base.OnActionExecuting(filterContext);
        }
    }
}
