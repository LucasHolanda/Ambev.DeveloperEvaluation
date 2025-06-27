using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Serilog;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSaleById
{
    public class GetSaleByIdHandler : IRequestHandler<GetSaleByIdCommand, SaleDto?>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public GetSaleByIdHandler(ISaleRepository SaleRepository, IMapper mapper)
        {
            _saleRepository = SaleRepository;
            _mapper = mapper;
        }

        public async Task<SaleDto?> Handle(GetSaleByIdCommand command, CancellationToken cancellationToken)
        {
            Log.Information("Handling GetSaleByIdCommand for SaleId: {SaleId}", command.Id);
            var Sale = await _saleRepository.GetWithItemsAndProductsAsync(command.Id, cancellationToken);

            return Sale != null
                ? _mapper.Map<SaleDto>(Sale)
                : null;
        }
    }
}