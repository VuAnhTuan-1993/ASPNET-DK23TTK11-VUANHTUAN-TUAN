using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppleStore_MVC.Data;
using AppleStore_MVC.Models;

namespace AppleStore_MVC.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class ProductController : Controller
    {
        private readonly AppleStoreContext _context;

        public ProductController(AppleStoreContext context)
        {
            _context = context;
        }

        // GET: Dashboard/Product
        public async Task<IActionResult> Index()
        {
            var appleStoreContext = _context.Products.Include(p => p.Category);
            return View(await appleStoreContext.ToListAsync());
        }

        // GET: Dashboard/Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Dashboard/Product/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Dashboard/Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppleStore_MVC.Data.Product product, IFormFile ImageLink)
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            if (ModelState.IsValid)
            {
                if(ImageLink == null)
                {
                    ModelState.AddModelError(nameof(AppleStore_MVC.Data.Product.ImageLink), "Image is required.");
                    return View(product);
                }

                var imageName = Guid.NewGuid() + Path.GetExtension(ImageLink.FileName);

                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/product")))
                {
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/product"));
                }

                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/product", imageName);

                await using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await ImageLink.CopyToAsync(stream);
                }
                product.ImageLink = $"/Upload/product/{imageName}";

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Dashboard/Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // POST: Dashboard/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AppleStore_MVC.Data.Product product, IFormFile ImageLink)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldProduct = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
                    if (id != null)
                    {
                        if (ImageLink != null && ImageLink.Length > 0) // Có chọn ảnh mới
                        {
                            var imageName = Guid.NewGuid() + Path.GetExtension(ImageLink.FileName);
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/product", imageName);

                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                ImageLink.CopyTo(stream);
                            }

                            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/product")))
                            {
                                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/product"));
                            }

                            var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/product", imageName);

                            await using (var stream = new FileStream(savePath, FileMode.Create))
                            {
                                await ImageLink.CopyToAsync(stream);
                            }
                            oldProduct.ImageLink = $"/Upload/product/{imageName}";
                        }
                    }

                    oldProduct.Price = product.Price;
                    oldProduct.Description = product.Description;
                    oldProduct.ProductName = product.ProductName;
                    oldProduct.CategoryId = product.CategoryId;
                    oldProduct.Amount = product.Amount;
                    oldProduct.SoldQuantity = product.SoldQuantity;


                    _context.Update(oldProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            return View(product);
        }

        // GET: Dashboard/Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Dashboard/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var product = await _context.Products.FindAsync(id);
            //if (product != null)
            //{
            //    _context.Products.Remove(product);
            //}

            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));

            try
            {             
                var product = _context.Products.Find(id);
                _context.Products.Remove(product);
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

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
