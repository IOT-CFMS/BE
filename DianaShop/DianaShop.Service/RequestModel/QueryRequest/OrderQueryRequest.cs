using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.RequestModel.QueryRequest
{
    public class OrderQueryRequest : BaseQuery
    {
        public int? UserID { get; set; }
        public int? VoucherID { get; set; }
        public int? Quantity { get; set; }
        public string? Address { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public DateTime? MinCreatedDate { get; set; }
        public DateTime? MaxCreatedDate { get; set; }
        public DateTime? MinUpdateDate { get; set; }
        public DateTime? MaxUpdateDate { get; set; }
        public int? StatusId { get; set; }
        public OrderSort SortBy { get; set; } = OrderSort.Default;
        public bool Ascending { get; set; } = true;
    }

    public enum OrderSort
    {
        [Display(Name = "Default")]
        Default, // Sort by ID
        [Display(Name = "Quantity")]
        Quantity,
        [Display(Name = "Final Amount")]
        FinalAmount
    }
}
