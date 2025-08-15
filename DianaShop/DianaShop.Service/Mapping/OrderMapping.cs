using AutoMapper;
using DianaShop.Data.Entities;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Mapping
{
    public class OrderMapping : Profile
    {
        public OrderMapping()
        {
            CreateMap<OrderRequestModel, Order>();
            CreateMap<Order, OrderResponseModel>()
                .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.Details))
                .ForMember(dest => dest.UserName, otp => otp.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.StatusName, otp => otp.MapFrom(src => src.StageStatus.StatusName));
            CreateMap<CheckoutRequestModel, Order>();
        }
    }

}
