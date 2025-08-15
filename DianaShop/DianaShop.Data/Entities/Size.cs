using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class Size : BaseEntity
    {
        public string SizeCode { get; set; }
        public int Capacity { get; set; }
        public virtual ICollection<OrderDetail> Details { get; set; }
    }
}
