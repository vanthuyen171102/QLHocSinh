using AzeShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using PagedList.Core.Mvc;
using System.Drawing.Printing;

namespace AzeShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly dbAZEShopContext _context;
        public ProductController(dbAZEShopContext context)
        {
            _context = context;
        }

        [Route("/collections/{alias?}")]
        public IActionResult Index(string? alias)
        {
            ViewBag.CatList = _context.Categories.AsNoTracking().ToList();
            if(alias == null)
            {
                ViewBag.CurrentCat = null;
                ViewBag.Alias = true;
                return View(true);
            }
            else
            {
                var danhmuc = _context.Categories.AsNoTracking().SingleOrDefault(x => x.Alias == alias);
                if (danhmuc == null)
                {
                    return View(false);
                }
                ViewBag.Alias = false;
                ViewBag.CurrentCat = danhmuc;
            }

            return View(true);
        }

        public IActionResult Shop(string? sortOrder, string? alias, string? searchString, decimal? minPrice, decimal? maxPrice, int page = 1, int pageSize = 3)
        {
            var lsProduct = _context.Products
               .AsNoTracking()
               .Include(x => x.Cat)
               .OrderBy(x => x.ProductId);
            if (alias != null)
            {
                var danhmuc = _context.Categories.AsNoTracking().SingleOrDefault(x => x.Alias == alias);
                if (danhmuc == null)
                {
                    return View(null);
                }
                lsProduct = lsProduct.Where(x => x.CatId == danhmuc.CatId).OrderByDescending(x => x.ProductId);
                ViewBag.CurrentCat = danhmuc;
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                lsProduct = lsProduct.Where(x => x.ProductName.Contains(searchString)).OrderBy(x => x.ProductId);
            }

            if(minPrice != null && maxPrice != null)
            {
                lsProduct = lsProduct.Where(x => ((100 - x.Discount) * x.Price / 100) >= minPrice && ((100 - x.Discount) * x.Price / 100) <= maxPrice).OrderBy(x => x.ProductId);
            }
            switch (sortOrder)
            {
                case "Price":
                    lsProduct = lsProduct.OrderBy(x => (100-x.Discount)* x.Price);
                    break;
                case "Price_Desc":
                    lsProduct = lsProduct.OrderByDescending(x => (100 - x.Discount) * x.Price);
                    break;
                case "Name":
                    lsProduct = lsProduct.OrderBy(x => x.ProductName);
                    break;
                case "Name_Desc":
                    lsProduct = lsProduct.OrderByDescending(x => x.ProductName);
                    break;
                case "Oldest":
                    lsProduct = lsProduct.OrderBy(x => x.DateCreated);
                    break;
                case "Newest":
                    lsProduct = lsProduct.OrderByDescending(x => x.DateCreated);
                    break;  
                default:
                    break;
            }
           
            PagedList<Product> models = new PagedList<Product>(lsProduct, page, pageSize);
            ViewBag.CurrentPage = page;
            if (lsProduct.Count() % pageSize != 0)
            {
                ViewBag.CountPage = lsProduct.Count() / pageSize +1;
            }
            else
            {
                ViewBag.CountPage = lsProduct.Count() / pageSize;
            }
            return PartialView("Shop", models);
        }


        [Route("/products/{Alias}-{id}", Name = ("ProductDetails"))]
        public IActionResult Details(int id)
        {
            try
            {
                var product = _context.Products.Include(x => x.Cat).FirstOrDefault(x => x.ProductId == id);
                if (product == null)
                {
                    return RedirectToAction("Index");
                }
                var lsProduct = _context.Products
                    .AsNoTracking()
                    .Where(x => x.CatId == product.CatId && x.ProductId != id && x.Active == true)
                    .Take(4)
                    .Include(x => x.Cat)
                    .OrderByDescending(x => x.DateCreated)
                    .ToList();
                ViewBag.Products = lsProduct;
                return View(product);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
