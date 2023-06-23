using System;
using System.Collections.Generic;

namespace AzeShop.Models
{
    public partial class Page
    {
        public string PageId { get; set; } = null!;
        public string PageName { get; set; } = null!;
        public string? Contents { get; set; }
        public string Thumb { get; set; } = null!;
        public bool Published { get; set; }
        public string? Tittle { get; set; }
        public string? Alias { get; set; }
        public DateTime CreateDate { get; set; }
        public int? Ordering { get; set; }
        public string? MetaDesc { get; set; }
        public string? MetaKey { get; set; }
    }
}
