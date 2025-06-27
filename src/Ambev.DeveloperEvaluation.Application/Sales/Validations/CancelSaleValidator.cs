using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Validations
{
    public class CancelSaleValidator : AbstractValidator<CancelSaleCommand>
    {
        public CancelSaleValidator()
        {
            RuleFor(s => s.Id)
                .NotEmpty()
                .WithMessage("Sale ID is required.")
                .Must(id => id != Guid.Empty)
                .WithMessage("Sale ID must not be an empty GUID.");

            RuleFor(s => s.CancelationReason)
                .NotEmpty()
                .WithMessage("Cancelation reason is required.")
                .MaximumLength(500)
                .WithMessage("Cancelation reason must not exceed 500 characters.");
        }
    }
}