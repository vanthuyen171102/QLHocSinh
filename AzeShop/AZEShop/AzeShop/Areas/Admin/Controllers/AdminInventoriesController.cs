using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AzeShop.Models;

namespace AzeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminInventoriesController : Controller
    {
        private readonly dbAZEShopContext _context;

        public AdminInventoriesController(dbAZEShopContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminInventories
        public async Task<IActionResult> Index()
        {
            var dbAZEShopContext = _context.Inventories.Include(i => i.Product).OrderByDescending(i=>i.MonthAndYear.ToLower());
            return View(await dbAZEShopContext.ToListAsync());
        }

        // GET: Admin/AdminInventories/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Inventories == null)
            {
                return NotFound();
            }
            var inventory = await _context.Inventories
                .Include(i => i.Product)
                .FirstOrDefaultAsync(m => m.MonthAndYear == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // GET: Admin/AdminInventories/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId");
            return View();
        }

        // POST: Admin/AdminInventories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MonthAndYear,ProductId,ImportAmount,ExportAmount,ImportMoney,ExportMoney")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inventory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", inventory.ProductId);
            return View(inventory);
        }

        // GET: Admin/AdminInventories/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Inventories == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", inventory.ProductId);
            return View(inventory);
        }

        // POST: Admin/AdminInventories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MonthAndYear,ProductId,ImportAmount,ExportAmount,ImportMoney,ExportMoney")] Inventory inventory)
        {
            if (id != inventory.MonthAndYear)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryExists(inventory.MonthAndYear))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", inventory.ProductId);
            return View(inventory);
        }

        // GET: Admin/AdminInventories/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Inventories == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventories
                .Include(i => i.Product)
                .FirstOrDefaultAsync(m => m.MonthAndYear == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // POST: Admin/AdminInventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Inventories == null)
            {
                return Problem("Entity set 'dbAZEShopContext.Inventories'  is null.");
            }
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory != null)
            {
                _context.Inventories.Remove(inventory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventoryExists(string id)
        {
          return _context.Inventories.Any(e => e.MonthAndYear == id);
        }
    }
}
