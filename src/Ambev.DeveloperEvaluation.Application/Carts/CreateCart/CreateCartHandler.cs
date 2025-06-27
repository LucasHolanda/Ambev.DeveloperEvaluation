using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories.Mongo;
using AutoMapper;
using FluentValidation;
using MediatR;
using Serilog;

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
            Log.Information("Handling CreateCartCommand for UserId: {UserId}", command.UserId);
            var cartExists = await _repository.GetByUserIdAsync(command.UserId, cancellationToken);

            if (cartExists != null)
            {
                throw new ValidationException("Cart already exists for this user.");
            }

            var cart = _mapper.Map<Cart>(command);
            cart.GenerateCartProductIds();
            var carValidationResult = cart.Validate();
            Log.Information("Cart validation result: {IsValid}", carValidationResult.IsValid);
            if (!carValidationResult.IsValid)
            {
                throw new ValidationException(string.Join(", ", carValidationResult.Errors.Select(e => e.Detail)));
            }

            Log.Information("Creating cart for UserId: {UserId}", command.UserId);
            var cartCreated = await _repository.AddCartWithProductsAsync(cart, cancellationToken);
            var result = _mapper.Map<CartDto>(cartCreated);
            return result;
        }
    }
}