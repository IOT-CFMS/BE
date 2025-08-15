using System;
using AutoMapper;
using DianaShop.Data.Entities;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Mapping
{
    public class ImageMapping : Profile
    {
        public ImageMapping()
        {
            CreateMap<Image, ImageResponseModel>()
                //.ForMember(dest => dest.PostIds, opt => opt.MapFrom(src => src.PostImages.Select(pi => pi.PostId)))
                .ForMember(dest => dest.ProductIds, opt => opt.MapFrom(src => src.ProductImages.Select(pi => pi.ProductId)))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDelete));

        }
    }
}
