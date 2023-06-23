using System;
using System.Collections.Generic;
#nullable disable
namespace AzeShop.Models
{
    public partial class Receipt
    {
        public int ReceiptId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public DateTime? DateCreated { get; set; }
        public int Amount { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}
