using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Interfaces
{
    public interface IKioskCameraService
    {
        Task<string> ManualAddCamera(string path);
    }
}
