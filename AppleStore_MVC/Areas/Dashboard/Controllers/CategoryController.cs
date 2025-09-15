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
    public class CategoryController : Controller
    {
        private readonly AppleStoreContext _context;

        public CategoryController(AppleStoreContext context)
        {
            _context = context;
        }

        // GET: Dashboard/Category
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        // GET: Dashboard/Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Dashboard/Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dashboard/Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppleStore_MVC.Data.Category category, IFormFile Icon)
        {
            if (ModelState.IsValid)
            {
                if (Icon == null)
                {
                    ModelState.AddModelError(nameof(AppleStore_MVC.Data.Category.Icon), "Icon is required.");
                    return View(category);
                }

                if (_context.Categories.Any(c => c.CategoryId == category.CategoryId))
                {
                    ModelState.AddModelError("", "ID này đã tồn tại.");
                    return View(category);
                }

                var imageName = Guid.NewGuid() + Path.GetExtension(Icon.FileName);

                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/category")))
                {
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/category"));
                }

                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/category", imageName);

                await using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await Icon.CopyToAsync(stream);
                }
                category.Icon = imageName;

                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Dashboard/Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Dashboard/Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AppleStore_MVC.Data.Category category, IFormFile Icon)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldCategory = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);
                    if (oldCategory == null)
                    {
                        return NotFound();
                    }

                    // Nếu có chọn ảnh mới
                    if (Icon != null && Icon.Length > 0)
                    {
                        var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/category");
                        if (!Directory.Exists(uploadFolder))
                        {
                            Directory.CreateDirectory(uploadFolder);
                        }

                        var imageName = Guid.NewGuid() + Path.GetExtension(Icon.FileName);
                        var savePath = Path.Combine(uploadFolder, imageName);

                        await using (var stream = new FileStream(savePath, FileMode.Create))
                        {
                            await Icon.CopyToAsync(stream);
                        }

                        oldCategory.Icon = imageName;
                    }

                    // Cập nhật các trường khác
                    oldCategory.CategoryName = category.CategoryName;

                    _context.Update(oldCategory);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    if (ex is DbUpdateConcurrencyException)
                    {
                        if (!CategoryExists(category.CategoryId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    if (ex.InnerException != null && ex.InnerException.Message.Contains("PRIMARY"))
                    {
                        ModelState.AddModelError("", "Khóa chính đã tồn tại. Không thể cập nhật.");
                    }
                    else if (ex.InnerException != null && ex.InnerException.Message.Contains("FOREIGN"))
                    {
                        ModelState.AddModelError("", "Giá trị khóa ngoại không tồn tại. Vui lòng chọn lại.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Có lỗi khi cập nhật dữ liệu.");
                    }
                }
            }
            return View(category);
        }

        //public async Task<IActionResult> Edit(int id, AppleStore_MVC.Data.Category category, IFormFile Icon)
        //{
        //    if (id != category.CategoryId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var oldCategory = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);
        //            if (id != null)
        //            {
        //                if (Icon != null && Icon.Length > 0) // Có chọn ảnh mới
        //                {
        //                    var imageName = Guid.NewGuid() + Path.GetExtension(Icon.FileName);
        //                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/category", imageName);

        //                    using (var stream = new FileStream(path, FileMode.Create))
        //                    {
        //                        Icon.CopyTo(stream);
        //                    }


        //                    if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/category")))
        //                    {
        //                        Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/category"));
        //                    }

        //                    var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/category", imageName);

        //                    await using (var stream = new FileStream(savePath, FileMode.Create))
        //                    {
        //                        await Icon.CopyToAsync(stream);
        //                    }
        //                    oldCategory.Icon = imageName;
        //                }
        //            }

        //            oldCategory.CategoryId = category.CategoryId;
        //            oldCategory.CategoryName = oldCategory.CategoryName;

        //            _context.Update(oldCategory);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CategoryExists(category.CategoryId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(category);
        //}

        // GET: Dashboard/Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Dashboard/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                //var category = await _context.Categories.FindAsync(id);
                //if (category != null)
                //{
                //    _context.Categories.Remove(category);
                //}

                //await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));

                var category = _context.Categories.Find(id);
                _context.Categories.Remove(category);
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

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}
