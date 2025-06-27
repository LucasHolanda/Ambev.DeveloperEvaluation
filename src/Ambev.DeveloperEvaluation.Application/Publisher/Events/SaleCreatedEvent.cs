namespace Ambev.DeveloperEvaluation.Application.Publisher.Events
{
    public record SaleCreatedEvent(Guid SaleId, Guid CartId, DateTime SaleDate, decimal TotalAmount, string SaleNumber)
    { }
}
