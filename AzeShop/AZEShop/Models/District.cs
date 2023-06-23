using System;
using System.Collections.Generic;

namespace AzeShop.Models
{
    public partial class District
    {
        public District()
        {
            Wards = new HashSet<Ward>();
        }

        public int DistrictId { get; set; }
        public int CityId { get; set; }
        public string DistrictName { get; set; } = null!;

        public virtual City City { get; set; } = null!;
        public virtual ICollection<Ward> Wards { get; set; }
    }
}
