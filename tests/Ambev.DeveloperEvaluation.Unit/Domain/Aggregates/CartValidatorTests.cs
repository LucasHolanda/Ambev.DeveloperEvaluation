using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Aggregates;

public class CartValidatorTests
{
    [Fact]
    public void Validate_WithValidCart_ReturnsValidResult()
    {
        // Given
        var cart = new Cart
        {
            UserId = Guid.NewGuid(),
            Date = DateTime.UtcNow,
            CartProducts = new List<CartProduct>
            {
                new (){
                    ProductId = Guid.NewGuid(),
                    Quantity = 1,
                    Product = new Product
                    {
                        Title = "Product 1",
                        Price = 10.0m
                    }
                }
            }
        };
        var validator = new CartValidator();

        // When
        var result = validator.Validate(cart);

        // Then
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithEmptyCartProducts_ReturnsInvalidResult()
    {
        // Given
        var cart = new Cart
        {
            UserId = Guid.NewGuid(),
            CartProducts = new List<CartProduct>()
        };
        var validator = new CartValidator();

        // When
        var result = validator.Validate(cart);

        // Then
        result.IsValid.Should().BeFalse();
    }
}