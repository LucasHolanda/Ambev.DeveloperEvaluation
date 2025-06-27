using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Serilog;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public UpdateProductHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            Log.Information("Handling UpdateProductCommand for ProductId: {ProductId}", command.Id);
            var product = await _productRepository.GetByIdAsync(command.Id, cancellationToken);
            if (product == null)
                throw new ValidationException($"Product with id {command.Id} not found.");

            _mapper.Map(command, product);

            Log.Information("Product found with Id: {ProductId}, proceeding to update.", command.Id);
            await _productRepository.UpdateAsync(product, cancellationToken);

            var result = _mapper.Map<ProductDto>(product);
            return result;
        }
    }
}