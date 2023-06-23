using AzeShop.Models;

namespace AzeShop.ModelViews
{
    public class CartItem
    {
        public Product product { set; get; }

        public int quantity { set; get; }
    }
}
