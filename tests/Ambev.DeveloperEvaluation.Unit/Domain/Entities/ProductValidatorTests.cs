using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class ProductValidatorTests
{
    [Fact(DisplayName = "Given valid product When validating Then returns valid result")]
    public void Validate_WithValidProduct_ReturnsValidResult()
    {
        // Given
        var product = new Product
        {
            Title = "Cerveja",
            Price = 10.5m,
            Description = "Cerveja gelada",
            Category = "Bebidas",
            Image = "https://img.com/beer.png"
        };
        var validator = new ProductValidator();

        // When
        var result = validator.Validate(product);

        // Then
        result.IsValid.Should().BeTrue();
    }

    [Fact(DisplayName = "Given empty product title When validating Then returns invalid result")]
    public void Validate_WithEmptyTitle_ReturnsInvalidResult()
    {
        // Given
        var product = new Product
        {
            Title = "",
            Price = 10.5m,
            Description = "Cerveja gelada",
            Category = "Bebidas",
            Image = "https://img.com/beer.png"
        };
        var validator = new ProductValidator();

        // When
        var result = validator.Validate(product);

        // Then
        result.IsValid.Should().BeFalse();
    }
}