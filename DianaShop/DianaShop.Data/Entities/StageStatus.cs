using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class StageStatus : BaseEntity
    {
        public String StatusName { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
