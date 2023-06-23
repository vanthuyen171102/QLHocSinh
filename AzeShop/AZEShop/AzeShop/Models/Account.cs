using System;
using System.Collections.Generic;
#nullable disable
namespace AzeShop.Models
{
    public partial class Account
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool Active { get; set; }
        public int RoleId { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Role Role { get; set; } = null!;
    }
}
