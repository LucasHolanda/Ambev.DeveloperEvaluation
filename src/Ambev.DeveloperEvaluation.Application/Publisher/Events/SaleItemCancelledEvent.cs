namespace Ambev.DeveloperEvaluation.Application.Publisher.Events
{
    public record SaleItemCancelledEvent(Guid SaleId, Guid SaleItemId, Guid ProductId, int Quantity, string? CancelationReason, DateTime? CancelationDate);
}
