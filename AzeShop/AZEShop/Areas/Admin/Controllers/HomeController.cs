using AspNetCoreHero.ToastNotification.Abstractions;
using AzeShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Composition;

namespace AzeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin,Sale")]
    public class HomeController : Controller
    {
        private readonly dbAZEShopContext _context;

        public HomeController(dbAZEShopContext context)
        {
            _context = context;
        }

        [Route("admin", Name = "AdminIndex")]
        public IActionResult Index()
        {
            ViewBag.TotalRevenue = _context.Inventories.Sum(x=>x.ExportMoney);
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            decimal revenue_this_year=0;
            decimal revenue_last_year=0;

            //Lay du lieu doanh thu nam nay
            for (int i = 1; i <= month; i++)
            {
                var month_and_year = i.ToString() + "/" + year.ToString();
                var report_by_month = _context.Inventories.Where(x => x.MonthAndYear == month_and_year);
                if (report_by_month != null)
                    revenue_this_year += report_by_month.Sum(x => x.ExportMoney);
            }

            //Lay du lieu doanh thu cung ky nam ngoai
            for (int i = 1; i <= month; i++)
            {
                var month_and_year = i.ToString() + "/" + (year-1).ToString();
                var report_by_month = _context.Inventories.Where(x => x.MonthAndYear == month_and_year);
                if (report_by_month != null)
                    revenue_last_year += report_by_month.Sum(x => x.ExportMoney);
            }
            if (revenue_last_year == 0)
            {
                ViewBag.Growth = ">999";
            }
            else
            {
                ViewBag.Growth = Math.Round((revenue_this_year - revenue_last_year) / revenue_last_year * 100, 2).ToString();
            }
            ViewBag.TotalOrder = _context.Orders.Count();
            ViewBag.TotalCustomer = _context.Customers.Count();
            ViewBag.ImportProductList = _context.Products.Where(x => x.UnitsInStock<10)
                                                        .OrderBy(x => x.UnitsInStock)
                                                         .Take(8).ToList();


            return View();
        }

        [HttpGet]
        public List<decimal> GetReportByYear(int year)
        {
            var report = new List<decimal>();
            for (int i = 1; i <= 12; i++)
            {
                var month_and_year = i.ToString() + "/" + year.ToString();
                var report_by_month = _context.Inventories.Where(x => x.MonthAndYear == month_and_year);
                if (report_by_month == null)
                    report.Add(0);
                else
                    report.Add(report_by_month.Sum(x=>x.ExportMoney));
            }
            return report;
        }

        [HttpGet]
        public List<int> GetReportCustomer()
        {
            var report = new List<int>();
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var totalCustomer = _context.Customers.Count();
            var newCustomer = _context.Customers.Where(x => x.CreateDate.Value.Month == month && x.CreateDate.Value.Year == year).Count();
            var returnCustomer = _context.Customers.Where(x => x.LastLogin.Value.Month == month && x.LastLogin.Value.Year == year).Count();
            report.Add(newCustomer);
            report.Add(returnCustomer);
            report.Add(totalCustomer - newCustomer - returnCustomer);
            return report;
        }
    }
}

