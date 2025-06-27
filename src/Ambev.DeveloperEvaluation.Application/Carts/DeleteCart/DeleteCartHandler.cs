using Ambev.DeveloperEvaluation.Domain.Repositories.Mongo;
using FluentValidation;
using MediatR;
using Serilog;

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
            Log.Information("Handling DeleteCartCommand for CartId: {CartId}", command.Id);
            var cart = await _repository.GetByIdAsync(command.Id, cancellationToken);
            if (cart == null)
            {
                throw new ValidationException("Cart not found.");
            }

            Log.Information("Deleting cart with CartId: {CartId}", command.Id);
            return await _repository.DeleteAllCartAsync(command.Id, cancellationToken);
        }
    }
}