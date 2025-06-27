using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSaleById
{
    public class GetSaleByIdCommand : IRequest<SaleDto>
    {
        public Guid Id { get; set; }
    }
}