using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class CartProductValidator : AbstractValidator<CartProduct>
    {
        public CartProductValidator()
        {
            RuleFor(x => x.Id).NotEmpty()
                .WithMessage("Cart Product ID must not be empty.");

            RuleFor(x => x.CartId).NotEmpty()
                .WithMessage("Cart ID must not be empty.");

            RuleFor(x => x.ProductId).NotEmpty()
                .WithMessage("Product ID must not be empty.");

            RuleFor(x => x.Quantity).GreaterThan(0)
                .WithMessage("Quantity must be greater than zero.");
        }
    }
}