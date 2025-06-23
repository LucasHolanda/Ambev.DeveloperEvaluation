using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart
{
    public class GetCartByIdCommand : IRequest<CartDto?>
    {
        public Guid Id { get; set; }
    }
}