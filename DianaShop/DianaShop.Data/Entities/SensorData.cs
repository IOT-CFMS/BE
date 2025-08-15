using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class SensorData : BaseEntity
    {
        public int KioskID { get; set; }
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public float Brightness { get; set; }
        public float Weight { get; set; }
        public virtual Kiosk Kiosk { get; set; }
    }
}
