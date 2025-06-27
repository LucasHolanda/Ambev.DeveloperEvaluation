using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories.Mongo;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts;

public class CreateCartHandlerTests
{
    private readonly ICartMongoRepository repo;
    private readonly IMapper mapper;
    public CreateCartHandlerTests()
    {
        repo = Substitute.For<ICartMongoRepository>();

        var config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly));
        mapper = config.CreateMapper();
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesCart()
    {
        // Given
        var handler = new CreateCartHandler(repo, mapper);
        var command = CartHandlerTestData.GenerateValidCreateCommand();
        var cart = new Cart
        {
            Id = Guid.NewGuid(),
            UserId = command.UserId,
            CartProducts = command.CartProducts.Select(p => new CartProduct
            {
                Id = Guid.NewGuid(),
                ProductId = p.ProductId,
                Quantity = p.Quantity
            }).ToList()
        };

        repo.AddCartWithProductsAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>())
            .Returns(cart);

        // When
        var result = await handler.Handle(command, default);

        // Then
        result.Should().NotBeNull();
        result.Id.Should().Be(cart.Id);
        await repo.Received(1).AddCartWithProductsAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithEmptyCartProducts_ThrowsException()
    {
        // Given
        var handler = new CreateCartHandler(repo, mapper);
        var command = CartHandlerTestData.GenerateInvalidCreateCommand_EmptyCartProducts();

        // When
        var act = () => handler.Handle(command, default);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }
}