using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Aggregates;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Repositories.Mongo;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class CreateSaleHandlerTests
{
    private readonly ISaleRepository repo;
    private readonly ICartRepository cartRepo;
    private readonly ICartMongoRepository cartMongoRepo;
    private readonly IMapper mapper;

    public CreateSaleHandlerTests()
    {
        repo = Substitute.For<ISaleRepository>();
        cartRepo = Substitute.For<ICartRepository>();
        cartMongoRepo = Substitute.For<ICartMongoRepository>();
        var config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(CreateSaleHandler).Assembly));
        mapper = config.CreateMapper();
    }

    [Fact(DisplayName = "Given valid sale data When creating sale Then returns created sale")]
    public async Task Handle_ValidCommand_CreatesSale()
    {
        // Given
        var handler = new CreateSaleHandler(repo, cartMongoRepo, cartRepo, mapper);
        var command = SaleHandlerTestData.GenerateValidCreateCommand();
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            CustomerId = command.CustomerId,
            CartId = command.CartId,
            SaleDate = command.SaleDate,
            SaleNumber = command.SaleNumber,
            TotalAmount = command.TotalAmount,
            SaleItems = command.SaleItems.Select(i => new SaleItem
            {
                Id = Guid.NewGuid(),
                ProductId = i.ProductId,
                Quantity = i.Quantity
            }).ToList()
        };

        cartMongoRepo.GetByIdAsync(command.CartId, default).Returns(_ => new Cart());
        cartRepo.AddCartWithProductsAsync(Arg.Any<Cart>(), default).Returns(_ => new());
        cartMongoRepo.DeleteAsync(command.CartId, default).Returns(_ => new());
        repo.AddAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        // When
        var createSaleResult = await handler.Handle(command, default);

        // Then
        createSaleResult.Should().NotBeNull();
        createSaleResult.Id.Should().Be(sale.Id);
        await repo.Received(1).AddAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given empty sale items When creating sale Then throws validation exception")]
    public async Task Handle_EmptyItems_ThrowsValidationException()
    {
        // Given
        var handler = new CreateSaleHandler(repo, cartMongoRepo, cartRepo, mapper);
        var command = SaleHandlerTestData.GenerateInvalidCreateCommand_EmptyItems();

        cartMongoRepo.GetByIdAsync(command.CartId, default).ReturnsNull();

        // When
        var act = () => handler.Handle(command, default);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }
}