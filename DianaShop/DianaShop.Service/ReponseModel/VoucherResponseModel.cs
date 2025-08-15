using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.ReponseModel
{
    public class VoucherResponseModel
    {
        public int Id { get; set; }
        public double? DiscountPercentage { get; set; }
        public double? MinimumPurchase { get; set; }
        public double? Value { get; set; }
        public bool? Status { get; set; }
    }
}
