using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FirebaseService;

namespace DianaShop.Service.ReponseModel
{
    public class FirebaseRespondModel
    {
        public string ImageBase64 { get; set; }
        public double Temperature { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class FirebaseRoot
    {
        public CameraData camera { get; set; }
        public SensorData sensor { get; set; }
        public string time { get; set; }
    }

    public class CameraData
    {
        public ImageData image { get; set; }
    }

    public class ImageData
    {
        public string data { get; set; }
        public string error { get; set; }
    }

    public class SensorData
    {
        public TempData temp { get; set; }
    }

    public class TempData
    {
        public double data { get; set; }
        public string error { get; set; }
    }
}
