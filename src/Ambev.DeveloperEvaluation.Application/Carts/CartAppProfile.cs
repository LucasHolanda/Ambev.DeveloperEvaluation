using Ambev.DeveloperEvaluation.Application.Carts;
using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products
{
    public class CartAppProfile : Profile
    {
        public CartAppProfile()
        {
            CreateMap<CreateCartCommand, Cart>()
                .ForMember(uc => uc.CartProducts, opt => opt.MapFrom(src => src.CartProducts))
                .ReverseMap();

            CreateMap<UpdateCartCommand, Cart>()
                .ForMember(uc => uc.CartProducts, opt => opt.MapFrom(src => src.CartProducts))
                .ReverseMap();

            CreateMap<CartProductCommand, CartProduct>().ReverseMap();

            CreateMap<CartProductDto, CartProduct>().ReverseMap();

            CreateMap<Cart, CartDto>()
                .ForMember(uc => uc.CartProducts, opt => opt.MapFrom(src => src.CartProducts))
                .ReverseMap();
        }
    }
}
