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
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<ProductRequestModel, Product>();
            CreateMap<Product, Product>();
            CreateMap<Product, ProductResponseModel>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.ProductImage, opt => opt.MapFrom(src => src.ProductImages.Any() ? src.ProductImages.OrderByDescending(pi => pi.ImageId).FirstOrDefault().Image.Base64Image : null ))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDelete));
        }
    }
}
