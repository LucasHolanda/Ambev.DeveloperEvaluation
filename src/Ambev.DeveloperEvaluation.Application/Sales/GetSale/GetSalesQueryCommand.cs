using Ambev.DeveloperEvaluation.Application.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSalesQueryCommand : IRequest<GetSalesQueryDto>
    {
        public QueryParametersCommand QueryParameters { get; set; } = new();
    }
}