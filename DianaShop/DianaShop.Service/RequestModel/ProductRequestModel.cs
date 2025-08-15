using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.RequestModel
{
    public class ProductRequestModel
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public decimal? Price { get; set; }
        public bool Status { get; set; } // true: available, false: out of stock
        public List<IFormFile> ProductImageFiles { get; set; } = new List<IFormFile>(); // List of ProductImageIds
    }
}
