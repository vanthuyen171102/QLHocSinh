using AspNetCoreHero.ToastNotification.Abstractions;
using AzeShop.Areas.Admin.Models;
using AzeShop.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AzeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly dbAZEShopContext _context;
        public INotyfService _notyfService { get; }
        public AccountController(dbAZEShopContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("admin-login", Name = "AdminLogin")]
        public IActionResult AdminLogin(string? returnUrl)
        {
            var accID = HttpContext.Session.GetString("AccountId");
            if (accID != null)
                return RedirectToAction("Index", "Home", new { Area = "Admin" });
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("admin-login", Name = "AdminLogin")]
        public async Task<IActionResult> AdminLogin(LoginModel model, string? returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Lấy về account có tên tài khoản trùng trong CSDL
                    Account acc = _context.Accounts
                    .Include(p => p.Role)
                    .FirstOrDefault(p => p.AccountName.ToLower() == model.AccountName.ToLower().Trim());

                    //Trả về Error khi không có tài khoản nào phù hợp
                    if (acc == null)
                    {
                        ViewBag.Error = "Thông tin đăng nhập chưa chính xác";
                        return View(model);
                    }
                    string password = (model.Password.Trim());
                    if (acc.Password.Trim() != password)
                    {
                        ViewBag.Error = "Thông tin đăng nhập chưa chính xác";
                        return View(model);
                    }

                    //đăng nhập thành công
                    //ghi nhận thời gian đăng nhập
                    acc.LastLogin = DateTime.Now;
                    _context.Update(acc);
                    await _context.SaveChangesAsync();


                    var accID = HttpContext.Session.GetString("AccountId");
                    //identity
                    //Lưu seccion Acc
                    HttpContext.Session.SetString("AccountId", acc.AccountId.ToString());
                    HttpContext.Session.SetString("AccountName", acc.AccountName.ToString());
                    HttpContext.Session.SetString("RoleName", acc.Role.RoleName.ToString());
                    ViewBag.RoleName = acc.Role.RoleName;
                    ViewBag.AccountName = acc.AccountName;

                    //identity
                    var userClaims = new List<Claim>
                    {
                        new Claim("AccountId", acc.AccountId.ToString()),
                        new Claim("RoleId", acc.RoleId.ToString()),
                        new Claim(ClaimTypes.Role, acc.Role.RoleName)
                    };
                    var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
                    var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
                    await HttpContext.SignInAsync(userPrincipal);
                    return RedirectToAction("Index", "Home", new { Area = "Admin" });
                }
            }
            catch
            {
                return RedirectToAction("AdminLogin", "Account", new { Area = "Admin" });
            }
            return RedirectToAction("AdminLogin", "Account", new { Area = "Admin" });
        }



        public IActionResult AdminLogout()
        {
            try
            {
                HttpContext.SignOutAsync();
                HttpContext.Session.Remove("AccountId");
                return RedirectToAction("AdminLogin", "Account", new { Area = "Admin" });
            }
            catch
            {
                return RedirectToAction("AdminLogin", "Account", new { Area = "Admin" });
            }
        }

    }
}
