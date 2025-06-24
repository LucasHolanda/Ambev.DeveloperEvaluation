using Ambev.DeveloperEvaluation.Domain.Enums;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleCommand : IRequest<SaleDto>
    {
        public string SaleNumber { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public Guid CustomerId { get; set; }
        public Guid BranchId { get; set; }
        public decimal TotalAmount { get; set; }
        public SaleStatus Status { get; set; }
        public string? CancelationReason { get; set; }
        public List<CreateSaleItemCommand> SaleItems { get; set; } = new();
    }

    public class CreateSaleItemCommand
    {
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
    }
}