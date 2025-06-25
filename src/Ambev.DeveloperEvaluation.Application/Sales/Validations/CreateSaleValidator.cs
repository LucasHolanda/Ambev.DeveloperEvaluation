using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Validations
{
    public class CreateSaleValidator : AbstractValidator<CreateSaleCommand>
    {
        public CreateSaleValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty()
                .WithMessage("Customer ID is required.")
                .Must(id => id != Guid.Empty)
                .WithMessage("Customer ID must not be an empty GUID.");

            RuleFor(x => x.SaleDate).NotEmpty()
                .WithMessage("Sale date is required.")
                .Must(date => date != default)
                .WithMessage("Sale date must be a valid date.");

            RuleFor(x => x.SaleItems)
                .NotEmpty()
                .WithMessage("At least one sale item is required.")
                .Must(items => items.Count > 0)
                .WithMessage("Sale items must contain at least one item.");

            RuleForEach(x => x.SaleItems)
                .NotEmpty()
                .WithMessage("Sale item cannot be empty.")
                .SetValidator(new SaleItemValidator());

            RuleFor(x => x.TotalAmount)
                .GreaterThan(0)
                .WithMessage("Total amount must be greater than 0.")
                .PrecisionScale(18, 2, true)
                .WithMessage("Total amount must have a maximum of 18 digits and 2 decimal places.");

            RuleFor(cart => cart.CartId)
                .NotEmpty()
                .WithMessage("Cart ID is required.")
                .Must(id => id != Guid.Empty)
                .WithMessage("Cart ID must not be an empty GUID."); 

            RuleFor(x => x.SaleNumber)
                .NotEmpty()
                .WithMessage("Sale number is required.")
                .MaximumLength(50)
                .WithMessage("Sale number must not exceed 50 characters.");
        }
    }

    public class SaleItemValidator : AbstractValidator<CreateSaleItemCommand>
    {
        public SaleItemValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty()
                .WithMessage("Product ID is required.")
                .Must(id => id != Guid.Empty)
                .WithMessage("Product ID must not be an empty GUID.");

            RuleFor(x => x.ProductName).NotEmpty()
                .WithMessage("Product name is required.")
                .MaximumLength(200)
                .WithMessage("Product name must not exceed 200 characters.");

            RuleFor(x => x.Quantity).GreaterThan(0)
                .WithMessage("Quantity must be greater than 0.")
                .Must(q => q % 1 == 0)
                .WithMessage("Quantity must be a whole number.");

            RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0)
                .WithMessage("Unit price must be greater than or equal to 0.")
                .PrecisionScale(18, 2, true)
                .WithMessage("Unit price must have a maximum of 18 digits and 2 decimal places.");

            RuleFor(x => x.TotalAmount).GreaterThanOrEqualTo(0)
                .WithMessage("Total amount must be greater than or equal to 0.")
                .PrecisionScale(18, 2, true)
                .WithMessage("Total amount must have a maximum of 18 digits and 2 decimal places.");
        }
    }
}