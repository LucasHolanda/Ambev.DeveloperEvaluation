using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.Validations
{
    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {
            RuleFor(command => command.Title)
               .NotEmpty().WithMessage("Title is required.")
               .MaximumLength(500).WithMessage("Title must not exceed 500 characters.");

            RuleFor(command => command.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.")
                .PrecisionScale(18, 2, true).WithMessage("Price must have a maximum of 18 digits and 2 decimal places.");

            RuleFor(command => command.Description)
                .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.");

            RuleFor(command => command.Category)
                .NotEmpty().WithMessage("Category is required.")
                .MaximumLength(100).WithMessage("Category must not exceed 100 characters.");

            RuleFor(command => command.Image)
                .MaximumLength(1000).WithMessage("Image URL must not exceed 1000 characters.");

            RuleFor(command => command.Rating)
                .NotNull().WithMessage("Rating is required.")
                .DependentRules(() =>
                {
                    RuleFor(command => command.Rating.Rate)
                        .InclusiveBetween(0, 5).WithMessage("Rating rate must be between 0 and 5.");
                    RuleFor(command => command.Rating.Count)
                        .GreaterThanOrEqualTo(0).WithMessage("Rating count must be greater than or equal to 0.");
                });

        }
    }
}