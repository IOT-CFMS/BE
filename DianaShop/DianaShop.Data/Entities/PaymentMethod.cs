using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class PaymentMethod : BaseEntity
    {
        public string MethodName {  get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
