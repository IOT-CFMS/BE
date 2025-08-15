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
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            // Mapping User -> UserResponseModel
            CreateMap<User, UserResponseModel>()
                //.ForMember(dest => dest.SkinTypeName, opt => opt.MapFrom(src => src.SkinType.Name))
                .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.UserRoles.Select(ur =>
                    new UserRoleResponseNameModel
                    {
                        //UserId = ur.UserId,
                        RoleName = ur.Role != null ? ur.Role.Name : string.Empty
                    })));
            //.ForMember(dest => dest.Cart, opt => opt.Ignore()) // Tránh vòng lặp
            //.ForMember(dest => dest.FeedBacks, opt => opt.Ignore()); // Tránh vòng lặp

            // Mapping UserCreateRequest -> User
            CreateMap<UserCreateRequest, User>()
                .ForMember(dest => dest.Token, opt => opt.Ignore())
                .ForMember(dest => dest.Cart, opt => opt.Ignore())
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            // Mapping UserUpdateRequest -> User
            CreateMap<UserUpdateRequest, User>()
                .ForMember(dest => dest.Token, opt => opt.Ignore())
                .ForMember(dest => dest.Cart, opt => opt.Ignore())
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
                .ForMember(dest => dest.Orders, opt => opt.Ignore())
                .ForMember(dest => dest.VnPay, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        }
    }
}
