using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.ReponseModel
{
    public class CategoryResponseModel
    {
        public int Id { get; set; } // Unique identifier for the category
        public string Name { get; set; } // Example: "Cleanser", "Toner", etc.
        public string? Description { get; set; } // Description of the category (optional)
    }
}
