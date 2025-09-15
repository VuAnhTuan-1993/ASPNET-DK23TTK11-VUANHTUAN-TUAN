using AppleStore_MVC.Data;
using AppleStore_MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppleStore_MVC.Areas.Dashboard.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppleStoreContext _context;

        public HomeController(AppleStoreContext context)
        {
            _context = context;
        }
        [Area("Dashboard")]
        public IActionResult Index()
        {
            var vm = new DashboardViewModel();

            // Doanh thu tháng này
            vm.RevenueMonth = _context.CartItem
                .Where(ci => ci.Cart.buy_date.Month == DateTime.Now.Month &&
                             ci.Cart.buy_date.Year == DateTime.Now.Year)
                .Sum(ci => (decimal)(ci.quantity * ci.unitPrice));

            // Doanh thu năm nay
            vm.RevenueYear = _context.CartItem
                .Where(ci => ci.Cart.buy_date.Year == DateTime.Now.Year)
                .Sum(ci => (decimal)(ci.quantity * ci.unitPrice));

            // Tổng số đơn hàng
            vm.TotalOrders = _context.Cart.Count();

            // Tổng số người dùng
            vm.TotalUsers = _context.Users.Count();

            // Doanh thu theo từng tháng (biểu đồ line)
            for (int m = 1; m <= 12; m++)
            {
                vm.RevenueMonths.Add($"Tháng {m}");
                var total = _context.CartItem
                    .Where(ci => ci.Cart.buy_date.Month == m &&
                                 ci.Cart.buy_date.Year == DateTime.Now.Year)
                    .Sum(ci => (decimal)(ci.quantity * ci.unitPrice));
                vm.RevenueValues.Add(total);
            }

            // Top 5 sản phẩm bán chạy
            var topProducts = _context.Products
                .OrderByDescending(p => p.SoldQuantity)
                .Take(5)
                .ToList();

            foreach (var p in topProducts)
            {
                vm.TopProductNames.Add(p.ProductName);
                vm.TopProductSales.Add(p.SoldQuantity);
            }

            vm.LowStockProducts = _context.Products
                .Where(p => p.Amount < 10)
                .ToDictionary(p => p.ProductName, p => p.Amount);

            return View(vm);
        }


    }
}