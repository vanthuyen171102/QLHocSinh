using AspNetCoreHero.ToastNotification.Abstractions;
using AzeShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AzeShop.Controllers
{
    public class CartController : Controller
    {

        private readonly ILogger<CartController> _logger;

        private readonly dbAZEShopContext _context;

        public INotyfService _notyfService { get; }

        public CartController(ILogger<CartController> logger, dbAZEShopContext context, INotyfService notyfService)
        {
            _logger = logger;
            _context = context;
            _notyfService = notyfService;
        }

        [Route("/cart", Name = "cart")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LoadCart()
        {
            return PartialView("Cart");
        }

        /// Thêm sản phẩm vào cart
        [Route("/addcart/{productid:int}.{quantity:int?}", Name = "addcart")]
        public IActionResult AddToCart([FromRoute] int productid, [FromRoute] int? quantity)
        {
            var product = _context.Products
                .Where(p => p.ProductId == productid)
                .FirstOrDefault();
            if (product == null)
            {
                _notyfService.Error("Không tìm thấy sản phẩm");
                return Json(new { status = "error" });
            }
            // Xử lý đưa vào Cart ...
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.product.ProductId == productid);
            if (cartitem != null)
            {
                if(quantity.HasValue)
                {
                    cartitem.quantity += quantity.Value;
                }
                // Đã tồn tại, tăng thêm 1
                else
                    cartitem.quantity++;
                if (product.UnitsInStock < cartitem.quantity)
                {
                    _notyfService.Error("Tồn kho không đủ");
                    return Json(new { status = "error" });
                }
            }
            else
            {
                if(!quantity.HasValue)
                {
                    quantity = 1;
                }
                if (product.UnitsInStock < quantity)
                {
                    _notyfService.Error("Tồn kho không đủ");
                    return Json(new { status = "error" });
                }
                cart.Add(new AzeShop.ModelViews.CartItem() { quantity = quantity.HasValue ? quantity.Value : 1, product = product });
            }
            
            // Lưu cart vào Session
            SaveCartSession(cart);
            return Json(new { status = "success" });
        }



        [Route("/removecart/{productid:int}", Name = "removecart")]
        public IActionResult RemoveCart(int productid)
        {
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.product.ProductId == productid);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cart.Remove(cartitem);
            }
            _notyfService.Success("Xoá sản phẩm khỏi giỏ hàng thành công");
            SaveCartSession(cart);
            return Ok();
        }


        /// Cập nhật
        [Route("/updatecart", Name = "updatecart")]
        [HttpPost]
        public IActionResult UpdateCart([FromForm] int productid, [FromForm] int quantity)
        {
            // Cập nhật Cart thay đổi số lượng quantity ...
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.product.ProductId == productid);
            
            if (cartitem != null)
            {
                if (cartitem.product.UnitsInStock < quantity)
                {
                    _notyfService.Error("Số lượng tồn kho không đủ");
                    return Json(new { status = "error" });
                }
                cartitem.quantity = quantity;
            }
            
            _notyfService.Success("Cập nhật giỏ hàng thành công");
            SaveCartSession(cart);
            // Trả về mã thành công (không có nội dung gì - chỉ để Ajax gọi)
            return Json(new { status = "error" });
        }


        // Key lưu chuỗi json của Cart
        public const string CARTKEY = "cart";

        // Lấy cart từ Session (danh sách CartItem)
        public List<AzeShop.ModelViews.CartItem> GetCartItems()
        {
            var session = HttpContext.Session;
            string jsoncart = session.GetString(CARTKEY);
            if (jsoncart != null)
            {
                return JsonConvert.DeserializeObject<List<AzeShop.ModelViews.CartItem>>(jsoncart);
            }
            return new List<AzeShop.ModelViews.CartItem>();
        }

        // Xóa cart khỏi session
        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(CARTKEY);
        }

        // Lưu Cart (Danh sách CartItem) vào session
        void SaveCartSession(List<AzeShop.ModelViews.CartItem> ls)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(ls);
            session.SetString(CARTKEY, jsoncart);
        }





    }
}
