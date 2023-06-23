using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AzeShop.Models;
using System.Security.Policy;
using NuGet.Protocol;
using NuGet.Versioning;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace AzeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class AdminAccountsController : Controller
    {
        private readonly dbAZEShopContext _context;

        public INotyfService _notyfService { get; }
        public AdminAccountsController(dbAZEShopContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminAccounts
        public async Task<IActionResult> Index()
        {
            var db = _context.Accounts.Include(a => a.Role);
            return View(await db.ToListAsync());

        }
        // GET: Admin/AdminAccounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                _notyfService.Error("Không tìm thấy tài khoản");
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                _notyfService.Error("Không tìm thấy tài khoản");
                return NotFound();
            }

            return View(account);
        }

        // GET: Admin/AdminAccounts/Create

        public IActionResult Create()
        {
            ViewData["QuyenTruyCap"] = new SelectList(_context.Roles, "RoleId", "RoleName");

            return View();
        }

        // POST: Admin/AdminAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("AccountId,AccountName,Password,Active,RoleId,LastLogin,CreateDate")] Account account)
        {
            if (ModelState.IsValid)
            {
                account.CreateDate = DateTime.Now;
                _context.Add(account);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm tài khoản thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuyenTruyCap"] = new SelectList(_context.Roles, "RoleId", "RoleName");
            return View(account);
        }

        // GET: Admin/AdminAccounts/Edit/5

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                _notyfService.Error("Không tìm thấy tài khoản");
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                _notyfService.Error("Không tìm thấy tài khoản");

                return NotFound();
            }
            ViewData["QuyenTruyCap"] = new SelectList(_context.Roles.Where(x => x.RoleId != 7), "RoleId", "RoleName", account.RoleId);

            return View(account);
        }

        // POST: Admin/AdminAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, [Bind("AccountId,AccountName,Password,Active,RoleId,LastLogin,CreateDate")] Account account)
        {
            if (id != account.AccountId)
            {
                _notyfService.Error("Không tìm thấy tài khoản");

                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    _notyfService.Success("Cập nhật thành công");
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(id,account.AccountName))
                    {
                        _notyfService.Error("Không tìm thấy tài khoản");
                        return NotFound();
                    }
                    else
                    {
                        _notyfService.Error("Cập nhật không thành công");
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuyenTruyCap"] = new SelectList(_context.Roles.Where(x => x.RoleId != 7), "RoleId", "RoleName", account.RoleId);

            return View(account);
        }

        // GET: Admin/AdminAccounts/Delete/5

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                _notyfService.Error("Không tìm thấy tài khoản");
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                _notyfService.Error("Không tìm thấy tài khoản");
                return NotFound();
            }
            return View(account);
        }

        


        // POST: Admin/AdminAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Accounts == null)
            {
                return Problem("Entity set 'dbAZEShopContext.Accounts'  is null.");
            }
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {

                try
                {
                    _context.Accounts.Remove(account);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Xóa tài khoản thành công");
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    _notyfService.Error("Xóa tài khoản không thành công");
                    return View(account);
                }
            }

            return View();
        }

        [AllowAnonymous]
        private bool AccountExists(int id, string AccountName)
        {
            return _context.Accounts.Any(e => e.AccountName == AccountName && e.AccountId != id);
        }


        [AllowAnonymous]
        public JsonResult CheckAccountNameExists(int id, string AccountName)
        {
            if (AccountExists(id,AccountName))
                return Json(false);
            else return Json(true);
        }
    }
}
