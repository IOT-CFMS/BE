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
    public class CartMapping : Profile
    {

        public CartMapping()
        {
            CreateMap<CartRequestModel, Cart>();

            // Mapping từ Cart -> CartResponse
            CreateMap<Cart, CartResponseModel>()
            .ForMember(dest => dest.CartProducts, opt => opt.MapFrom(src => src.CartProducts))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.CartProducts.Sum(cp => cp.Quantity)));

            // Mapping từ CartProduct -> CartProductResponse
            CreateMap<CartProduct, CartProductResponseModel>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));
        }
    }
}
