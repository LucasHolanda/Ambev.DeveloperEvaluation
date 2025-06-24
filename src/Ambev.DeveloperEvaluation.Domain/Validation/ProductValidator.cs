using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Description).MaximumLength(2000);
            RuleFor(x => x.Category).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Image).MaximumLength(1000);

            RuleFor(x => x.Rating)
                .NotNull()
                .WithMessage("Rating must not be null.")
                .SetValidator(new InlineValidator<ProductRating>
                {
                    v => v.RuleFor(r => r.Rate).InclusiveBetween(0, 5)
                        .WithMessage("Rate must be between 0 and 5."),
                    v => v.RuleFor(r => r.Count).GreaterThanOrEqualTo(0)
                        .WithMessage("Count must be greater than or equal to 0.")
                });
        }
    }
}