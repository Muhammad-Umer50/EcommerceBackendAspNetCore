using AutoMapper;
using ECommerceStore.Dtos.CartDtos;
using ECommerceStore.Dtos.CategoryDtos;
using ECommerceStore.Dtos.OrderDtos;
using ECommerceStore.Dtos.ProductsDtos;
using ECommerceStore.Models;

namespace ECommerceStore.AutoMaprConfiguration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Product, GetProductDto>();

            CreateMap<ProductImage, ProductImageDto>()
            .ForMember(
                dest => dest.Url,
                opt => opt.MapFrom(src => src.ImageUrl)
            );

            CreateMap<Category, GetCategoriesDto>()
            .ForMember(
                dest => dest.Products,
                opt => opt.MapFrom(src => src.Products)
            );
            CreateMap<Category, GetCategoriesNamesDto>();
            CreateMap<Product, SearchSuggestionDto>();
            CreateMap<Cart, CreateCartDto>();
            CreateMap<CreateCartItemsDto, CartItem>();
            CreateMap<Cart, GetCartDto>();
            CreateMap<CartItem, GetCartItemsDto>()
            .ForMember(
                dest => dest.CartProductDto,
                opt => opt.MapFrom(src => src.Product)
            );
            CreateMap<Product, CartProductDto>();
            CreateMap<Order, CreateOrderDto>().ReverseMap();
            CreateMap<Order, GetOrderDto>().ForMember(
            dest => dest.ShippingAddressDetails,
            opt => opt.MapFrom(src => src.ShippingAddressDetails));
            CreateMap<OrderItems, GetOrderItemsDto>().ReverseMap();
            CreateMap<ShippingAddressDetails, ShippingAddressDetailsDto>().ReverseMap();
        }
    }
}
