using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class CartValidator : AbstractValidator<Cart>
    {
        public CartValidator()
        {
            RuleFor(x => x.UserId).NotEmpty()
                .WithMessage("User ID is required.");

            RuleFor(x => x.Date).NotEmpty()
                .WithMessage("Cart date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Cart date cannot be in the future.");

            RuleFor(x => x.CartProducts).NotEmpty().WithMessage("Cart must contain at least one product.")
                .Must(products => products.All(p => p.Quantity > 0))
                .WithMessage("All products in the cart must have a quantity greater than zero.");

            RuleForEach(x => x.CartProducts).SetValidator(new CartProductValidator());
        }
    }
}