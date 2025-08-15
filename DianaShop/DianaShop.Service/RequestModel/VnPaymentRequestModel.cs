using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.RequestModel
{
    public class VnPaymentRequestModel
    {
        public int OrderId { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UserId { get; set; }  // Added UserId
        public int PaymentId { get; set; }
    }
}
