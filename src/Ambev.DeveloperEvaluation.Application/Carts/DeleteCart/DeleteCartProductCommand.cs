using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart
{
    public class DeleteCartProductCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}