using Ambev.DeveloperEvaluation.Domain.Aggregates;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public record ItemAddedToSaleEvent(Guid SaleId, SaleItem Item) : IDomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}
