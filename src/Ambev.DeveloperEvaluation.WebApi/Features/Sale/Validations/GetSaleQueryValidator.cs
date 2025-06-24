using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.Validations
{
    public class GetSaleQueryValidator : AbstractValidator<GetSalesQueryCommand>
    {
        public GetSaleQueryValidator()
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
