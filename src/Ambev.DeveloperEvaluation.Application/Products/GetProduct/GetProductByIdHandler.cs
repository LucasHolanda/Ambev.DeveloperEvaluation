using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdCommand, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductByIdHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(GetProductByIdCommand command, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetByIdAsync(command.Id, cancellationToken);

            if (products == null)
                throw new KeyNotFoundException($"Product with id {command.Id} not found.");

            return _mapper.Map<ProductDto>(products);
        }
    }
}
