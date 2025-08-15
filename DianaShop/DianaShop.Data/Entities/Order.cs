using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class Order : BaseEntity
    {
        public int UserID { get; set; }
        public int KioskID { get; set; }
        public int Quantity { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public int? VoucherID { get; set; }
        public decimal? FinalAmount { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? StatusId { get; set; }

        public User User { get; set; }
        public Kiosk Kiosk { get; set; }
        public virtual ICollection<OrderDetail> Details { get; set; }
        public Payment Payment { get; set; }
        public StageStatus StageStatus { get; set; }
        public Voucher? Voucher { get; set; }
    }
}
