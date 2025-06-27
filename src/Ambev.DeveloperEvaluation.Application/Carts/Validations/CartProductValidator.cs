using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.Validations
{
    public class CartProductValidator : AbstractValidator<CartProductCommand>
    {
        public CartProductValidator()
        {
            RuleFor(product => product.ProductId)
                .NotEmpty().WithMessage("ProductId is required.")
                .Must(id => id != Guid.Empty).WithMessage("ProductId must be a valid GUID.");

            RuleFor(product => product.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Quantity must not exceed 100.");
        }
    }
}
