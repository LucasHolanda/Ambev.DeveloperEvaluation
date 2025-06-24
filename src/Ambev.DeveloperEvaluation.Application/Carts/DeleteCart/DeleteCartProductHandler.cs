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

        public async Task<bool> Handle(DeleteCartProductCommand command, CancellationToken cancellationToken)
        {
            await _cartRepository.DeleteCartProductAsync(command.Id, cancellationToken);

            var count = await _cartRepository.CountByIdAsync(command.Id, cancellationToken);
            if (count == 0) await _cartRepository.DeleteAsync(command.Id, cancellationToken);

            return true;
        }
    }
}