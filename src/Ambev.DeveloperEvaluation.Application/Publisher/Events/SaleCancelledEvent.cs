namespace Ambev.DeveloperEvaluation.Application.Publisher.Events
{
    public record SaleCancelledEvent(Guid SaleId, DateTime CancelationDate, string SaleNumber, string Reason)
    { }
}
