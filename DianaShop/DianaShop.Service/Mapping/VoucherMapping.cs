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
    public class VoucherMapping : Profile
    {
        public VoucherMapping()
        {
            //CreateMap<VoucherRequestModel, Voucher>();
            //CreateMap<Voucher, VoucherResponseModel>();

            //CreateMap<VoucherStorageRequestModel, VoucherStorage>();
            //CreateMap<VoucherStorage, VoucherStorageResponseModel>()
            //    .ForMember(dest => dest.Vouchers, opt => opt.MapFrom(src => src.Voucher.Select(v => new VoucherResponseModel
            //    {
            //        Id = v.Id,
            //        DiscountPercentage = v.DiscountPercentage,
            //        MinimumPurchase = v.MinimumPurchase,
            //        Quantity = v.Quantity,
            //        Value = v.Value,
            //        Status = v.Status
            //    }).ToList()));
            CreateMap<Voucher, VoucherResponseModel>();
            CreateMap<VoucherRequestModel, Voucher>();
        }
    }
}
