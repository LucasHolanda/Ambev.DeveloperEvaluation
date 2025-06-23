using Ambev.DeveloperEvaluation.Application.Carts.GetCart;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.Validations
{
    public class GetCartQueryValidator : AbstractValidator<GetCartsQueryCommand>
    {
        public GetCartQueryValidator()
        {
            RuleFor(qc => qc.QueryParameters)
                .NotNull()
                .WithMessage("Query parameters cannot be null.");

            RuleFor(qc => qc.QueryParameters._page)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0.");
        }
    }
}
