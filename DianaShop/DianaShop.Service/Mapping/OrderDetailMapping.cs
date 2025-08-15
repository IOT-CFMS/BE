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
    public class OrderDetailMapping : Profile
    {
        public OrderDetailMapping()
        {
            CreateMap<OrderDetailRequestModel, OrderDetail>();
            CreateMap<OrderDetail, OrderDetailResponseModel>()
                .ForMember(dest => dest.ProductName, otp => otp.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.UnitPrice, otp => otp.MapFrom(src => src.Product.Price ?? 0));
            CreateMap<CheckoutDetailRequestModel, OrderDetail>();
        }
    }
}
