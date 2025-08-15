using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.ReponseModel
{
    public class ProductResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Category { get; set; } // Tên của danh mục
        public decimal? Price { get; set; }
        public int StockQuantity { get; set; }
        public bool Status { get; set; } // true: available, false: out of stock
        public bool IsDeleted { get; set; }
        public string? ProductImage { get; set; } // List of image URLs
    }

    public class ProductResponseCart
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
    }
}
