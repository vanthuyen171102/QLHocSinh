using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AzeShop.Models;
using AzeShop.Helpper;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using PagedList.Core;

namespace AzeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin,Sale")]

    public class AdminProductsController : Controller
    {
        private readonly dbAZEShopContext _context;
        public INotyfService _notyfService { get; }

        public AdminProductsController(dbAZEShopContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminProducts
        public async Task<IActionResult> Index()
        {
            var dbAZEShopContext = _context.Products.Include(x => x.Cat);
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatName", "CatName");
            return View(await dbAZEShopContext.ToListAsync());
        }


        public IActionResult ProductList( decimal? minPrice, decimal? maxPrice,bool? Stock,int? discountFrom,int? discountTo,bool? isActive,bool? isHomeFlag,bool? isBestSeller)
        {
            var lsProduct = _context.Products
               .AsNoTracking()
               .Include(x => x.Cat)
               .OrderBy(x => x.ProductId);
            
            if (minPrice != null && maxPrice != null)
            {
                lsProduct = lsProduct.Where(x => x.Price >= minPrice && x.Price <= maxPrice).OrderBy(x => x.ProductId);
            }
            if(Stock!=null)
            {
                if(Stock == true)
                {
                    lsProduct = lsProduct.Where(x => x.UnitsInStock > 0).OrderBy(x => x.ProductId);
                }
                else 
                {
                    lsProduct = lsProduct.Where(x => x.UnitsInStock == 0).OrderBy(x => x.ProductId);
                }
            }    
            if(isActive!=null)
            {
                lsProduct = lsProduct.Where(x => x.Active == isActive).OrderBy(x => x.ProductId);

            }
            if (isHomeFlag != null)
            {
                lsProduct = lsProduct.Where(x => x.HomeFlag == isHomeFlag).OrderBy(x => x.ProductId);

            }
            if (isBestSeller != null)
            {
                lsProduct = lsProduct.Where(x => x.BestSeller == isBestSeller).OrderBy(x => x.ProductId);

            }

            if (discountFrom != null && discountTo != null)
            {
                lsProduct = lsProduct.Where(x => x.Discount >= discountFrom && x.Discount <= discountTo).OrderBy(x => x.ProductId);
            }
            return PartialView("ProductList", lsProduct);
        }






        // GET: Admin/AdminProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Cat)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.Sales = _context.Inventories.Where(x=>x.ProductId==id).Sum(x=>x.ExportAmount);
            return View(product);
        }

        // GET: Admin/AdminProducts/Create
        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Create()
        {
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName");
            return View();
        }

        // POST: Admin/AdminProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]


        public async Task<IActionResult> Create([Bind("ProductId,ProductName,Description,CatId,Price,Discount,UnitsInStock,Thumb,DateCreated,DateModified,Active,HomeFlag,BestSeller,ShortDesc,Title,Alias,MetaDesc,MetaKey,Tags")] Product product, IFormFile? fThumb)
        {
            if (ModelState.IsValid)
            {
                product.ProductName = Utilities.ToTitleCase(product.ProductName);
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string image = Utilities.SEOUrl(product.ProductName +"-"+ DateTime.Now.Ticks.ToString()) + extension;
                    product.Thumb = await Utilities.UploadFile(fThumb, @"products", image.ToLower());
                }
                if (string.IsNullOrEmpty(product.Thumb)) product.Thumb = "default.jpg";
                product.Alias = Utilities.SEOUrl(product.ProductName);
                product.DateModified = DateTime.Now;
                product.DateCreated = DateTime.Now;

                _context.Add(product);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm sản phẩm thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName", product.CatId);
            return View(product);
        }

        // GET: Admin/AdminProducts/Edit/5
        [Authorize(Roles = "SuperAdmin,Admin")]

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
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName",product.CatId);
            return View(product);
        }

        // POST: Admin/AdminProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]

        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ShortDesc,Description,CatId,Price,Discount,Thumb,DateCreated,DateModified,BestSeller,HomeFlag,Active,Tags,Title,Alias,MetaDesc,MetaKey,UnitsInStock")] Product product, Microsoft.AspNetCore.Http.IFormFile? fThumb)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    product.ProductName = Utilities.ToTitleCase(product.ProductName);
                    if (fThumb != null)
                    {
                        string extension = Path.GetExtension(fThumb.FileName);
                        string image = Utilities.SEOUrl(product.ProductName + "-" + DateTime.Now.Ticks.ToString()) + extension;
                        product.Thumb = await Utilities.UploadFile(fThumb, @"products", image.ToLower());
                    }
                    if (string.IsNullOrEmpty(product.Thumb)) product.Thumb = "default.jpg";
                    product.Alias = Utilities.SEOUrl(product.ProductName);
                    product.DateModified = DateTime.Now;

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Cập nhật thành công");

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
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName", product.CatId);
            return View(product);
        }

        // GET: Admin/AdminProducts/Delete/5
        [Authorize(Roles = "SuperAdmin,Admin")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Cat)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/AdminProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]


        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                _notyfService.Error("Không tìm thấy sản phẩm");
                return RedirectToAction(nameof(Index));
            }
            var product = await _context.Products.FindAsync(id);
            try
            {
                if (product != null)
                {

                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Xóa sản phẩm thành công");
                }
            }
            catch (Exception ex)
            {
                _notyfService.Error("Xóa sản phẩm không thành công");
                return RedirectToAction(nameof(Index));
            }


            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Lock")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]

        public async Task<IActionResult> Lock(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'dbAZEShopContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                product.Active = false;
                _notyfService.Success("Khóa sản phẩm thành công");
                _context.Update(product);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpPost, ActionName("UnLock")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]

        public async Task<IActionResult> UnLock(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'dbAZEShopContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                product.Active = true;
                _notyfService.Success("Mở khóa cho sản phẩm thành công");
                _context.Update(product);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool ProductExists(int id)
        {
          return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
