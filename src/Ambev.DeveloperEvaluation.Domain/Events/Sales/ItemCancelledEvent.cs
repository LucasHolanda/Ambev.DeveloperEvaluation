using Ambev.DeveloperEvaluation.Domain.Aggregates;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public record ItemCancelledEvent(Guid SaleId, Guid ItemId, string ProductName, int Quantity) : IDomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}
