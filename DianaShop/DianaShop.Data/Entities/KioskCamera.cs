using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class KioskCamera : BaseEntity
    {
        public int KiodID { get; set; }
        public string Base64Image { get; set; }
        public DateTime? CreatedAt { get; set; }
        
        public virtual Kiosk Kiosk { get; set; }
    }
}
