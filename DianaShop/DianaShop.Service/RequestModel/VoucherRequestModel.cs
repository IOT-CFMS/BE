using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.RequestModel
{
    public class VoucherRequestModel
    {
        public double? DiscountPercentage { get; set; }
        public double? MinimumPurchase { get; set; }
        public double? Value { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Status { get; set; }
    }
    public class CreateVoucherRequest
    {
        public double? DiscountPercentage { get; set; }
        public double? MinimumPurchase { get; set; }
        public double? Value { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Status { get; set; }
    }

    public class UpdateVoucherRequest
    {
        public double? DiscountPercentage { get; set; }
        public double? MinimumPurchase { get; set; }
        public double? Value { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Status { get; set; }
    }

}
