using AutoMapper;
using Bazar.Application.DTOS;
using Bazar.Application.DTOS.Product;
using Bazar.Domain.Entites;

namespace Bazar.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User Mappings
            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email)); // استخدام الايميل كاسم مستخدم

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.JoinedDate, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));


            CreateMap<User, UserProfileDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.JoinedDate, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

            CreateMap<Category, CategoryDto>()
               .ReverseMap();
            CreateMap<CreateCategoryDto, Category>();

            CreateMap<Images, ImagesDto>().ReverseMap();

            // Product Mappings
            CreateMap<CreateProductDto, Product>();

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
                .ForMember(dest => dest.SellerName, opt => opt.MapFrom(src => src.User != null ? $"{src.User.FirstName} {src.User.LastName}" : null))
                .ForMember(dest => dest.Condition, opt => opt.MapFrom(src => src.Condition.ToString())); // تحويل الـ Enum لنص

            CreateMap<Product, UserProductSummaryDto>()
                .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.Images.FirstOrDefault(x => x.IsMain) != null ? src.Images.FirstOrDefault(x => x.IsMain).FilePath : null));
        }
    }
}