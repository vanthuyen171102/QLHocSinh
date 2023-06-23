using System;
using System.Collections.Generic;

namespace AzeShop.Models
{
    public partial class Inventory
    {
        public string MonthAndYear { get; set; } = null!;
        public int ProductId { get; set; }
        public int ImportAmount { get; set; }
        public int ExportAmount { get; set; }
        public decimal ImportMoney { get; set; }
        public decimal ExportMoney { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}
