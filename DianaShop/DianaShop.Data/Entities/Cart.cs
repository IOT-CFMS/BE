using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class Cart : BaseEntity
    {
        public int UserID { get; set; }
        public int Quantity { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<CartProduct> CartProducts { get; set; }
    }

}
