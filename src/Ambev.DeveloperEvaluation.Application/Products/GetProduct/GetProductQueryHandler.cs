using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Serilog;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQueryCommand, GetProductQueryDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<GetProductQueryDto> Handle(GetProductQueryCommand command, CancellationToken cancellationToken)
        {
            Log.Information("Handling GetProductQueryCommand with QueryParameters: {@QueryParameters}", command.QueryParameters);
            var query = _mapper.Map<QueryParameters>(command.QueryParameters);
            var products = await _productRepository.GetByQueryParameters(query, null, cancellationToken);

            if (products == null)
            {
                throw new ValidationException("Product with QueryParameters not found.");
            }

            var totalCount = await _productRepository.CountByQueryParametersAsync(query, cancellationToken);
            Log.Information("Total count of products matching query: {TotalCount}", totalCount);

            var productsResult = _mapper.Map<List<ProductDto>>(products);

            return new GetProductQueryDto
            {
                Products = productsResult,
                TotalCount = totalCount
            };
        }
    }
}
