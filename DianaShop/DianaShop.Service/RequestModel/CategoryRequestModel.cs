using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.RequestModel
{
    public class CategoryRequestModel
    {
        [Required]
        public string Name { get; set; } // Example: "Cleanser", "Toner", etc.
        public string? Description { get; set; } // Optional description of the category
    }

}
