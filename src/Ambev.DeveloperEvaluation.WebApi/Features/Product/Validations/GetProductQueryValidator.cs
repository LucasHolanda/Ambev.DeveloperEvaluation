using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Product.Validations
{
    public class GetProductQueryValidator : AbstractValidator<GetProductQueryCommand>
    {
        public GetProductQueryValidator()
        {
            RuleFor(qp => qp.QueryParameters)
                .NotNull()
                .WithMessage("Query parameters cannot be null.");

            RuleFor(qp => qp.QueryParameters._page)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0.");

            RuleFor(qp => qp.QueryParameters.Filters)
                .NotNull()
                .NotEmpty()
                .WithMessage("Filters cannot be null or empty.");
        }
    }
}
