using Ambev.DeveloperEvaluation.Domain.Repositories.Mongo;
using AutoMapper;
using MediatR;
using Serilog;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart
{
    public class GetCartByIdHandler : IRequestHandler<GetCartByIdCommand, CartDto?>
    {
        private readonly ICartMongoRepository _repository;
        private readonly IMapper _mapper;

        public GetCartByIdHandler(ICartMongoRepository cartRepository, IMapper mapper)
        {
            _repository = cartRepository;
            _mapper = mapper;
        }

        public async Task<CartDto?> Handle(GetCartByIdCommand command, CancellationToken cancellationToken)
        {
            Log.Information("Handling GetCartByIdCommand for CartId: {CartId}", command.Id);
            var cart = await _repository.GetByIdAsync(command.Id, cancellationToken);

            return cart != null
                ? _mapper.Map<CartDto>(cart)
                : null;
        }
    }
}