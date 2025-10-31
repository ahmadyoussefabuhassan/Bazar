using AutoMapper;
using Bazar.Application.DTOS;
using Bazar.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazar.Application.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            // User Mapping
            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Advertisements, opt => opt.MapFrom(src => src.Advertisements));
            // Product Mapping 
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ReverseMap()
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Advertisements, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore());


            // Images Mapping
            CreateMap<Images, ImagesDto>()
                .ReverseMap()
                .ForMember(dest => dest.Product, opt => opt.Ignore());

            // Advertisements Mapping 
            CreateMap<Advertisements, AdvertisementsDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
                .ForMember(dest => dest.UserPhone, opt => opt.MapFrom(src => src.User.PhoneNumber))
                .ReverseMap()
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Products, opt => opt.Ignore());

            // Category Mapping 
            CreateMap<Category, CategoryDto>();

            
         
        }
    }
}

