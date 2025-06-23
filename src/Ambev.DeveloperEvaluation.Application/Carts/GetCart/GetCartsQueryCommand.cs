using Ambev.DeveloperEvaluation.Application.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart
{
    public class GetCartsQueryCommand : IRequest<GetCartsQueryDto>
    {
        public QueryParametersCommand QueryParameters { get; set; } = new();
    }
}