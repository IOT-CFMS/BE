using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.RequestModel
{
    public class CartRequestModel
    {
        [Required]
        public int UserID { get; set; }

        //[Required]
        //[Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        //public int Quantity { get; set; }

        //public List<int> ProductIDs { get; set; } = new List<int>();
    }

}
