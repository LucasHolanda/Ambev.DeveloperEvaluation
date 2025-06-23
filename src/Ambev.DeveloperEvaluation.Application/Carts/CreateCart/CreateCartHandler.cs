using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart
{
    public class CreateCartHandler : IRequestHandler<CreateCartCommand, CartDto>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public CreateCartHandler(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<CartDto> Handle(CreateCartCommand command, CancellationToken cancellationToken)
        {
            var cartExists = await _cartRepository.GetByUserIdAsync(command.UserId, cancellationToken);

            if (cartExists != null)
            {
                throw new InvalidOperationException("Cart already exists for this user.");
            }

            if (command.CartProducts == null || !command.CartProducts.Any())
            {
                throw new InvalidOperationException("Cart must contain at least one product.");
            }

            var cart = _mapper.Map<Cart>(command);
            var cartCreated = await _cartRepository.AddCartWithProductsAsync(cart, cancellationToken);

            var result = _mapper.Map<CartDto>(cartCreated);
            return result;
        }
    }
}