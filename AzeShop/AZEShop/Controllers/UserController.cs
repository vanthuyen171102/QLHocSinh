using AspNetCoreHero.ToastNotification.Abstractions;
using AzeShop.Helpper;
using AzeShop.Models;
using AzeShop.ModelViews;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;

namespace AzeShop.Controllers
{
    public class UserController : Controller
    {
        private readonly dbAZEShopContext _context;
        public INotyfService _notyfService { get; }
        public UserController(dbAZEShopContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        [Route("/account", Name = "Account")]
        public IActionResult MyAccount()
        {
            var customerId = HttpContext.Session.GetString("CustomerId");
            if (customerId != null)
            {
                var customer = _context.Customers.AsNoTracking().Include(x => x.City).SingleOrDefault(x => x.CustomerId == Convert.ToInt32(customerId));
                if (customer != null)
                {
                    var lsDonHang = _context.Orders
                        .Include(x => x.TransactStatus)
                        .AsNoTracking()
                        .Where(x => x.CustomerId == customer.CustomerId)
                        .OrderByDescending(x => x.OrderDate)
                        .ToList();
                    ViewBag.Orders = lsDonHang;
                    ViewData["TinhThanh"] = new SelectList(_context.Cities, "CityId", "CityName");
                    return View(customer);
                }
            }
            ViewData["TinhThanh"] = new SelectList(_context.Cities, "CityId", "CityName");
            _notyfService.Information("Bạn cần đăng nhập để sử dụng chức năng này");
            return RedirectToAction("UserLogin", "User", new { returnUrl = "/account" });
        }



        public IActionResult EditProfile([Bind("CustomerId,Avatar,FullName,Birthday,Address,CityId,Email,Phone,CreateDate,Password,LastLogin,Active")] Customer customer)
        {
            var customerId = HttpContext.Session.GetString("CustomerId");
            if (customerId != null)
            {
                try
                {
                    _context.Update(customer);
                    _context.SaveChanges();
                    _notyfService.Success("Chỉnh sửa thành công");
                    ViewData["TinhThanh"] = new SelectList(_context.Cities, "CityId", "CityName",customer.CityId);
                    return RedirectToAction("MyAccount", "User");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_context.Customers.Where(x => x.CustomerId ==  customer.CustomerId) == null)
                    {
                        ViewData["TinhThanh"] = new SelectList(_context.Cities, "CityId", "CityName", customer.CityId);
                        _notyfService.Error("Không tìm thấy khách hàng trong hệ thống");
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewData["TinhThanh"] = new SelectList(_context.Cities, "CityId", "CityName", customer.CityId);
                        _notyfService.Error("Thay đổi thông tin không thành công");
                        return RedirectToAction("MyAccount", "User");
                    }
                }
                ViewData["TinhThanh"] = new SelectList(_context.Cities, "CityId", "CityName", customer.CityId);
                _notyfService.Error("Thay đổi thông tin không thành công");
                return RedirectToAction("MyAccount", "User");
            }
            _notyfService.Information("Bạn cần đăng nhập để sử dụng chức năng này");
            return RedirectToAction("UserLogin", "User", new { returnUrl = "/account" });
            
        }

