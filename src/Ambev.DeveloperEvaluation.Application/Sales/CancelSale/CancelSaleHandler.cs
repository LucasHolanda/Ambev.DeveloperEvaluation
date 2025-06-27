using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, bool>
    {
        private readonly ISaleRepository _saleRepository;

        public CancelSaleHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
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

            return await _saleRepository.UpdateSaleAndItemsAsync(sale, cancellationToken);
        }
    }
}