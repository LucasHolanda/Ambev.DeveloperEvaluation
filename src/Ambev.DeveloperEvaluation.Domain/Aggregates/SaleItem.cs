using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates
{
    public class SaleItem : BaseEntity
    {
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty; 
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal TotalAmount { get; set; }
        public SaleItemStatus Status { get; set; } = SaleItemStatus.Active;
        public string? CancelationReason { get; set; }
        public DateTime? CancelationDate { get; set; }

        public virtual Sale Sale { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;

        public void Cancel(string? reason = null)
        {
            Status = SaleItemStatus.Cancelled;
            CancelationReason = reason;
            CancelationDate = DateTime.UtcNow;
        }
    }
}
