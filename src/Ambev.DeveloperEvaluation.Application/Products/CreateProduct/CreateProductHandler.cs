using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Serilog;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductDto>
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

        public async Task<ProductDto> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            Log.Information("Handling CreateProductCommand for Product: {Title}, Category: {Category}", command.Title, command.Category);
            var existingProduct = await _productRepository.GetByTitleAndCategoryAsync(command.Title, command.Category, cancellationToken);
            if (existingProduct.Any())
                throw new ValidationException($"A product with title '{command.Title}' and category '{command.Category}' already exists.");

            var product = _mapper.Map<Product>(command);

            var validationResult = product.Validate();
            Log.Information("Product validation result: {IsValid}", validationResult.IsValid);
            if (!validationResult.IsValid)
                throw new ValidationException(string.Join(", ", validationResult.Errors.Select(e => e.Detail)));

            Log.Information("Creating product with Title: {Title}, Category: {Category}", command.Title, command.Category);
            product = await _productRepository.AddAsync(product, cancellationToken);
            return _mapper.Map<ProductDto>(product);
        }
    }
}
