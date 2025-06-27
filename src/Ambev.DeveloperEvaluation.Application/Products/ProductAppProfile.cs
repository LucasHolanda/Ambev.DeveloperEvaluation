using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products
{
    internal class ProductAppProfile : Profile
    {
        public ProductAppProfile()
        {
            CreateMap<QueryParametersCommand, GetProductQueryCommand>()
                .ForMember(dest => dest.QueryParameters, opt => opt.MapFrom(src => src));

            CreateMap<ProductRatingCommand, ProductRatingDto>().ReverseMap();

            CreateMap<QueryParametersCommand, GetProductQueryCommand>()
                .ForMember(dest => dest.QueryParameters, opt => opt.MapFrom(src => src));

            CreateMap<QueryParameters, QueryParametersCommand>().ReverseMap();

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating));

            CreateMap<CreateProductCommand, Product>()
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating));

            CreateMap<UpdateProductCommand, Product>()
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating));

            CreateMap<ProductRatingCommand, ProductRating>().ReverseMap();

            CreateMap<ProductRatingDto, ProductRating>().ReverseMap();
        }
    }
}
