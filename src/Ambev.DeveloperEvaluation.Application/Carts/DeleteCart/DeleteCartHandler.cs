using Ambev.DeveloperEvaluation.Domain.Repositories.Mongo;
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
            return await _repository.DeleteAllCartAsync(command.Id, cancellationToken);
        }
    }
}