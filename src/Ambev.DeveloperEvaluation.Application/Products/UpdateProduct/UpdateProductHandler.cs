using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, ProductResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public UpdateProductHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        { 
            var product = await _productRepository.GetByIdAsync(command.Id, cancellationToken);
            if (product == null)
                throw new KeyNotFoundException($"Product with id {command.Id} not found.");

            _mapper.Map(command, product);

            await _productRepository.UpdateAsync(product, cancellationToken);

            var result = _mapper.Map<ProductResult>(product);
            return result;
        }
    }
}