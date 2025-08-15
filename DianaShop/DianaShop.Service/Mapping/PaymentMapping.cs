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
    public class PaymentMapping : Profile
    {
        public PaymentMapping()
        {
            CreateMap<PaymentRequestModel, Payment>();
            CreateMap<Payment, PaymentResponseModel>()
                .ForMember(dest => dest.MethodName, otp => otp.MapFrom(src => src.PaymentMethod.MethodName))
                .ForMember(dest => dest.StatusName, otp => otp.MapFrom(src => src.StageStatus.StatusName));
        }
    }
}