        [HttpPost]
        public IActionResult ChangePassword(string Password, string NewPassword)
        {
            try
            {
                var customerId = HttpContext.Session.GetString("CustomerId");
                if (customerId == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                if (ModelState.IsValid)
                {
                    var customer = _context.Customers.Find(Convert.ToInt32(customerId));
                    if (customer == null)
                        return RedirectToAction("UserLogin", "User");
                    if (customer.Password != Password)
                    {
                        ViewBag.Error = "Mật khẩu hiện tại không chính xác!";
                        return Redirect("/account");
                    }
                    customer.Password = NewPassword;
                    _context.Update(customer);
                    _context.SaveChanges();
                    _notyfService.Success("Đổi mật khẩu thành công");
                    return RedirectToAction("MyAccount", "User");
                }
            }
            catch
            {
                _notyfService.Error("Thay đổi mật khẩu không thành công");
                return RedirectToAction("MyAccount", "User");
            }
            _notyfService.Error("Thay đổi mật khẩu không thành công");
            return RedirectToAction("MyAccount", "User");
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult CheckPassword(string Password)
        {
            var customerId = HttpContext.Session.GetString("CustomerId");
            if(customerId!=null)
            {
                try
                {
                    var customer = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(customerId));
                    if (customer != null && customer.Password.Trim()!=Password.Trim())
                        return Json(false);
                    return Json(true);
                }
                catch
                {
                    return Json(true);
                }
            }
            return Json(true);
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult CheckPhoneUsed(string Phone)
        {
            try
            {
                var customer = _context.Customers.AsNoTracking().SingleOrDefault(x => x.Phone.ToLower() == Phone.ToLower());
                if (customer != null)
                    return Json(false);
                return Json(true);
            }
            catch
            {
                return Json(true);
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public JsonResult CheckEmailUsed(string? Email)
        {
            try
            {
                var customer = _context.Customers.AsNoTracking().SingleOrDefault(x => x.Email.ToLower() == Email.ToLower());
                if (customer != null)
                    return Json(false);
                return Json(true);
            }
            catch
            {
                return Json(true);
            }
        }


        [AllowAnonymous]
        [Route("login", Name = "UserLogin")]
        public IActionResult UserLogin(string? returnUrl)
        {
            var customerID = HttpContext.Session.GetString("CustomerId");
            if (customerID != null)
            {
                return Redirect("/account");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login", Name = "UserLogin")]
        public async Task<IActionResult> UserLogin(UserLoginModel customer, string? returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isEmail = Utilities.IsValidEmail(customer.UserName);
                    if (!isEmail)
                    {
                        ViewBag.Error = "Tên đăng nhập phải là dạng Email!";
                        return View(customer);
                    }
                    var khachhang = _context.Customers.SingleOrDefault(x => x.Email.Trim() == customer.UserName);
                    if (khachhang == null)
                    {
                        ViewBag.Error = "Tên đăng nhập không tồn tại!";
                        return View(customer);
                    }
                    string pass = customer.Password;
                    if (khachhang.Password != pass)
                    {
                        ViewBag.Error = "Thông tin đăng nhập không chính xác!";
                        return View(customer);
                    }
                    //kiem tra xem account co bi disable hay khong

                    if (khachhang.Active == false)
                    {
                        ViewBag.Error = "Tài khoản của bạn bị hạn chế truy cập!\nLiên hệ: 0986354108";
                        return View(customer);
                    }

                    khachhang.LastLogin = DateTime.Now;
                    _context.Update(khachhang);
                    _context.SaveChanges();

                    //Luu Session MaKh
                    HttpContext.Session.SetString("CustomerId", khachhang.CustomerId.ToString());
                    var taikhoanID = HttpContext.Session.GetString("CustomerId");

                    //Identity
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, khachhang.FullName),
                        new Claim("CustomerId", khachhang.CustomerId.ToString())
                    };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    _notyfService.Success("Đăng nhập thành công");
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return Redirect(returnUrl);
                    }
                }
            }
            catch
            {
                _notyfService.Error("Đăng nhập không thành công");
                return View(customer);
            }
            _notyfService.Error("Đăng nhập không thành công");
            return View(customer);
        }
        [HttpGet]
        [Route("logout", Name = "UserLogout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("CustomerId");
            _notyfService.Success("Đăng xuất thành công");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("register", Name = "Register")]
        public IActionResult UserRegister()
        {
            ViewData["TinhThanh"] = new SelectList(_context.Cities, "CityId", "CityName");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register", Name = "Register")]
        public async Task<IActionResult> UserRegister(UserRegisterModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Customer khachhang = new Customer
                    {
                        FullName = user.FullName,
                        Address = user.Address,
                        Phone = user.Phone.Trim().ToLower(),
                        Email = user.Email.Trim().ToLower(),
                        Password = user.Password.Trim(),
                        Avatar = "default.png",
                        Active = true,
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        CityId = user.CityId
                    };
                    try
                    {
                        _context.Add(khachhang);
                        await _context.SaveChangesAsync();
                        //Lưu Session MaKh
                        HttpContext.Session.SetString("CustomerId", khachhang.CustomerId.ToString());
                        var userID = HttpContext.Session.GetString("CustomerId");

                        //Identity
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,khachhang.FullName),
                            new Claim("CustomerId", khachhang.CustomerId.ToString())
                        };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        _notyfService.Success("Đăng ký thành công");
                        ViewData["TinhThanh"] = new SelectList(_context.Cities, "CityId", "CityName");
                        return RedirectToAction("Index", "Home");
                    }
                    catch
                    {
                        ViewData["TinhThanh"] = new SelectList(_context.Cities, "CityId", "CityName");
                        _notyfService.Error("Đăng ký không thành công!");
                        return View();
                    }
                }
                else
                {
                    ViewData["TinhThanh"] = new SelectList(_context.Cities, "CityId", "CityName");
                    _notyfService.Error("Đăng ký không thành công!");
                    return View();
                }
            }
            catch
            {
                ViewData["TinhThanh"] = new SelectList(_context.Cities, "CityId", "CityName");
                _notyfService.Error("Đăng ký không thành công!");
                return View();
            }
        }


        [Route("/MyOrder/{id}", Name = "MyOrder")]
        public async Task<IActionResult> MyOrders(int? id)
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
    }
}
