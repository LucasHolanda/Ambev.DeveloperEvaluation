using Ambev.DeveloperEvaluation.Application.Publisher;
using Ambev.DeveloperEvaluation.Application.Publisher.Events;
using Ambev.DeveloperEvaluation.Domain.Aggregates;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Repositories.Mongo;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, SaleDto>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ICartMongoRepository _cartMongoRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;
        private readonly IMessagePublisher _messagePublisher;

        public CreateSaleHandler(
            ISaleRepository saleRepository,
            ICartMongoRepository cartMongoRepository,
            ICartRepository cartRepository,
            IMapper mapper,
            IMessagePublisher messagePublisher
            )
        {
            _saleRepository = saleRepository;
            _cartMongoRepository = cartMongoRepository;
            _cartRepository = cartRepository;
            _mapper = mapper;
            _messagePublisher = messagePublisher;
        }

        public async Task<SaleDto> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
        {
            var cart = await _cartMongoRepository.GetByIdAsync(command.CartId, cancellationToken);
            if (cart == null)
            {
                throw new ValidationException("Cart not found.");
            }

            var sale = _mapper.Map<Sale>(command);
            var validationResult = sale.Validate();
            if (!validationResult.IsValid)
            {
                throw new ValidationException(string.Join(", ", validationResult.Errors.Select(e => e.Detail)));
            }

            await _cartRepository.AddCartWithProductsAsync(cart, cancellationToken);
            sale = await _saleRepository.AddAsync(sale, cancellationToken);
            await _cartMongoRepository.DeleteAsync(cart.Id, cancellationToken);

            await _messagePublisher.PublishEventAsync("sales.created", new SaleCreatedEvent(sale.Id, sale.CartId, sale.SaleDate, sale.TotalAmount, sale.SaleNumber));

            return _mapper.Map<SaleDto>(sale);
        }
    }
}