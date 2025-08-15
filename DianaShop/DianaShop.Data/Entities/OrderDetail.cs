using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class OrderDetail : BaseEntity
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int SizeId {  get; set; }
        public Size Size { get; set; }

        public int Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
