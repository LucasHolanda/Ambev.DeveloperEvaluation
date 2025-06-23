using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart
{
    public class GetCartByIdHandler : IRequestHandler<GetCartByIdCommand, CartDto?>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public GetCartByIdHandler(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<CartDto?> Handle(GetCartByIdCommand request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetWithProductsAsync(request.Id, cancellationToken);

            return cart != null
                ? _mapper.Map<CartDto>(cart)
                : null;
        }
    }
}