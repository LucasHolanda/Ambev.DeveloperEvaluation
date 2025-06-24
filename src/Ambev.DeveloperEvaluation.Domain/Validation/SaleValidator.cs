using Ambev.DeveloperEvaluation.Domain.Aggregates;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class SaleValidator : AbstractValidator<Sale>
    {
        public SaleValidator()
        {
            RuleFor(x => x.SaleNumber).NotEmpty().MaximumLength(50);
            RuleFor(x => x.SaleDate).NotEmpty();
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.BranchId).NotEmpty();
            RuleFor(x => x.TotalAmount).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Status).NotNull();
            RuleForEach(x => x.SaleItems).SetValidator(new SaleItemValidator());
        }
    }
}