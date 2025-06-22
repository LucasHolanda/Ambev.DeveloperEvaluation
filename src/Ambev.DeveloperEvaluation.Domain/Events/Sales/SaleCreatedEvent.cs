using Ambev.DeveloperEvaluation.Domain.Aggregates;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public record SaleCreatedEvent(int SaleId, string SaleNumber, int CustomerId, decimal TotalAmount) : IDomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}
