using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSalesQueryHandler : IRequestHandler<GetSalesQueryCommand, GetSalesQueryDto>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public GetSalesQueryHandler(ISaleRepository SaleRepository, IMapper mapper)
        {
            _saleRepository = SaleRepository;
            _mapper = mapper;
        }

        public async Task<GetSalesQueryDto> Handle(GetSalesQueryCommand command, CancellationToken cancellationToken)
        {
            var query = _mapper.Map<QueryParameters>(command.QueryParameters);
            var sales = await _saleRepository.GetAllByQuery(query, cancellationToken);

            if (sales == null)
            {
                throw new KeyNotFoundException("Sales with QueryParameters not found.");
            }

            var totalCount = await _saleRepository.CountByQueryParametersAsync(query, cancellationToken);

            var SalesResult = _mapper.Map<List<SaleDto>>(sales);

            return new GetSalesQueryDto
            {
                Sales = SalesResult,
                TotalCount = totalCount
            };
        }
    }
}