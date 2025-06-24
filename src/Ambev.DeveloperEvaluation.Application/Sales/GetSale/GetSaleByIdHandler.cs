using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSaleById
{
    public class GetSaleByIdHandler : IRequestHandler<GetSaleByIdCommand, SaleDto?>
    {
        private readonly ISaleRepository _SaleRepository;
        private readonly IMapper _mapper;

        public GetSaleByIdHandler(ISaleRepository SaleRepository, IMapper mapper)
        {
            _SaleRepository = SaleRepository;
            _mapper = mapper;
        }

        public async Task<SaleDto?> Handle(GetSaleByIdCommand command, CancellationToken cancellationToken)
        {
            var Sale = await _SaleRepository.GetByIdAsync(command.Id, cancellationToken);

            return Sale != null
                ? _mapper.Map<SaleDto>(Sale)
                : null;
        }
    }
}