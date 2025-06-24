using Ambev.DeveloperEvaluation.Domain.Aggregates;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class CartValidator : AbstractValidator<Cart>
    {
        public CartValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.CartProducts).NotEmpty().WithMessage("Cart must contain at least one product.");
            RuleForEach(x => x.CartProducts).SetValidator(new CartProductValidator());
        }
    }
}