using AspNetCoreHero.ToastNotification.Abstractions;
using AzeShop.Helpper;
using AzeShop.Models;
using AzeShop.ModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;

namespace AzeShop.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly dbAZEShopContext _context;
        public INotyfService _notyfService { get; }
        public CheckoutController(dbAZEShopContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        public const string CARTKEY = "cart";

        [Route("/checkout",Name ="Checkout")]
        public IActionResult Checkout(string? returnUrl)
        {
            var session = HttpContext.Session;
            string jsoncart = session.GetString(CARTKEY);
            if (string.IsNullOrEmpty(jsoncart))
            {
                _notyfService.Information("Giỏ hàng của bạn cần có ít nhất 1 sản phẩm để tiến hành thanh toán!");
                return Redirect("home");
            }
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            if (taikhoanID == null)
            {
                _notyfService.Information("Bạn cần đăng nhập để thực hiện thanh toán");
                return RedirectToAction("UserLogin", "User", new { returnUrl = "/checkout" });
            }
            List<AzeShop.ModelViews.CartItem> lsItem =  JsonConvert.DeserializeObject<List<AzeShop.ModelViews.CartItem>>(jsoncart);
            CheckoutModel model = new CheckoutModel();
            var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
            model.CustomerId = khachhang.CustomerId;
            model.FullName = khachhang.FullName.Trim();
            model.Phone = khachhang.Phone.Trim();
            model.Address = khachhang.Address;
            ViewData["City"] = new SelectList(_context.Cities.OrderBy(x => x.CityId).ToList(),"CityId","CityName");
            ViewBag.GioHang = lsItem;
            return View(model);
        }


        [HttpPost]
        [Route("checkout", Name = "Checkout")]
        public IActionResult Checkout(CheckoutModel order)
        {
            var session = HttpContext.Session;
            string jsoncart = session.GetString(CARTKEY);
            //Lay ra gio hang de xu ly
            var customerId = session.GetString("CustomerId");
            if (string.IsNullOrEmpty(jsoncart))
            {

                _notyfService.Information("Giỏ hàng của bạn cần có ít nhất 1 sản phẩm để tiến hành thanh toán!");
                return Redirect("home");
            }
            List<AzeShop.ModelViews.CartItem> lsItem = JsonConvert.DeserializeObject<List<AzeShop.ModelViews.CartItem>>(jsoncart);
            try
            {
                
                if (ModelState.IsValid)
                {
                    //Khoi tao don hang
                    Order donhang = new Order();
                    donhang.CustomerId = Convert.ToInt32(customerId);
                    donhang.FullName = order.FullName;
                    donhang.Address = order.Address;
                    donhang.WardId = order.Ward;
                    donhang.Phone = order.Phone;
                    donhang.OrderDate = DateTime.Now;
                    donhang.TransactStatusId = 1;
                    donhang.Note = Utilities.StripHTML(order.Note);
                    donhang.TotalMoney = lsItem.Sum(x => (100 - x.product.Discount) * x.product.Price / 100 *x.quantity) + GetDeliveryPrice(order.City);
                    _context.Add(donhang);
                    _context.SaveChanges();
                    //tao danh sach don hang
                    foreach (var item in lsItem)
                    {
                        OrderDetail orderDetail = new OrderDetail();
                        orderDetail.OrderId = donhang.OrderId;
                        orderDetail.ProductId = item.product.ProductId;
                        if(item.quantity > item.product.UnitsInStock)
                        {
                            _notyfService.Error("Số lượng tồn kho không đủ");
                            HttpContext.Session.Remove(CARTKEY);
                            return RedirectToAction("Index", "Home");
                        }
                        orderDetail.Amount = item.quantity;
                        orderDetail.Discount = item.product.Discount;
                        orderDetail.Price = item.product.Price;
                        orderDetail.CreateDate = DateTime.Now;
                        _context.Add(orderDetail);
                    }
                    _context.SaveChanges();
                    //clear gio hang
                    HttpContext.Session.Remove(CARTKEY);
                    //Xuat thong bao
                    _notyfService.Success("Đặt hàng thành công");
                    //cap nhat thong tin khach hang
                    return Redirect("/account");

                }
            }
            catch
            {
                _notyfService.Error("Đặt hàng không thành công");
                ViewData["City"] = new SelectList(_context.Cities.OrderBy(x => x.CityId).ToList(), "CityId", "CityName");
                ViewData["District"] = new SelectList(_context.Districts.OrderBy(x => x.DistrictId)
                                                     .Where(x => x.CityId == order.City).ToList(), "DistrictId", "DistrictName");
                ViewData["Ward"] = new SelectList(_context.Wards.OrderBy(x => x.WardId).
                    Where(x => x.DistrictId == order.District).ToList(), "WardId", "WardName");
                ViewBag.GioHang = lsItem;
                return View(order);
            }
            _notyfService.Error("Đặt hàng không thành công");
            ViewData["City"] = new SelectList(_context.Cities.OrderBy(x => x.CityId).ToList(), "CityId", "CityName");
            ViewData["District"] = new SelectList(_context.Districts.OrderBy(x => x.DistrictId)
                                                 .Where(x => x.CityId == order.City).ToList(), "DistrictId", "DistrictName");
            ViewData["Ward"] = new SelectList(_context.Wards.OrderBy(x => x.WardId).
                Where(x => x.DistrictId == order.District).ToList(), "WardId", "WardName"); ViewBag.GioHang = lsItem;
            return View(order);
        }

        [HttpGet]
        public decimal GetDeliveryPrice(int CityId)
        {
            return _context.Cities.FirstOrDefault(x => x.CityId == CityId).DeliveryPrice;
        }

        [HttpGet]
        public JsonResult DistrictList(int CityId)
        {
            var Districts = _context.Districts.Where(x => x.CityId == CityId).OrderBy(x => x.DistrictName).ToList();
            return Json(Districts.ToList());
        }
        public ActionResult WardList(int DistrictId)
        {
            List<Ward> Wards = _context.Wards.Where(x => x.DistrictId == DistrictId).OrderBy(x => x.WardName).ToList();
            return Json(Wards);
        }
    }
}
