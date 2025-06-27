using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates
{
    public class Sale : AggregateRoot<Sale>
    {
        public string SaleNumber { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public Guid CustomerId { get; set; }
        public Guid CartId { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsCancelled { get; set; }
        public string? CancelationReason { get; set; }
        public DateTime? CancelationDate { get; set; }

        public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
        public virtual Cart Cart { get; set; } = null!;

        private List<string> SaleItemValidationErrors { get; set; } = new List<string>();
        public ValidationResultDetail Validate()
        {
            var validator = new SaleValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }

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
                TotalAmount = totalItemAmount
            };

            var saleItemValidationResult = saleItem.Validate();
            if (!saleItemValidationResult.IsValid)
            {
                SaleItemValidationErrors.Add(string.Join(", ", saleItemValidationResult.Errors.Select(e => e.Detail)));
            }
            else
            {
                SaleItems.Add(saleItem);
                RecalculateTotal();

                AddDomainEvent(new ItemAddedToSaleEvent(Id, saleItem));
            }
        }

        public void CancelSaleAndItems(string reason)
        {
            IsCancelled = true;
            CancelationReason = reason;
            CancelationDate = DateTime.UtcNow;

            foreach (var item in SaleItems.Where(i => !i.IsCancelled))
            {
                item.Cancel();
            }

            AddDomainEvent(new SaleCancelledEvent(Id, reason));
        }

        public void CancelItem(Guid saleItemId, string reason)
        {
            var item = SaleItems.FirstOrDefault(i => i.Id == saleItemId) ?? throw new ValidationException("Item not found");

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
            TotalAmount = SaleItems.Where(i => !i.IsCancelled)
                                  .Sum(i => i.TotalAmount);
        }

        public static Sale CreateByCart(Cart cart)
        {
            var sale = new Sale
            {
                SaleNumber = GenerateSaleNumber(),
                SaleDate = cart.Date,
                CustomerId = cart.UserId,
                CartId = cart.Id,
                TotalAmount = 0
            };

            var saleValidationResult = sale.Validate();
            if (!saleValidationResult.IsValid)
            {
                throw new DomainException("Sale validation failed: " + string.Join(", ", saleValidationResult.Errors.Select(e => e.Detail)));
            }

            foreach (var cartProduct in cart.CartProducts)
            {
                sale.AddItem(cartProduct.ProductId, cartProduct.Product.Title, cartProduct.Quantity, cartProduct.Product.Price);
            }

            if (sale.SaleItemValidationErrors.Any())
            {
                throw new DomainException("Sale item validation failed: " + string.Join(", ", sale.SaleItemValidationErrors));
            }

            return sale;
        }

        private static string GenerateSaleNumber()
        {
            return $"SALE-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }
    }
}
