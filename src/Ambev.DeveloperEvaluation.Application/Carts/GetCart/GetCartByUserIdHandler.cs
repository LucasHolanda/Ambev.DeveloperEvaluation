using Ambev.DeveloperEvaluation.Domain.Repositories.Mongo;
using AutoMapper;
using MediatR;
using Serilog;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart
{
    public class GetCartByUserIdHandler : IRequestHandler<GetCartByUserIdCommand, CartDto?>
    {
        private readonly ICartMongoRepository _repository;
        private readonly IMapper _mapper;

        public GetCartByUserIdHandler(ICartMongoRepository cartRepository, IMapper mapper)
        {
            _repository = cartRepository;
            _mapper = mapper;
        }

        public async Task<CartDto?> Handle(GetCartByUserIdCommand command, CancellationToken cancellationToken)
        {
            Log.Information("Handling GetCartByUserIdCommand for UserId: {UserId}", command.UserId);
            var cart = await _repository.GetByUserIdAsync(command.UserId, cancellationToken);

            return cart != null
                ? _mapper.Map<CartDto>(cart)
                : null;
        }
    }
}