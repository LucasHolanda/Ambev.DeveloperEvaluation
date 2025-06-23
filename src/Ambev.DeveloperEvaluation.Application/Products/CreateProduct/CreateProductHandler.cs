using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public CreateProductHandler(
            IProductRepository productRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var existingProduct = await _productRepository.GetByTitleAndCategoryAsync(command.Title, command.Category, cancellationToken);
            if (existingProduct.Any())
                throw new ValidationException($"A product with title '{command.Title}' and category '{command.Category}' already exists.");

            var product = _mapper.Map<Product>(command);
            await _productRepository.AddAsync(product, cancellationToken);
            var result = _mapper.Map<ProductResult>(product);
            return result;
        }
    }
}
