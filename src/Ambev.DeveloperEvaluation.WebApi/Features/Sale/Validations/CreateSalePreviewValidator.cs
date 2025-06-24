using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.Validations
{
    public class CreateSalePreviewValidator : AbstractValidator<GetSalePreviewCommand>
    {
        public CreateSalePreviewValidator()
        {
            RuleFor(ps => ps.CartId)
                .NotEmpty()
                .WithMessage("CartId is required.")
                .NotEqual(Guid.Empty)
                .WithMessage("CartId cannot be an empty GUID.");
        }
    }
}