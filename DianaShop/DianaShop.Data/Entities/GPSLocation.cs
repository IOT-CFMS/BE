namespace DianaShop.Data.Entities
{
    public class GPSLocation : BaseEntity
    {
        public int KioskId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime RecordedAt { get; set; }
        public virtual Kiosk Kiosk { get; set; }
    }
}
