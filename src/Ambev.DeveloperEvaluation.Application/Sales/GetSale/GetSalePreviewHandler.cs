using Ambev.DeveloperEvaluation.Domain.Aggregates;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSalePreviewHandler : IRequestHandler<GetSalePreviewCommand, SaleDto>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public GetSalePreviewHandler(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<SaleDto> Handle(GetSalePreviewCommand command, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetValidCartWithProductsAsync(command.CartId, cancellationToken);
            if (cart == null)
                throw new KeyNotFoundException("Cart not found.");

            var sale = Sale.CreateByCart(cart);
            return _mapper.Map<SaleDto>(sale);
        }
    }
}