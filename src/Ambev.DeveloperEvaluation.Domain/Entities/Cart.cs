using Ambev.DeveloperEvaluation.Domain.Aggregates;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Cart : AggregateRoot<Cart>
    {
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<CartProduct> CartProducts { get; set; } = new List<CartProduct>();
    }
}
