using Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;
using Ambev.DeveloperEvaluation.Domain.Aggregates;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories.Mongo;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts;

public class DeleteCartHandlerTests
{
    private readonly ICartMongoRepository _repo;

    public DeleteCartHandlerTests()
    {
        _repo = Substitute.For<ICartMongoRepository>();
    }

    [Fact(DisplayName = "Given valid cart id When deleting cart Then returns true")]
    public async Task Handle_ValidId_DeletesCart()
    {
        var handler = new DeleteCartHandler(_repo);
        var command = CartHandlerTestData.GenerateValidDeleteCommand();
        var cart = new Cart { Id = command.Id };

        _repo.DeleteAllCartAsync(command.Id, default).Returns(true);
        _repo.GetByIdAsync(command.Id, default).Returns(cart);

        var result = await handler.Handle(command, default);

        result.Should().BeTrue();
        await _repo.Received(1).DeleteAllCartAsync(command.Id, default);
    }

    [Fact(DisplayName = "Given invalid cart id When deleting cart Then returns false")]
    public async Task Handle_InvalidId_ReturnsFalse()
    {
        var handler = new DeleteCartHandler(_repo);
        var command = CartHandlerTestData.GenerateInvalidDeleteCommand();
        _repo.DeleteAllCartAsync(command.Id, default).Returns(false);

        // When
        var act = () => handler.Handle(command, default);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }
}