using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class Image : BaseEntity
    {
        public string Base64Image { get; set; }
        public string ContentType { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<KioskImage> KioskImages { get; set; }
    }
}
