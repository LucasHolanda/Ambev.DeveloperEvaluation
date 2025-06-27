using Ambev.DeveloperEvaluation.Domain.Repositories.Mongo;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart
{
    public class DeleteCartHandler : IRequestHandler<DeleteCartCommand, bool>
    {
        private readonly ICartMongoRepository _repository;

        public DeleteCartHandler(ICartMongoRepository cartRepository)
        {
            _repository = cartRepository;
        }

        public async Task<bool> Handle(DeleteCartCommand command, CancellationToken cancellationToken)
        {
            var cart = await _repository.GetByIdAsync(command.Id, cancellationToken);
            if (cart == null)
            {
                throw new ValidationException("Cart not found.");
            }

            return await _repository.DeleteAllCartAsync(command.Id, cancellationToken);
        }
    }
}