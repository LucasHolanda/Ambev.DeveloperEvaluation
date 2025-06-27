using Ambev.DeveloperEvaluation.Domain.Aggregates;
using Ambev.DeveloperEvaluation.Domain.Repositories.Mongo;
using AutoMapper;
using FluentValidation;
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
                throw new ValidationException("Cart already exists for this user.");
            }
 
            var cart = _mapper.Map<Cart>(command);             
            cart.GenerateCartProductIds();
            var carValidationResult = cart.Validate();
            if (!carValidationResult.IsValid)
            {
                throw new ValidationException(string.Join(", ", carValidationResult.Errors.Select(e => e.Detail)));
            }

            var cartCreated = await _repository.AddCartWithProductsAsync(cart, cancellationToken);
            var result = _mapper.Map<CartDto>(cartCreated);
            return result;
        }
    }
}