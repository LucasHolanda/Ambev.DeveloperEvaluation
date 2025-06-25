using Ambev.DeveloperEvaluation.Domain.Aggregates;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, SaleDto>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public CreateSaleHandler(ISaleRepository saleRepository, ICartRepository cartRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<SaleDto> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetByIdAsync(command.CartId, cancellationToken);
            if (cart == null)
            {
                throw new InvalidOperationException("Cart not found.");
            }

            var sale = _mapper.Map<Sale>(command);
            await _saleRepository.AddAsync(sale, cancellationToken);

            cart.SetCartSold();
            await _cartRepository.UpdateCartAsync(cart, cancellationToken);

            return _mapper.Map<SaleDto>(sale);
        }
    }
}