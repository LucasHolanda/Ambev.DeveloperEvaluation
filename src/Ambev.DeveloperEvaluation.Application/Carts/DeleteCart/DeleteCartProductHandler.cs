using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart
{
    public class DeleteCartProductHandler : IRequestHandler<DeleteCartProductCommand, bool>
    {
        private readonly ICartRepository _cartRepository;

        public DeleteCartProductHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<bool> Handle(DeleteCartProductCommand request, CancellationToken cancellationToken)
        {
            return await _cartRepository.DeleteCartProductAsync(request.Id, cancellationToken);
        }
    }
}