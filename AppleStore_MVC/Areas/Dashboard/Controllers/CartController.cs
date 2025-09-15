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
    public class CartController : Controller
    {
        private readonly AppleStoreContext _context;

        public CartController(AppleStoreContext context)
        {
            _context = context;
        }

        // GET: Dashboard/Cart
        public async Task<IActionResult> Index()
        {
            var appleStoreContext = _context.Cart.Include(c => c.User_Of_Cart);
            return View(await appleStoreContext.ToListAsync());
        }

        // GET: Dashboard/Cart/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .Include(c => c.User_Of_Cart)
                .FirstOrDefaultAsync(m => m.id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Dashboard/Cart/Create
        public IActionResult Create()
        {
            ViewData["u_id"] = new SelectList(_context.Users, "UserId", "UserName");
            return View();
        }

        // POST: Dashboard/Cart/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cart cart)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(cart);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["u_id"] = new SelectList(_context.Users, "UserId", "UserId", cart.u_id);
            //return View(cart);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(cart);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    // Lỗi EF Core
                    Console.WriteLine("DbUpdateException: " + ex.Message);

                    // Log inner exception chi tiết (SQL Server báo lỗi cụ thể)
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                    }

                    // Bạn cũng có thể hiển thị ra ViewBag để debug
                    ViewBag.ErrorMessage = ex.InnerException?.Message;
                }
            }

            ViewData["u_id"] = new SelectList(_context.Users, "UserId", "UserId", cart.u_id);
            return View(cart);
        }



        // GET: Dashboard/Cart/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["u_id"] = new SelectList(_context.Users, "UserId", "UserName", cart.u_id);
            return View(cart);
        }

        // POST: Dashboard/Cart/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cart cart)
        {
            if (id != cart.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.id))
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
            ViewData["u_id"] = new SelectList(_context.Users, "UserId", "UserName", cart.u_id);
            return View(cart);
        }

        // GET: Dashboard/Cart/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .Include(c => c.User_Of_Cart)
                .FirstOrDefaultAsync(m => m.id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Dashboard/Cart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var cart = await _context.Cart.FindAsync(id);
            //if (cart != null)
            //{
            //    _context.Cart.Remove(cart);
            //}

            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));

            try
            {              
                var cart = _context.Cart.Find(id);
                _context.Cart.Remove(cart);
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

        private bool CartExists(int id)
        {
            return _context.Cart.Any(e => e.id == id);
        }
    }
}
