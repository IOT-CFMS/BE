using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel.QueryRequest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.RequestModel
{
    public class OrderRequestModel
    {
        [Required]
        public int UserID { get; set; }

    }

    public class CheckoutRequestModel
    {
        [Required]
        public int UserID { get; set; }
        public int PaymentMethodID { get; set; }
        public int? voucherID { get; set; }

        public IEnumerable<CheckoutDetailRequestModel> Details { get; set; }

    }
}
