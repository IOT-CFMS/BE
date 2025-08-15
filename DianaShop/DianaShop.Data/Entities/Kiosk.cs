using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class Kiosk : BaseEntity
    {
        public string KioskName { get; set; }
        public string Address { get; set; }
        public virtual ICollection<GPSLocation> Locations { get; set; }
        public virtual ICollection<KioskCamera> Cameras { get; set; }
        public virtual ICollection<SensorData> Sensors { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<KioskImage> Images { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

    }
}
