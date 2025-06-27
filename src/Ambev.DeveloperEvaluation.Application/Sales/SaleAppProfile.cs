using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales
{
    public class SaleAppProfile : Profile
    {
        public SaleAppProfile()
        {
            CreateMap<CreateSaleCommand, SaleDto>().ReverseMap();

            CreateMap<CreateSaleItemCommand, SaleItemDto>().ReverseMap();

            CreateMap<QueryParametersCommand, GetSalesQueryCommand>()
                .ForMember(dest => dest.QueryParameters, opt => opt.MapFrom(src => src));

            CreateMap<QueryParameters, QueryParametersCommand>().ReverseMap();

            CreateMap<CreateSaleCommand, Sale>()
                .ForMember(uc => uc.SaleItems, opt => opt.MapFrom(src => src.SaleItems))
                .ReverseMap();

            CreateMap<CreateSaleItemCommand, SaleItem>().ReverseMap();

            CreateMap<SaleItemDto, SaleItem>().ReverseMap();

            CreateMap<Sale, SaleDto>()
                .ForMember(uc => uc.SaleItems, opt => opt.MapFrom(src => src.SaleItems))
                .ReverseMap();
        }
    }
}
