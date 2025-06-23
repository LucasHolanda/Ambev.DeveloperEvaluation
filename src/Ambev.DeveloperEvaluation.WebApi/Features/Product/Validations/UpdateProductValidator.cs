using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Product.Validations
{
    public class UpdateProductValidator : AbstractValidator<ProductDto>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Id).NotEmpty()
                .WithMessage("Product ID is required.")
                .Must(id => id != Guid.Empty)
                .WithMessage("Product ID must not be an empty GUID.");

            RuleFor(x => x.Title).NotEmpty()
                .WithMessage("Title is required.")
                .MaximumLength(500).WithMessage("Title must not exceed 500 characters.");
            
            RuleFor(x => x.Price).GreaterThan(0)
                .WithMessage("Price must be greater than 0.")
                .PrecisionScale(18, 2, true).WithMessage("Price must have a maximum of 18 digits and 2 decimal places.");

            RuleFor(x => x.Description).NotEmpty()
                .WithMessage("Description is required.")
                .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.");

            RuleFor(x => x.Category).NotEmpty()
                .WithMessage("Category is required.")
                .MaximumLength(100).WithMessage("Category must not exceed 100 characters.");

            RuleFor(x => x.Image).NotEmpty()
                .WithMessage("Image URL is required.")
                .MaximumLength(1000).WithMessage("Image URL must not exceed 1000 characters.");

            RuleFor(x => x.Rating)
                .NotNull().WithMessage("Rating is required.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Rating.Rate)
                        .InclusiveBetween(0, 5).WithMessage("Rating rate must be between 0 and 5.");
                    RuleFor(x => x.Rating.Count)
                        .GreaterThanOrEqualTo(0).WithMessage("Rating count must be greater than or equal to 0.");
                });
        }
    }
}