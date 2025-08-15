using AutoMapper;
using DianaShop.Data.Entities;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Mapping
{
    public class CartProductMapping : Profile
    {
        public CartProductMapping()
        {
            CreateMap<CartProductRequest, CartProduct>();

            CreateMap<CartProduct, CartProductResponse>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price ?? 0))
            .ForMember(dest => dest.ImageURL, opt => opt.MapFrom(src => src.Product.ProductImages.FirstOrDefault() != null ? src.Product.ProductImages.FirstOrDefault().Image.Base64Image : null));

        }
    }
}
