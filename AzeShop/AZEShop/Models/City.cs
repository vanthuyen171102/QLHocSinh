using System;
using System.Collections.Generic;

namespace AzeShop.Models
{
    public partial class City
    {
        public City()
        {
            Customers = new HashSet<Customer>();
            Districts = new HashSet<District>();
        }

        public int CityId { get; set; }
        public string CityName { get; set; } = null!;
        public decimal DeliveryPrice { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<District> Districts { get; set; }
    }
}
