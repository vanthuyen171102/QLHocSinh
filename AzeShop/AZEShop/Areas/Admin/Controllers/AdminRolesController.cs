using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AzeShop.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace AzeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class AdminRolesController : Controller
    {
        private readonly dbAZEShopContext _context;

        public INotyfService _notyfService { get; }


        public AdminRolesController(dbAZEShopContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminRoles
        [Route("/roles", Name = "AdminRole")]
        public async Task<IActionResult> Index()
        {
              return View(await _context.Roles.ToListAsync());
        }

        // GET: Admin/AdminRoles/Details/5
        [Route("/roles/{id}", Name = ("RoleDetails"))]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Roles == null)
            {
                _notyfService.Error("Không tìm thấy quyền truy cập");
                return NotFound();
            }

            var role = await _context.Roles
                .FirstOrDefaultAsync(m => m.RoleId == id);
            if (role == null)
            {
                _notyfService.Error("Không tìm thấy quyền truy cập");
                return NotFound();
            }

            return View(role);
        }

        [Route("/roles/addRole", Name = "AddRole")]
        // GET: Admin/AdminRoles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/roles/addRole", Name = "AddRole")]
        public async Task<IActionResult> Create([Bind("RoleId,RoleName,Description")] Role role)
        {
            if (ModelState.IsValid)
            {
                _context.Add(role);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm quyền truy cập thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        [Route("/roles/editRole/{id}", Name = ("EditRole"))]
        // GET: Admin/AdminRoles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Roles == null)
            {
                _notyfService.Error("Không tìm thấy quyền truy cập");
                return NotFound();
            }

            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                _notyfService.Error("Không tìm thấy quyền truy cập");
                return NotFound();
            }
            return View(role);
        }

        // POST: Admin/AdminRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/roles/editRole/{id}", Name = ("EditRole"))]

        public async Task<IActionResult> Edit(int id, [Bind("RoleId,RoleName,Description")] Role role)
        {
            if (id != role.RoleId)
            {
                _notyfService.Error("Không tìm thấy quyền truy cập");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(role);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Sửa quyền truy cập thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(role.RoleId))
                    {
                        _notyfService.Error("Không tìm thấy quyền truy cập");
                        return NotFound();
                    }
                    else
                    {
                        _notyfService.Error("Sửa quyền truy cập không thành công");
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: Admin/AdminRoles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Roles == null)
            {
                _notyfService.Error("Không tìm thấy quyền truy cập");

                return NotFound();
            }

            var role = await _context.Roles
                .FirstOrDefaultAsync(m => m.RoleId == id);
            if (role == null)
            {
                _notyfService.Error("Không tìm thấy quyền truy cập");

                return NotFound();
            }

            return View(role);
        }

        // POST: Admin/AdminRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Roles == null)
            {
                return Problem("Entity set 'dbAZEShopContext.Roles'  is null.");
            }
            var role = await _context.Roles.FindAsync(id);
            if (role != null)
            {
                try
                {
                    _context.Roles.Remove(role);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Xóa quyền truy cập thành công");
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    _notyfService.Error("Xóa quyền truy cập không thành công");
                    return View(role);
                }
            }
            return View();
        }

        private bool RoleExists(int id)
        {
          return _context.Roles.Any(e => e.RoleId == id);
        }
    }
}
