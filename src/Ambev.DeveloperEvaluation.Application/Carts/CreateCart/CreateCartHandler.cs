using Ambev.DeveloperEvaluation.Domain.Aggregates;
using Ambev.DeveloperEvaluation.Domain.Repositories.Mongo;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart
{
    public class CreateCartHandler : IRequestHandler<CreateCartCommand, CartDto>
    {
        private readonly ICartMongoRepository _repository;
        private readonly IMapper _mapper;

        public CreateCartHandler(ICartMongoRepository cartRepository, IMapper mapper)
        {
            _repository = cartRepository;
            _mapper = mapper;
        }

        public async Task<CartDto> Handle(CreateCartCommand command, CancellationToken cancellationToken)
        {
            var cartExists = await _repository.GetByUserIdAsync(command.UserId, cancellationToken);

            if (cartExists != null)
            {
                throw new InvalidOperationException("Cart already exists for this user.");
            }

            var cart = _mapper.Map<Cart>(command);
            var carValidationResult = cart.Validate();
            if (!carValidationResult.IsValid)
            {
                throw new InvalidOperationException(string.Join(", ", carValidationResult.Errors.Select(e => e.Error)));
            }

            var cartCreated = await _repository.AddCartWithProductsAsync(cart, cancellationToken);

            var result = _mapper.Map<CartDto>(cartCreated);
            return result;
        }
    }
}