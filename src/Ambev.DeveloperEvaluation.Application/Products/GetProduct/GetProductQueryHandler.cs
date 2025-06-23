using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQueryCommand, GetProductQueryResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<GetProductQueryResult> Handle(GetProductQueryCommand command, CancellationToken cancellationToken)
        {
            var query = _mapper.Map<QueryParameters>(command.QueryParameters);
            var products = await _productRepository.GetByQueryParameters(query, cancellationToken);

            if (products == null)
            {
                throw new KeyNotFoundException("Product with QueryParameters not found.");
            }

            var totalCount = await _productRepository.CountAsync(query, cancellationToken);

            var productsResult = _mapper.Map<List<ProductResult>>(products);

            return new GetProductQueryResult
            {
                Products = productsResult,
                TotalCount = totalCount
            };
        }
    }
}
