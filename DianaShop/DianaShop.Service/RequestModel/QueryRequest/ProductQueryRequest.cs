using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.RequestModel.QueryRequest
{
    public class ProductQueryRequest : BaseQuery
    {
        public string? Name { get; set; }
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public ProductSort SortBy { get; set; } = ProductSort.Default;
        public bool Ascending { get; set; } = true;
    }
    public enum ProductSort
    {
        [Display(Name = "Default")]
        Default, //id
        [Display(Name = "Name")]
        Name,
        [Display(Name = "Price")]
        Price
    }
}

