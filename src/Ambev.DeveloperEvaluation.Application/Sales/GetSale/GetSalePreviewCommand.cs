using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public record GetSalePreviewCommand(Guid CartId) : IRequest<SaleDto>
    { }
}