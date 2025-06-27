using Ambev.DeveloperEvaluation.Application.Products;

namespace Ambev.DeveloperEvaluation.Application.Sales
{
    public class SaleDto
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public Guid CustomerId { get; set; }
        public Guid CartId { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsCancelled { get; set; }
        public string StatusName => IsCancelled ? "Cancelled" : "Active";
        public string? CancelationReason { get; set; }
        public DateTime? CancelationDate { get; set; }

        public List<SaleItemDto> SaleItems { get; set; } = new();
    }

    public class SaleItemDto
    {
        public Guid Id { get; set; }
        public Guid SaleId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsCancelled { get; set; }
        public string StatusName => IsCancelled ? "Cancelled" : "Active";
        public string? CancelationReason { get; set; }
        public DateTime? CancelationDate { get; set; }
        public ProductDto Product { get; set; } = new();
    }
}
