using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class Schedule : BaseEntity
    {
        public int UserID { get; set; }
        public int KioskID { get; set; }
        public DateOnly ShiftDate { get; set; }
        public int ShiftSlot { get; set; }
        public int AddressID { get; set; }

        public virtual User User { get; set; }
        public virtual Kiosk Kiosk { get; set; }
        public virtual KioskAddress Address { get; set; }
    }
}
