using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.RequestModel
{
    public class PaymentRequestModel
    {
        public int OrderID { get; set; }
        public int PaymentMethodID { get; set; }
    }

}
