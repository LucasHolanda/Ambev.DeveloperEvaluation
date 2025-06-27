using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class CartProductValidator : AbstractValidator<CartProduct>
    {
        public CartProductValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty()
                .WithMessage("Product ID must not be empty.");

            RuleFor(x => x.Quantity).GreaterThan(0)
                .WithMessage("Quantity must be greater than zero.");

            RuleFor(x => x.Product)
                .NotNull()
                .WithMessage("Product must not be null.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Product.Title).NotEmpty()
                        .WithMessage("Product title must not be empty.");
                    RuleFor(x => x.Product.Price).GreaterThan(0)
                        .WithMessage("Product price must be greater than zero.");
                });
        }
    }
}