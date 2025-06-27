using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

public class CreateProductHandlerTests
{
    private readonly IProductRepository repo;
    private readonly IMapper mapper;

    public CreateProductHandlerTests()
    {
        repo = Substitute.For<IProductRepository>();
        var config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(CreateProductHandler).Assembly));
        mapper = config.CreateMapper();
    }

    [Fact(DisplayName = "Given valid product data When creating product Then returns created product")]
    public async Task Handle_ValidCommand_CreatesProduct()
    {
        // Given
        var handler = new CreateProductHandler(repo, mapper);
        var command = ProductHandlerTestData.GenerateValidCreateCommand();
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Title = command.Title,
            Description = command.Description,
            Price = command.Price
        };

        repo.AddAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>())
            .Returns(product);

        // When
        var result = await handler.Handle(command, default);

        // Then
        result.Should().NotBeNull();
        result.Id.Should().Be(product.Id);
        await repo.Received(1).AddAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given empty product title When creating product Then throws validation exception")]
    public async Task Handle_EmptyTitle_ThrowsValidationException()
    {
        // Given
        var handler = new CreateProductHandler(repo, mapper);
        var command = ProductHandlerTestData.GenerateInvalidCreateCommand_EmptyTitle();

        // When
        var act = () => handler.Handle(command, default);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }
}