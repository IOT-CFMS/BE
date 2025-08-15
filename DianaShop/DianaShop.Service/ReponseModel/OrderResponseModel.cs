using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.ReponseModel
{
    public class OrderResponseModel
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public int? VoucherID { get; set; }
        public string UserName { get; set; }
        public int Quantity { get; set; }
        public string Address { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? ShipAmount { get; set; }
        public decimal? ReducedAmount { get; set; }
        public decimal? FinalAmount { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public IEnumerable<OrderDetailResponseModel> Details { get; set; }
    }

}
