using Ambev.DeveloperEvaluation.Domain.Aggregates;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Aggregates;

public class SaleValidatorTests
{
    [Fact(DisplayName = "Given valid sale When validating Then returns valid result")]
    public void Validate_WithValidSale_ReturnsValidResult()
    {
        // Given
        var sale = new Sale
        {
            CustomerId = Guid.NewGuid(),
            TotalAmount = 100m,
            SaleDate = DateTime.UtcNow,
            SaleNumber = "SALE-20231001-12345678",
            CartId = Guid.NewGuid(),
            SaleItems = new List<SaleItem>
            {
                new SaleItem
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Product A",
                    DiscountPercentage = 0,
                    Quantity = 2,
                    UnitPrice = 50m,
                    TotalAmount = 100m
                }
            }
        };
        var validator = new SaleValidator();

        // When
        var result = validator.Validate(sale);

        // Then
        result.IsValid.Should().BeTrue();
    }

    [Fact(DisplayName = "Given empty sale items When validating Then returns invalid result")]
    public void Validate_WithEmptySaleItems_ReturnsInvalidResult()
    {
        // Given
        var sale = new Sale
        {
            CustomerId = Guid.NewGuid(),
            TotalAmount = 100m,
            SaleItems = new List<SaleItem>()
        };
        var validator = new SaleValidator();

        // When
        var result = validator.Validate(sale);

        // Then
        result.IsValid.Should().BeFalse();
    }
}