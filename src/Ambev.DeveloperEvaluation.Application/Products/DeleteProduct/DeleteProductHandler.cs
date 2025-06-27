using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;
using Serilog;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductByIdCommand, bool>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> Handle(DeleteProductByIdCommand command, CancellationToken cancellationToken)
        {
            Log.Information("Handling DeleteProductByIdCommand for ProductId: {ProductId}", command.Id);
            var product = await _productRepository.GetByIdAsync(command.Id, cancellationToken);
            if (product == null)
                throw new ValidationException($"Product with id {command.Id} not found.");

            Log.Information("Product found with Id: {ProductId}, proceeding to delete.", command.Id);
            return await _productRepository.DeleteAsync(command.Id, cancellationToken);
        }
    }
}