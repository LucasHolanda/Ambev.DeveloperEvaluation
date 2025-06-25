using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.Validations
{
    public class UpdateCartValidator : AbstractValidator<UpdateCartCommand>
    {
        public UpdateCartValidator()
        {
            RuleFor(cart => cart.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .Must(userId => userId != Guid.Empty).WithMessage("UserId must be a valid GUID.");
            RuleFor(cart => cart.Date)
                .NotEmpty().WithMessage("Date is required.")
                .Must(date => date != default(DateTime)).WithMessage("Date must be a valid DateTime.");
            RuleFor(cart => cart.CartProducts)
                .NotEmpty().WithMessage("At least one product is required in the cart.")
                .Must(products => products.Count > 0).WithMessage("Cart must contain at least one product.")
                .ForEach(product =>
                {
                    product.SetValidator(new CartProductValidator());
                });
        }
    }
}
