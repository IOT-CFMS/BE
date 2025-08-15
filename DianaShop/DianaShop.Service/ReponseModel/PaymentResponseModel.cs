using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.ReponseModel
{
    public class PaymentResponseModel
    {
        public int Id { get; set; }
        public int OrderID { get; set; }
        public int PaymentMethodID { get; set; }
        public string MethodName { get; set; }
        public decimal FinalAmount { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }

    }
}
