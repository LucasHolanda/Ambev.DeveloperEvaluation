using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates
{
    public class Sale : AggregateRoot<Sale>
    {
        public string SaleNumber { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public int BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public SaleStatus Status { get; set; } = SaleStatus.Active;
        public string? CancelationReason { get; set; }
        public DateTime? CancelationDate { get; set; }

        public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();

        public void AddItem(Guid productId, string productName, int quantity, decimal unitPrice)
        {
            ValidateQuantity(quantity);

            var discount = CalculateDiscount(quantity);
            var totalItemAmount = (unitPrice * quantity) * (1 - discount / 100);

            var saleItem = new SaleItem
            {
                ProductId = productId,
                ProductName = productName,
                Quantity = quantity,
                UnitPrice = unitPrice,
                DiscountPercentage = discount,
                TotalAmount = totalItemAmount,
                Status = SaleItemStatus.Active
            };

            SaleItems.Add(saleItem);
            RecalculateTotal();

            AddDomainEvent(new ItemAddedToSaleEvent(Id, saleItem));
        }

        public void CancelSale(string reason)
        {
            Status = SaleStatus.Cancelled;
            CancelationReason = reason;
            CancelationDate = DateTime.UtcNow;

            foreach (var item in SaleItems.Where(i => i.Status == SaleItemStatus.Active))
            {
                item.Cancel();
            }

            AddDomainEvent(new SaleCancelledEvent(Id, reason));
        }

        public void CancelItem(Guid saleItemId, string reason)
        {
            var item = SaleItems.FirstOrDefault(i => i.Id == saleItemId) ?? throw new InvalidOperationException("Item not found");

            item.Cancel(reason);
            RecalculateTotal();

            AddDomainEvent(new ItemCancelledEvent(Id, saleItemId, item.ProductName, item.Quantity));
        }

        private static void ValidateQuantity(int quantity)
        {
            if (quantity > 20)
                throw new DomainException("Cannot sell more than 20 identical items");
            if (quantity <= 0)
                throw new DomainException("Quantity must be greater than zero");
        }

        private static decimal CalculateDiscount(int quantity)
        {
            return quantity switch
            {
                >= 10 and <= 20 => 20m,
                >= 4 => 10m,
                _ => 0m
            };
        }

        private void RecalculateTotal()
        {
            TotalAmount = SaleItems.Where(i => i.Status == SaleItemStatus.Active)
                                  .Sum(i => i.TotalAmount);
        }
    }
}
