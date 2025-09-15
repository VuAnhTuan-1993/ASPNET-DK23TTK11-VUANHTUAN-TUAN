using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppleStore_MVC.Data;

namespace AppleStore_MVC.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class CartItemController : Controller
    {
        private readonly AppleStoreContext _context;

        public CartItemController(AppleStoreContext context)
        {
            _context = context;
        }

        // GET: Dashboard/CartItem
        public async Task<IActionResult> Index()
        {
            var appleStoreContext = _context.CartItem.Include(c => c.Cart).Include(c => c.Product);
            return View(await appleStoreContext.ToListAsync());
        }

        // GET: Dashboard/CartItem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItem
                .Include(c => c.Cart)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.CartItemId == id);
            if (cartItem == null)
            {
                return NotFound();
            }

            return View(cartItem);
        }

        // GET: Dashboard/CartItem/Create
        public IActionResult Create()
        {
            ViewData["cart_id"] = new SelectList(_context.Cart, "id", "id");
            ViewData["pro_id"] = new SelectList(_context.Products, "ProductId", "ProductName");
            return View();
        }

        // POST: Dashboard/CartItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("cart_id,CartItemId,quantity,unitPrice,pro_id")] CartItem cartItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cartItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["cart_id"] = new SelectList(_context.Cart, "id", "id", cartItem.cart_id);
            ViewData["pro_id"] = new SelectList(_context.Products, "ProductId", "ProductName", cartItem.pro_id);
            return View(cartItem);
        }

        // GET: Dashboard/CartItem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItem.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }
            ViewData["cart_id"] = new SelectList(_context.Cart, "id", "id", cartItem.cart_id);
            ViewData["pro_id"] = new SelectList(_context.Products, "ProductId", "ProductName", cartItem.pro_id);
            return View(cartItem);
        }

        // POST: Dashboard/CartItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("cart_id,CartItemId,quantity,unitPrice,pro_id")] CartItem cartItem)
        {
            if (id != cartItem.CartItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cartItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartItemExists(cartItem.CartItemId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["cart_id"] = new SelectList(_context.Cart, "id", "id", cartItem.cart_id);
            ViewData["pro_id"] = new SelectList(_context.Products, "ProductId", "ProductName", cartItem.pro_id);
            return View(cartItem);
        }

        // GET: Dashboard/CartItem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItem
                .Include(c => c.Cart)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.CartItemId == id);
            if (cartItem == null)
            {
                return NotFound();
            }

            return View(cartItem);
        }

        // POST: Dashboard/CartItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var cartItem = await _context.CartItem.FindAsync(id);
            //if (cartItem != null)
            //{
            //    _context.CartItem.Remove(cartItem);
            //}

            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));

            try
            {              
                var cartItem = _context.CartItem.Find(id);
                _context.CartItem.Remove(cartItem);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                // Kiểm tra xem có phải lỗi khóa ngoại không
                if (ex.InnerException != null && ex.InnerException.Message.Contains("REFERENCE"))
                {
                    TempData["ErrorMessage"] = "Không thể xóa vì dữ liệu đang được sử dụng ở bảng khác.";
                }
                else if (ex.InnerException != null && ex.InnerException.Message.Contains("PRIMARY"))
                {
                    TempData["ErrorMessage"] = "ID này đã tồn tại. Vui lòng nhập ID khác.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Lỗi khi xử lý dữ liệu.";
                }
                return RedirectToAction(nameof(Index));
            }
        }

        private bool CartItemExists(int id)
        {
            return _context.CartItem.Any(e => e.CartItemId == id);
        }
    }
}
