using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Cart : BaseEntity
    {
        public Guid UserId { get; set; }
        public int BranchId { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<CartProduct> CartProducts { get; set; } = new List<CartProduct>();
        public virtual Sale Sale { get; set; } = null!;

        public ValidationResultDetail Validate()
        {
            var validator = new CartValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }

        public void GenerateCartProductIds()
        {
            foreach (var cartProduct in CartProducts)
            {
                if (cartProduct.Id == Guid.Empty)
                {
                    cartProduct.Id = Guid.NewGuid();
                }
            }
        }
    }
}
