using Ambev.DeveloperEvaluation.Domain.Repositories.Mongo;
using AutoMapper;
using FluentValidation;
using MediatR;
using Serilog;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart
{
    public class UpdateCartHandler : IRequestHandler<UpdateCartCommand, CartDto?>
    {
        private readonly ICartMongoRepository _cartRepository;
        private readonly IMapper _mapper;

        public UpdateCartHandler(ICartMongoRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<CartDto?> Handle(UpdateCartCommand command, CancellationToken cancellationToken)
        {
            Log.Information("Handling UpdateCartCommand for CartId: {CartId}", command.Id);
            var cart = await _cartRepository.GetByIdAsync(command.Id, cancellationToken);

            if (cart == null)
                throw new ValidationException("Cart not found.");

            Log.Information("Cart found with Id: {CartId}, proceeding to update.", command.Id);
            _mapper.Map(command, cart);

            cart.SetUpdated();
            cart.GenerateCartProductIds();

            await _cartRepository.UpdateCartAsync(cart, cancellationToken);
            Log.Information("Cart with Id: {CartId} updated successfully.", command.Id);

            return _mapper.Map<CartDto>(cart);
        }
    }
}