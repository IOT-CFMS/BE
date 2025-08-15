using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class VnPayTransaction
    {
        public int TransactionId { get; set; }
        public int UserId { get; set; }
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderDescription { get; set; }
        public string OrderId { get; set; }
        public string Token { get; set; }
        public string VnPayResponseCode { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual User? User { get; set; }
        public virtual Payment? Payment { get; set; }
    }
}
