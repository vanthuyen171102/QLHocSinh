using System;
using System.Collections.Generic;
#nullable disable
namespace AzeShop.Models
{
    public partial class Product
    {
        public Product()
        {
            Inventories = new HashSet<Inventory>();
            OrderDetails = new HashSet<OrderDetail>();
            Receipts = new HashSet<Receipt>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? Description { get; set; }
        public int CatId { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public int UnitsInStock { get; set; }
        public string Thumb { get; set; } = null!;
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool BestSeller { get; set; }
        public bool HomeFlag { get; set; }
        public bool Active { get; set; }
        public string? ShortDesc { get; set; }
        public string? Title { get; set; }
        public string? Alias { get; set; }
        public string? MetaDesc { get; set; }
        public string? MetaKey { get; set; }
        public string? Tags { get; set; }

        public virtual Category Cat { get; set; } = null!;
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Receipt> Receipts { get; set; }
    }
}
