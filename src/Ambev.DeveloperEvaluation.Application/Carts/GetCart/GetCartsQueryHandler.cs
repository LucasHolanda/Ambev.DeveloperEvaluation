using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart
{
    public class GetCartsQueryHandler : IRequestHandler<GetCartsQueryCommand, GetCartsQueryDto>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public GetCartsQueryHandler(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<GetCartsQueryDto> Handle(GetCartsQueryCommand command, CancellationToken cancellationToken)
        {
            var query = _mapper.Map<QueryParameters>(command.QueryParameters);
            var carts = await _cartRepository.GetByQueryParameters(query, null, cancellationToken);

            if (carts == null)
            {
                throw new KeyNotFoundException("Carts with QueryParameters not found.");
            }

            var totalCount = await _cartRepository.CountByQueryParametersAsync(query, cancellationToken);

            var cartsResult = _mapper.Map<List<CartDto>>(carts);

            return new GetCartsQueryDto
            {
                Carts = cartsResult,
                TotalCount = totalCount
            };
        }
    }
}