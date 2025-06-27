using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.Domain.Aggregates;
using Ambev.DeveloperEvaluation.Domain.Repositories.Mongo;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts;

public class UpdateCartHandlerTests
{
    private readonly ICartMongoRepository _repo;
    private readonly IMapper _mapper;

    public UpdateCartHandlerTests()
    {
        _repo = Substitute.For<ICartMongoRepository>();
        var config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(UpdateCartHandler).Assembly));
        _mapper = config.CreateMapper();
    }

    [Fact(DisplayName = "Given valid update cart command When updating cart Then returns updated cart")]
    public async Task Handle_ValidCommand_UpdatesCart()
    {
        var handler = new UpdateCartHandler(_repo, _mapper);
        var command = CartHandlerTestData.GenerateValidUpdateCommand();
        var cart = new Cart { Id = command.Id };

        _repo.GetByIdAsync(command.Id, default).Returns(cart);
        _repo.UpdateCartAsync(Arg.Any<Cart>(), default).Returns(cart);

        var result = await handler.Handle(command, default);

        result.Should().NotBeNull();
        result!.Id.Should().Be(command.Id);
        await _repo.Received(1).UpdateCartAsync(Arg.Any<Cart>(), default);
    }

    [Fact(DisplayName = "Given invalid update cart command When updating cart Then throws validation exception")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        var handler = new UpdateCartHandler(_repo, _mapper);
        var command = CartHandlerTestData.GenerateInvalidUpdateCommand();

        _repo.GetByIdAsync(command.Id, default).ReturnsNull();

        var act = () => handler.Handle(command, default);

        await act.Should().ThrowAsync<ValidationException>();
    }
}