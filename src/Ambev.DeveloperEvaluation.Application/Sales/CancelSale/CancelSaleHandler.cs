using Ambev.DeveloperEvaluation.Application.Publisher;
using Ambev.DeveloperEvaluation.Application.Publisher.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;
using Serilog;

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
            Log.Information("Handling CancelSaleCommand for SaleId: {SaleId}", command.Id);
            var sale = await _saleRepository.GetWithItemsAsync(command.Id, cancellationToken);
            if (sale == null)
            {
                throw new ValidationException("Sale not found.");
            }

            Log.Information("Sale found with Id: {SaleId}, checking if it can be cancelled.", sale.Id);
            if (sale.IsCancelled)
            {
                throw new ValidationException("Sale is already cancelled.");
            }

            Log.Information("Sale with Id: {SaleId} is not cancelled, proceeding to cancel sale and items.", sale.Id);
            sale.CancelSaleAndItems(command.CancelationReason);
            foreach (var item in sale.SaleItems)
            {
                await _messagePublisher.PublishEventAsync("salesitem.cancelled", new SaleItemCancelledEvent(item.SaleId, item.Id, item.ProductId, item.Quantity, command.CancelationReason, DateTime.UtcNow));
            }
            Log.Information("Publishing SaleCancelledEvent for SaleId: {SaleId}", sale.Id);
            await _messagePublisher.PublishEventAsync("sales.cancelled", new SaleCancelledEvent(sale.Id, DateTime.UtcNow, sale.SaleNumber, command.CancelationReason));

            Log.Information("Sale with Id: {SaleId} cancelled successfully, updating sale and items in repository.", sale.Id);
            return await _saleRepository.UpdateSaleAndItemsAsync(sale, cancellationToken);
        }
    }
}