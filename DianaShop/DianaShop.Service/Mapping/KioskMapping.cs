using AutoMapper;
using DianaShop.Data.Entities;
using DianaShop.Service.ReponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Mapping
{
    public class KioskMapping : Profile
    {
        public KioskMapping()
        {
            CreateMap<Kiosk, KioskRespondModel>();
        }
    }
}
