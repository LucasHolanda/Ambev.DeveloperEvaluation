using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

public class UpdateProductHandlerTests
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;

    public UpdateProductHandlerTests()
    {
        _repo = Substitute.For<IProductRepository>();
        var config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(UpdateProductHandler).Assembly));
        _mapper = config.CreateMapper();
    }

    [Fact(DisplayName = "Given valid update product command When updating product Then returns updated product")]
    public async Task Handle_ValidCommand_UpdatesProduct()
    {
        // Given
        var handler = new UpdateProductHandler(_repo, _mapper);
        var command = ProductHandlerTestData.GenerateValidUpdateCommand();
        var product = new Product { Id = command.Id, Title = command.Title, Price = command.Price };

        _repo.GetByIdAsync(command.Id, default).Returns(product);
        _repo.UpdateAsync(Arg.Any<Product>(), default).Returns(_ => Task.FromResult(product));

        // When
        var result = await handler.Handle(command, default);

        // Then
        result.Should().NotBeNull();
        result.Id.Should().Be(product.Id);
        await _repo.Received(1).UpdateAsync(Arg.Any<Product>(), default);
    }

    [Fact(DisplayName = "Given invalid update product command When updating product Then throws validation exception")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        // Given
        var handler = new UpdateProductHandler(_repo, _mapper);
        var command = ProductHandlerTestData.GenerateInvalidUpdateCommand();

        // When
        var act = () => handler.Handle(command, default);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }
}