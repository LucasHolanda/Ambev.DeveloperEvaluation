using Ambev.DeveloperEvaluation.Application.Publisher;
using Ambev.DeveloperEvaluation.Application.Publisher.Events;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, bool>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMessagePublisher _messagePublisher;

        public CancelSaleHandler(ISaleRepository saleRepository, IMessagePublisher messagePublisher)
        {
            _saleRepository = saleRepository;
            _messagePublisher = messagePublisher;
        }

        public async Task<bool> Handle(CancelSaleCommand command, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetWithItemsAsync(command.Id, cancellationToken);
            if (sale == null)
            {
                throw new ValidationException("Sale not found.");
            }

            if (sale.IsCancelled)
            {
                throw new ValidationException("Sale is already cancelled.");
            }

            sale.CancelSaleAndItems(command.CancelationReason);
            foreach (var item in sale.SaleItems)
            {
                await _messagePublisher.PublishEventAsync("salesitem.cancelled", new SaleItemCancelledEvent(item.SaleId ,item.Id, item.ProductId, item.Quantity, command.CancelationReason, DateTime.UtcNow));
            }
            await _messagePublisher.PublishEventAsync("sales.cancelled", new SaleCancelledEvent(sale.Id, DateTime.UtcNow, sale.SaleNumber, command.CancelationReason));

            return await _saleRepository.UpdateSaleAndItemsAsync(sale, cancellationToken);
        }
    }
}