using Ambev.DeveloperEvaluation.Application.Products;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Sales
{
    public class SaleDto
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public Guid CustomerId { get; set; }
        public Branch BranchId { get; set; }
        public string BranchName => BranchId.ToString();
        public decimal TotalAmount { get; set; }
        public SaleStatus Status { get; set; } = SaleStatus.Active;
        public string? CancelationReason { get; set; }
        public DateTime? CancelationDate { get; set; }

        public List<CreateSaleItemCommand> SaleItems { get; set; } = new();
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
        public SaleItemStatus Status { get; set; } = SaleItemStatus.Active;
        public string? CancelationReason { get; set; }
        public DateTime? CancelationDate { get; set; }

        public SaleDto Sale { get; set; } = new();
        public ProductDto Product { get; set; } = new();
    }
}
