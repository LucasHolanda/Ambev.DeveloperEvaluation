using Ambev.DeveloperEvaluation.Domain.Repositories.Mongo;
using FluentValidation;
using MediatR;
using Serilog;

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
            Log.Information("Handling DeleteCartProductCommand for CartProductId: {CartProductId}", command.Id);
            var cart = await _cartRepository.GetByCarProductIdAsync(command.Id, cancellationToken);
            if (cart == null) throw new ValidationException("Cart not found.");

            Log.Information("Cart found with Id: {CartId}, proceeding to delete CartProductId: {CartProductId}", cart.Id, command.Id);
            await _cartRepository.DeleteCartProductAsync(command.Id, cancellationToken);

            Log.Information("CartProductId: {CartProductId} deleted successfully from CartId: {CartId}", command.Id, cart.Id);
            var count = cart.CartProducts.Count - 1;
            Log.Information("Remaining CartProducts count after deletion: {Count}", count);
            if (count == 0)
            {
                Log.Information("No more CartProducts left in CartId: {CartId}, deleting the cart.", cart.Id);
                await _cartRepository.DeleteAsync(cart.Id, cancellationToken);
            }

            return true;
        }
    }
}