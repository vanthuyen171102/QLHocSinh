using System;
using System.Collections.Generic;

#nullable disable
namespace AzeShop.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string FullName { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public string Phone { get; set; } = null!;
        public int TransactStatusId { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? Note { get; set; }
        public string Address { get; set; } = null!;
        public int WardId { get; set; }
        public decimal TotalMoney { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual TransactStatus TransactStatus { get; set; } = null!;
        public virtual Ward Ward { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
