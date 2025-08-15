using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Interfaces
{
    public interface IKioskService
    {
        Task<IEnumerable<KioskRespondModel>> GetAllKiosks();
        Task<KioskRespondModel> CreateKiosk(KioskRequestModel request);
    }
}
