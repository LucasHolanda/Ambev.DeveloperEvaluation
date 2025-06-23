using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct
{
    public record DeleteProductByIdCommand (Guid Id) : IRequest<bool>
    {
    }
}
