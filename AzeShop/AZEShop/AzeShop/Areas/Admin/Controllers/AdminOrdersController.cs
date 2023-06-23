using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AzeShop.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using System.Net.WebSockets;

namespace AzeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin,Sale")]
    public class AdminOrdersController : Controller
    {
        private readonly dbAZEShopContext _context;
        public INotyfService _notyfService { get; }
        public AdminOrdersController(dbAZEShopContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminOrders
        public async Task<IActionResult> Index()
        {
            var dbAZEShopContext = _context.Orders.Include(o => o.Customer).Include(o => o.TransactStatus).Include(o => o.Ward.District.City).OrderBy(x => x.TransactStatusId).ToList();
            ViewData["Status"] = new SelectList(_context.TransactStatuses, "Status", "Status");
            return View(dbAZEShopContext);
        }

        // GET: Admin/AdminOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.TransactStatus)
                .Include(o => o.Ward)
                .Include(o => o.Ward.District)
                .Include(o => o.Ward.District.City)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            var orderDetailsList = _context.OrderDetails
                .Include(x => x.Product)
                .AsNoTracking()
                .Where(x => x.OrderId == order.OrderId)
                .OrderBy(x => x.Order.OrderDate)
                .ToList();
            ViewBag.OrderDetailsList = orderDetailsList;

            return View(order);
        }

        // GET: Admin/AdminOrders/Create

        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");
            ViewData["TransactStatusId"] = new SelectList(_context.TransactStatuses, "TransactStatusId", "TransactStatusId");
            ViewData["WardId"] = new SelectList(_context.Wards, "WardId", "WardId");
            return View();
        }

        // POST: Admin/AdminOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,CustomerId,OrderDate,DeliveryDate,TransactStatusId,PaymentDate,TotalMoney,Note,Address,WardId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", order.CustomerId);
            ViewData["TransactStatusId"] = new SelectList(_context.TransactStatuses, "TransactStatusId", "TransactStatusId", order.TransactStatusId);
            ViewData["WardId"] = new SelectList(_context.Wards, "WardId", "WardId", order.WardId);
            return View(order);
        }

        // GET: Admin/AdminOrders/Edit/5

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", order.CustomerId);
            ViewData["TransactStatusId"] = new SelectList(_context.TransactStatuses, "TransactStatusId", "TransactStatusId", order.TransactStatusId);
            ViewData["WardId"] = new SelectList(_context.Wards, "WardId", "WardId", order.WardId);
            return View(order);
        }

        // POST: Admin/AdminOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, [Bind("OrderId,CustomerId,OrderDate,DeliveryDate,TransactStatusId,PaymentDate,TotalMoney,Note,Address,WardId")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", order.CustomerId);
            ViewData["TransactStatusId"] = new SelectList(_context.TransactStatuses, "TransactStatusId", "TransactStatusId", order.TransactStatusId);
            ViewData["WardId"] = new SelectList(_context.Wards, "WardId", "WardId", order.WardId);
            return View(order);
        }


        public async Task<IActionResult> ChangeStatus(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .AsNoTracking()
                .Include(x => x.Customer)
                .FirstOrDefaultAsync(x => x.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            if (order.TransactStatusId == 5)
            {
                _notyfService.Information("Đơn hàng này đã hoàn thành!");
                ViewData["Trangthai"] = new SelectList(_context.TransactStatuses.Where(x => x.TransactStatusId >= order.TransactStatusId), "TransactStatusId", "Status", order.TransactStatusId);
                return PartialView("ChangeStatus", order);
            }
            if (order.TransactStatusId == 6)
            {
                _notyfService.Information("Đơn hàng này đã bị hủy!");
                ViewData["Trangthai"] = new SelectList(_context.TransactStatuses.Where(x => x.TransactStatusId >= order.TransactStatusId), "TransactStatusId", "Status", order.TransactStatusId);
                return PartialView("ChangeStatus", order);
            }
            ViewData["Trangthai"] = new SelectList(_context.TransactStatuses.Where(x => x.TransactStatusId >= order.TransactStatusId), "TransactStatusId", "Status", order.TransactStatusId);
            return PartialView("ChangeStatus", order);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id, [Bind("OrderId,CustomerId,OrderDate,DeliveryDate,TotalMoney,TransactStatusId,PaymentDate,Note,Address,WardId")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var donhang = await _context.Orders.AsNoTracking().Include(x => x.Customer).FirstOrDefaultAsync(x => x.OrderId == id);
                     
                    if (donhang != null)
                    {
                        if (donhang.TransactStatusId == 5)
                        {
                            _notyfService.Information("Đơn hàng này đã hoàn thành!");
                            ViewData["Trangthai"] = new SelectList(_context.TransactStatuses.Where(x => x.TransactStatusId >= order.TransactStatusId), "TransactStatusId", "Status", order.TransactStatusId);
                            return PartialView("ChangeStatus", order);
                        }
                        if (donhang.TransactStatusId == 6)
                        {
                            _notyfService.Information("Đơn hàng này đã bị hủy!");
                            ViewData["Trangthai"] = new SelectList(_context.TransactStatuses.Where(x => x.TransactStatusId >= order.TransactStatusId), "TransactStatusId", "Status", order.TransactStatusId);
                            return PartialView("ChangeStatus", order);
                        }

                        //Đơn hàng được xuất kho thì cập nhật lại số lượng tồn kho
                        if (donhang.TransactStatusId < 3 && order.TransactStatusId >= 3 && order.TransactStatusId != 6)
                        {
                            List<OrderDetail> lsOrderDetails = _context.OrderDetails.Where(x => x.OrderId == order.OrderId).ToList();
                            foreach(var orderDetails in lsOrderDetails)
                            {
                                Product product = _context.Products.FirstOrDefault(x => x.ProductId == orderDetails.ProductId);
                                if(product == null)
                                {
                                    _notyfService.Error("Không tìm thấy sản phẩm!");
                                    ViewData["Trangthai"] = new SelectList(_context.TransactStatuses.Where(x => x.TransactStatusId >= order.TransactStatusId), "TransactStatusId", "Status", order.TransactStatusId);
                                    return PartialView("ChangeStatus", order);
                                }
                                if (product.UnitsInStock < orderDetails.Amount)
                                {
                                    _notyfService.Error("Số lượng tồn kho của sản phẩm "+ product.ProductName+ " không đủ để thực hiện xuất kho!");
                                    ViewData["Trangthai"] = new SelectList(_context.TransactStatuses.Where(x => x.TransactStatusId >= order.TransactStatusId), "TransactStatusId", "Status", order.TransactStatusId);
                                    return PartialView("ChangeStatus", order);
                                }
                                else
                                {
                                    product.UnitsInStock -= orderDetails.Amount;
                                }    
                            }
                        }

                        //Đơn hàng đã hoàn thành
                        if (order.TransactStatusId == 5)
                        {
                            //Nếu đơn hàng chưa thanh toán
                            if (donhang.PaymentDate == null)
                            {
                                //Cập nhật ngày thanh toán
                                donhang.PaymentDate = DateTime.Now;

                                //Làm mới báo cáo thống kê của sản phẩm
                                List<OrderDetail> lsOrderDetails = _context.OrderDetails.Where(x => x.OrderId == order.OrderId).ToList();
                                foreach (var orderDetails in lsOrderDetails)
                                {
                                    Product product = _context.Products.FirstOrDefault(x => x.ProductId == orderDetails.ProductId);
                                    if (product == null)
                                    {
                                        _notyfService.Error("Không tìm thấy sản phẩm!");
                                        ViewData["Trangthai"] = new SelectList(_context.TransactStatuses.Where(x => x.TransactStatusId >= order.TransactStatusId), "TransactStatusId", "Status", order.TransactStatusId);
                                        return PartialView("ChangeStatus", order);
                                    }
                                    else
                                    {
                                        //Cập nhật bảng thống kê
                                        var month_and_year = donhang.PaymentDate.Value.Month.ToString() + "/" + donhang.PaymentDate.Value.Year.ToString();
                                        var inventory = await _context.Inventories
                                                    .AsNoTracking()
                                                    .FirstOrDefaultAsync(x => x.ProductId == orderDetails.ProductId && x.MonthAndYear == month_and_year);
                                        if (inventory == null)
                                        {
                                            inventory = new Inventory();
                                            inventory.MonthAndYear = month_and_year;
                                            inventory.ProductId = product.ProductId;
                                            inventory.ImportAmount = 0;
                                            inventory.ExportAmount = orderDetails.Amount;
                                            inventory.ImportMoney = 0;
                                            inventory.ExportMoney = (100 - orderDetails.Discount) * (orderDetails.Amount * orderDetails.Price) / 100;
                                            _context.Add(inventory);
                                        }
                                        else
                                        {
                                            inventory.ExportAmount += orderDetails.Amount;
                                            inventory.ExportMoney += (100 - orderDetails.Discount) * (orderDetails.Amount * orderDetails.Price) / 100;
                                            _context.Update(inventory);
                                        }
                                        _context.Update(product);
                                        _context.SaveChanges();
                                    }
                                }
                            }
                            donhang.DeliveryDate = DateTime.Now;
                        }

                        //Đơn hàng đã xuất kho và bị hủy thì nhập lại hàng vào kho
                        if (donhang.TransactStatusId >= 3 && donhang.TransactStatusId != 5 && order.TransactStatusId == 6)
                        {
                            List<OrderDetail> lsOrderDetails = _context.OrderDetails.Where(x => x.OrderId == order.OrderId).ToList();
                            foreach (var orderDetails in lsOrderDetails)
                            {
                                Product product = _context.Products.FirstOrDefault(x => x.ProductId == orderDetails.ProductId);
                                if (product == null)
                                {
                                    _notyfService.Error("Không tìm thấy sản phẩm!");
                                    ViewData["Trangthai"] = new SelectList(_context.TransactStatuses.Where(x => x.TransactStatusId >= order.TransactStatusId), "TransactStatusId", "Status", order.TransactStatusId);
                                    return PartialView("ChangeStatus", order);
                                }
                                else
                                {
                                    product.UnitsInStock += orderDetails.Amount;
                                }
                            }
                        }

                        
                        donhang.TransactStatusId = order.TransactStatusId;
                    }
                    _context.Update(donhang);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Cập nhật trạng thái đơn hàng thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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
            _notyfService.Success("Cập nhật trạng thái hóa đơn thành công");
            ViewData["Trangthai"] = new SelectList(_context.TransactStatuses.Where(x => x.TransactStatusId >= order.TransactStatusId), "TransactStatusId", "Status", order.TransactStatusId);
            return PartialView("ChangeStatus", order);
        }

        // GET: Admin/AdminOrders/Delete/5

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.TransactStatus)
                .Include(o => o.Ward)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Admin/AdminOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'dbAZEShopContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
