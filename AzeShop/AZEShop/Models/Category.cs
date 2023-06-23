using System;
using System.Collections.Generic;

namespace AzeShop.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int CatId { get; set; }
        public string CatName { get; set; } = null!;
        public string? Description { get; set; }
        public bool Published { get; set; }
        public string? Thumb { get; set; }
        public string? Title { get; set; }
        public string? Alias { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
