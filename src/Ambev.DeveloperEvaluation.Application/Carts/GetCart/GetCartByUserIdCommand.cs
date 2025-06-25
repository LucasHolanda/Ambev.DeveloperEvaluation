using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart
{
    public class GetCartByUserIdCommand : IRequest<CartDto?>
    {
        public Guid UserId { get; set; }
    }
}