using Ambev.DeveloperEvaluation.Application.Publisher;
using Ambev.DeveloperEvaluation.Application.Publisher.Events;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Repositories.Mongo;
using AutoMapper;
using FluentValidation;
using MediatR;
using Serilog;

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
            Log.Information("Handling CreateSaleCommand for CartId: {CartId}, SaleDate: {SaleDate}, TotalAmount: {TotalAmount}",
                command.CartId, command.SaleDate, command.TotalAmount);
            var cart = await _cartMongoRepository.GetByIdAsync(command.CartId, cancellationToken);
            if (cart == null)
            {
                throw new ValidationException("Cart not found.");
            }

            Log.Information("Cart found with Id: {CartId}, proceeding to create Sale.", cart.Id);
            var sale = _mapper.Map<Sale>(command);
            var validationResult = sale.Validate();
            if (!validationResult.IsValid)
            {
                throw new ValidationException(string.Join(", ", validationResult.Errors.Select(e => e.Detail)));
            }

            await _cartRepository.AddCartWithProductsAsync(cart, cancellationToken);
            sale = await _saleRepository.AddAsync(sale, cancellationToken);
            await _cartMongoRepository.DeleteAsync(cart.Id, cancellationToken);

            Log.Information("Sale created with Id: {SaleId}, CartId: {CartId}, SaleDate: {SaleDate}, TotalAmount: {TotalAmount}",
                sale.Id, sale.CartId, sale.SaleDate, sale.TotalAmount);
            await _messagePublisher.PublishEventAsync("sales.created", new SaleCreatedEvent(sale.Id, sale.CartId, sale.SaleDate, sale.TotalAmount, sale.SaleNumber));
            Log.Information("Published SaleCreatedEvent for SaleId: {SaleId}", sale.Id);

            return _mapper.Map<SaleDto>(sale);
        }
    }
}