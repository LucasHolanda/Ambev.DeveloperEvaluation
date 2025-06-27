using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct
{
    public record GetProductByIdCommand(Guid Id) : IRequest<ProductDto>;
}
