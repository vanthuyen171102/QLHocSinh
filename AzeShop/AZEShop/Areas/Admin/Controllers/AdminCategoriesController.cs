using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AzeShop.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using AzeShop.Helpper;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AzeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin,Sale")]
    public class AdminCategoriesController : Controller
    {
        private readonly dbAZEShopContext _context;
        public INotyfService _notyfService { get; }


        public AdminCategoriesController(dbAZEShopContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;

        }

        // GET: Admin/AdminCategories
        public async Task<IActionResult> Index()
        {

            return View(await _context.Categories.ToListAsync());
        }

        // GET: Admin/AdminCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CatId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Admin/AdminCategories/Create


        [Authorize(Roles = "SuperAdmin,Admin")]

        public IActionResult Create()
        {
            return PartialView();
        }
        // POST: Admin/AdminCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]

        public async Task<IActionResult> Create([Bind("CatId,CatName,Description,Published,Thumb,Title,Alias")] Category category, IFormFile? fThumb)
        {
            if (ModelState.IsValid)
            {
                //Xu ly Thumb
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string imageName = Utilities.SEOUrl(category.CatName + "-" + DateTime.Now.Ticks.ToString()) + extension;
                    category.Thumb = await Utilities.UploadFile(fThumb, @"category", imageName.ToLower());
                }
                if (string.IsNullOrEmpty(category.Thumb))
                    category.Thumb = "default.jpg";
                category.Alias = Utilities.SEOUrl(category.CatName);
                _context.Add(category);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm mới thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Admin/AdminCategories/Edit/5

        [Authorize(Roles = "SuperAdmin,Admin")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories == null)
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

        // POST: Admin/AdminCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]

        public async Task<IActionResult> Edit(int id, [Bind("CatId,CatName,Description,Published,Thumb,Title,Alias")] Category category, IFormFile? fThumb)
        {
            if (id != category.CatId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (fThumb != null)
                    {
                        string extension = Path.GetExtension(fThumb.FileName);
                        string imageName = Utilities.SEOUrl(category.CatName + "-" + DateTime.Now.Ticks.ToString()) + extension;
                        category.Thumb = await Utilities.UploadFile(fThumb, @"category", imageName.ToLower());
                    }
                    if (string.IsNullOrEmpty(category.Thumb)) category.Thumb = "default.png";
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Chỉnh sửa thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CatId,category.CatName))
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
            return View(category);
        }

        // GET: Admin/AdminCategories/Delete/5
        [Authorize(Roles = "SuperAdmin,Admin")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                _notyfService.Error("Không tìm thấy danh mục");
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CatId == id);
            if (category == null)
            {
                _notyfService.Error("Không tìm thấy danh mục");
                return NotFound();
            }

            return View(category);
        }

        // POST: Admin/AdminCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                _notyfService.Error("Không tìm thấy danh mục");
                return RedirectToAction(nameof(Index));
            }
            var category = await _context.Categories.FindAsync(id);
            try
            {
                if (category != null)
                {

                    _context.Categories.Remove(category);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Xóa danh mục thành công");
                }
            }
            catch (Exception ex)
            {
                _notyfService.Error("Xóa danh mục không thành công");
                return RedirectToAction(nameof(Index));
            }


            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Lock")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]

        public async Task<IActionResult> Lock(int id)
        {
            if (_context.Categories == null)
            {
                _notyfService.Error("Không tìm thấy danh mục");
                return View();
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                category.Published = false;
                List<Product> lsProduct = _context.Products.Where(p => p.CatId == id).ToList();
                foreach(var item in lsProduct)
                {
                    item.Active = false;
                }
                _notyfService.Success("Khóa danh mục thành công");
                _context.Update(category);
                await _context.SaveChangesAsync();
            }
           
            return RedirectToAction(nameof(Index));
        }


        [HttpPost, ActionName("UnLock")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]

        public async Task<IActionResult> UnLock(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'dbAZEShopContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                category.Published = true;
                List<Product> lsProduct = _context.Products.Where(p => p.CatId == id).ToList();
                foreach (var item in lsProduct)
                {
                    item.Active = true;
                }
                _notyfService.Success("Mở khóa cho danh mục thành công");
                _context.Update(category);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id, string CatName)
        {
            return _context.Categories.Any(e => e.CatName == CatName  && e.CatId != id);
        }

        public JsonResult CheckCatNameExists(int id, string CatName)
        {
            if (CategoryExists(id, CatName))
                return Json(false);
            else return Json(true);
        }
        public JsonResult CheckAliasExists(int id, string alias)
        {
            if (_context.Categories.Any(e => e.Alias == alias && e.CatId != id))
                return Json(false);
            else return Json(true);
        }
    }
}
