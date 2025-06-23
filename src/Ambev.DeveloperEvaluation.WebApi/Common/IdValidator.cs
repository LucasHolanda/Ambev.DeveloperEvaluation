using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Common
{
    public class IdValidator : AbstractValidator<Guid>
    {
        public IdValidator()
        {
            RuleFor(x => x).NotEmpty()
                .WithMessage("Product ID is required.")
                .Must(id => id != Guid.Empty)
                .WithMessage("Product ID must not be an empty GUID.");
        }
    }
}