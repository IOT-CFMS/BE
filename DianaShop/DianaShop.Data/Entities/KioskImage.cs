using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class KioskImage
    {
        public int KioskId { get; set; }
        public int ImageId { get; set; }
        public virtual Kiosk Kiosk { get; set; }
        public virtual Image Image { get; set; }
    }
}
