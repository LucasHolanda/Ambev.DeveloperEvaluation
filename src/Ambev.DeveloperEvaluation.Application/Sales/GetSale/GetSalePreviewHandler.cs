using Ambev.DeveloperEvaluation.Domain.Aggregates;
using Ambev.DeveloperEvaluation.Domain.Repositories.Mongo;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSalePreviewHandler : IRequestHandler<GetSalePreviewCommand, SaleDto>
    {
        private readonly ICartMongoRepository _repository;
        private readonly IMapper _mapper;

        public GetSalePreviewHandler(ICartMongoRepository cartRepository, IMapper mapper)
        {
            _repository = cartRepository;
            _mapper = mapper;
        }

        public async Task<SaleDto> Handle(GetSalePreviewCommand command, CancellationToken cancellationToken)
        {
            var cart = await _repository.GetByIdAsync(command.CartId, cancellationToken);
            if (cart == null)
                throw new ValidationException("Cart not found.");

            var sale = Sale.CreateByCart(cart);
            return _mapper.Map<SaleDto>(sale);
        }
    }
}