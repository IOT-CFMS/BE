using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class Voucher :BaseEntity
    {
        public double? DiscountPercentage { get; set; }
        public double? MinimumPurchase { get; set; }
        public double? Value { get; set; }
        public bool? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

    }
}
