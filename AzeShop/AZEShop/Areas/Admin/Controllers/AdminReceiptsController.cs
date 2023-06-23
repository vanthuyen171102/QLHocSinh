using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AzeShop.Models;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace AzeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminReceiptsController : Controller
    {
        private readonly dbAZEShopContext _context;
        public INotyfService _notyfService { get; }

        public AdminReceiptsController(dbAZEShopContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminReceipts
        public async Task<IActionResult> Index()
        {
            var dbAZEShopContext = _context.Receipts.Include(r => r.Product);
            return View(await dbAZEShopContext.ToListAsync());
        }

        // GET: Admin/AdminReceipts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Receipts == null)
            {
                return NotFound();
            }

            var receipt = await _context.Receipts
                .Include(r => r.Product)
                .FirstOrDefaultAsync(m => m.ReceiptId == id);
            if (receipt == null)
            {
                return NotFound();
            }

            return View(receipt);
        }

        // GET: Admin/AdminReceipts/Create
        public IActionResult Create()
        {
            ViewData["SanPham"] = new SelectList(_context.Products, "ProductId", "ProductName");
            return View();
        }

        // POST: Admin/AdminReceipts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReceiptId,ProductId,Price,DateCreated,Amount")] Receipt receipt)
        {
            if (ModelState.IsValid)
            {
                if (receipt.DateCreated == null)
                    receipt.DateCreated = DateTime.Now;
                _context.Add(receipt);
                var product = await _context.Products
                                .AsNoTracking()
                                .FirstOrDefaultAsync(x => x.ProductId == receipt.ProductId);
                if(product!=null)
                {
                    //Cập nhật thông tin sản phẩm
                    var profit = (decimal) 120 / 100;
                    var new_price = ((profit*receipt.Price*receipt.Amount) +(product.Price*product.UnitsInStock))/(receipt.Amount+product.UnitsInStock);
                    product.Price = new_price;
                    product.UnitsInStock += receipt.Amount;
                    product.DateModified = receipt.DateCreated;
                    //Cập nhật bảng thống kê
                    var month_and_year = receipt.DateCreated.Value.Month.ToString() + "/" + receipt.DateCreated.Value.Year.ToString();
                    var inventory = await _context.Inventories
                                .AsNoTracking()
                                .FirstOrDefaultAsync(x => x.ProductId == receipt.ProductId && x.MonthAndYear == month_and_year);
                    if(inventory == null)
                    {
                        inventory = new Inventory();
                        inventory.MonthAndYear = month_and_year;
                        inventory.ProductId = product.ProductId;
                        inventory.ImportAmount = receipt.Amount;
                        inventory.ExportAmount = 0;
                        inventory.ImportMoney = receipt.Amount * receipt.Price;
                        inventory.ExportMoney = 0;
                        _context.Add(inventory);
                    }
                    else
                    {
                        inventory.ImportAmount += receipt.Amount;
                        inventory.ImportMoney += receipt.Amount * receipt.Price;
                        _context.Update(inventory);

                    }
                    _context.Update(product);
                    _context.SaveChanges();
                }
                await _context.SaveChangesAsync();
                _notyfService.Success("Tạo biên lai thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["SanPham"] = new SelectList(_context.Products, "ProductId", "ProductName");
            return View(receipt);
        }

        // GET: Admin/AdminReceipts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Receipts == null)
            {
                return NotFound();
            }

            var receipt = await _context.Receipts.FindAsync(id);
            if (receipt == null)
            {
                _notyfService.Error("Không tìm thấy biên lai");
                return NotFound();
            }
            ViewData["SanPham"] = new SelectList(_context.Products, "ProductId", "ProductName",receipt.ProductId);
            return View(receipt);
        }

        // POST: Admin/AdminReceipts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReceiptId,ProductId,Price,DateCreated,Amount")] Receipt receipt)
        {
            if (id != receipt.ReceiptId)
            {
                _notyfService.Error("Không tìm thấy biên lai");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(receipt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceiptExists(receipt.ReceiptId))
                    {
                        _notyfService.Error("Không tìm thấy biên lai");
                        return NotFound();
                    }
                    else
                    {
                        _notyfService.Error("Cập nhật biên lai không thành công");
                        return RedirectToAction(nameof(Index));
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SanPham"] = new SelectList(_context.Products, "ProductId", "ProductName",receipt.ProductId);
            return View(receipt);
        }

        // GET: Admin/AdminReceipts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Receipts == null)
            {
                _notyfService.Error("Không tìm thấy biên lai");
                return NotFound();
            }

            var receipt = await _context.Receipts
                .Include(r => r.Product)
                .FirstOrDefaultAsync(m => m.ReceiptId == id);
            if (receipt == null)
            {
                _notyfService.Error("Không tìm thấy biên lai");
                return NotFound();
            }
            return View(receipt);
        }

        // POST: Admin/AdminReceipts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Receipts == null)
            {
                _notyfService.Error("Không tìm thấy biên lai");
                return NotFound();
            }
            var receipt = await _context.Receipts.FindAsync(id);
            if (receipt != null)
            {
                _context.Receipts.Remove(receipt);
            }
            _notyfService.Success("Xóa biên lai thành công");
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReceiptExists(int id)
        {
          return _context.Receipts.Any(e => e.ReceiptId == id);
        }
    }
}
