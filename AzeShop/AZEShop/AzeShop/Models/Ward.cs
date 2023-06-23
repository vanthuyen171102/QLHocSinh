using System;
using System.Collections.Generic;

namespace AzeShop.Models
{
    public partial class Ward
    {
        public Ward()
        {
            Orders = new HashSet<Order>();
        }

        public int WardId { get; set; }
        public int DistrictId { get; set; }
        public string WardName { get; set; } = null!;

        public virtual District District { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; }
    }
}
