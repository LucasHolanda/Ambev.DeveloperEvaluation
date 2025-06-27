using Ambev.DeveloperEvaluation.Domain.Repositories.Mongo;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart
{
    public class DeleteCartProductHandler : IRequestHandler<DeleteCartProductCommand, bool>
    {
        private readonly ICartMongoRepository _cartRepository;

        public DeleteCartProductHandler(ICartMongoRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<bool> Handle(DeleteCartProductCommand command, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetByCarProductIdAsync(command.Id, cancellationToken);
            if (cart == null) throw new ValidationException("Cart not found.");

            await _cartRepository.DeleteCartProductAsync(command.Id, cancellationToken);

            var count = cart.CartProducts.Count - 1;
            if (count == 0) await _cartRepository.DeleteAsync(cart.Id, cancellationToken);

            return true;
        }
    }
}