using Ambev.DeveloperEvaluation.Domain.Aggregates;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public record SaleModifiedEvent(int SaleId, string ChangeDescription, decimal NewTotalAmount) : IDomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}
