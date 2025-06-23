using Ambev.DeveloperEvaluation.Application.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct
{
    public record GetProductQueryCommand : IRequest<GetProductQueryResult>
    {
        public QueryParametersCommand QueryParameters { get; set; } = new();
    }
}
