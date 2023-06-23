using System;
using System.Collections.Generic;

namespace AzeShop.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int CustomerId { get; set; }
        public string? Avatar { get; set; }
        public string FullName { get; set; } = null!;
        public DateTime? Birthday { get; set; }
        public string? Address { get; set; }
        public int CityId { get; set; }
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public DateTime? CreateDate { get; set; }
        public string Password { get; set; } = null!;
        public DateTime? LastLogin { get; set; }
        public bool Active { get; set; }

        public virtual City City { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; }
    }
}
