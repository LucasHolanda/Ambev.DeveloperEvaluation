using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

public class DeleteProductHandlerTests
{
    private readonly IProductRepository _repo;

    public DeleteProductHandlerTests()
    {
        _repo = Substitute.For<IProductRepository>();
    }

    [Fact(DisplayName = "Given valid product id When deleting product Then returns true")]
    public async Task Handle_ValidId_DeletesProduct()
    {
        // Given
        var handler = new DeleteProductHandler(_repo);
        var command = ProductHandlerTestData.GenerateValidDeleteCommand();
        var product = new Product { Id = command.Id };

        _repo.GetByIdAsync(command.Id, default).Returns(product);
        _repo.DeleteAsync(command.Id, default).Returns(true);

        // When
        var result = await handler.Handle(command, default);

        // Then
        result.Should().BeTrue();
        await _repo.Received(1).DeleteAsync(command.Id, default);
    }

    [Fact(DisplayName = "Given invalid product id When deleting product Then returns false")]
    public async Task Handle_InvalidId_ReturnsFalse()
    {
        // Given
        var handler = new DeleteProductHandler(_repo);
        var command = ProductHandlerTestData.GenerateInvalidDeleteCommand();

        _repo.GetByIdAsync(command.Id, default).ReturnsNull();
        _repo.DeleteAsync(command.Id, default).Returns(false);

        // When
        var act = () => handler.Handle(command, default);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }
}