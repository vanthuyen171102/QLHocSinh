using AzeShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace AzeShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly dbAZEShopContext _context;


        public HomeController(ILogger<HomeController> logger, dbAZEShopContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var lsCategories = _context.Categories.AsNoTracking()
                                .ToList();
            var lsProducts = _context.Products.AsNoTracking()
                .Where(x => x.Active == true && x.HomeFlag == true && x.BestSeller == true)
                .Include(x => x.Cat)
                .OrderByDescending(x => x.DateCreated)
                .ToList();
            ViewBag.Categories = lsCategories;
            ViewBag.Products = lsProducts;
            return View();
        }

        public const string CARTKEY = "cart";

        public IActionResult LoadCartPartial()
        {
            return PartialView("_CartPartial");
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}