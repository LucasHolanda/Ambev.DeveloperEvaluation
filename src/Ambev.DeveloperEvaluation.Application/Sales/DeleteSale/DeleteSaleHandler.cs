using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Serilog;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale
{
    public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, bool>
    {
        private readonly ISaleRepository _saleRepository;

        public DeleteSaleHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task<bool> Handle(DeleteSaleCommand command, CancellationToken cancellationToken)
        {
            Log.Information("Handling DeleteSaleCommand for SaleId: {SaleId}", command.Id);
            return await _saleRepository.DeleteAsync(command.Id, cancellationToken);
        }
    }
}