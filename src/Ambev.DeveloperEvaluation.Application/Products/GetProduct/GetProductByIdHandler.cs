using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Serilog;

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
            Log.Information("Handling GetProductByIdCommand for ProductId: {ProductId}", command.Id);
            var products = await _productRepository.GetByIdAsync(command.Id, cancellationToken);

            if (products == null)
                throw new ValidationException($"Product with id {command.Id} not found.");

            Log.Information("Product found with Id: {ProductId}, proceeding to map.", command.Id);
            return _mapper.Map<ProductDto>(products);
        }
    }
}
