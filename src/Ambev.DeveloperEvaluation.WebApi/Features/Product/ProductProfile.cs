using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Application.Products;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Product
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductDto, CreateProductCommand>()
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating));

            CreateMap<ProductDto, UpdateProductCommand>()
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating));

            CreateMap<ProductDto, GetProductByIdCommand>();

            CreateMap<ProductDto, DeleteProductByIdCommand>();

            CreateMap<QueryParametersCommand, GetProductQueryCommand>()
                .ForMember(dest => dest.QueryParameters, opt => opt.MapFrom(src => src));

            CreateMap<ProductRatingCommand, ProductRatingDto>().ReverseMap();
        }
    }
}