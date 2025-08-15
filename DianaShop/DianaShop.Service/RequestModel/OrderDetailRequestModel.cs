using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.RequestModel
{
    public class OrderDetailRequestModel
    {
        public int OrderID { get; set; }
        public int ProductId { get; set; }
        public int SizeID { get; set; }
        public int Quantity { get; set; }
    }

    public class CheckoutDetailRequestModel
    {
        public int ProductId { get; set; }
        public int SizeID { get; set; }
        public int Quantity { get; set; }
    }
}
