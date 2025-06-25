using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart
{
    public class GetCartByUserIdHandler : IRequestHandler<GetCartByUserIdCommand, CartDto?>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public GetCartByUserIdHandler(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<CartDto?> Handle(GetCartByUserIdCommand command, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetByUserIdAsync(command.UserId, cancellationToken);

            return cart != null
                ? _mapper.Map<CartDto>(cart)
                : null;
        }
    }
}