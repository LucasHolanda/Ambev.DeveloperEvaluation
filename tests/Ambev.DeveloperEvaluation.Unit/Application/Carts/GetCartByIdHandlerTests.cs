using Ambev.DeveloperEvaluation.Application.Carts.GetCart;
using Ambev.DeveloperEvaluation.Domain.Aggregates;
using Ambev.DeveloperEvaluation.Domain.Repositories.Mongo;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts;

public class GetCartByIdHandlerTests
{
    private readonly ICartMongoRepository _repo;
    private readonly IMapper _mapper;

    public GetCartByIdHandlerTests()
    {
        _repo = Substitute.For<ICartMongoRepository>();
        var config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(GetCartByIdHandler).Assembly));
        _mapper = config.CreateMapper();
    }

    [Fact(DisplayName = "Given valid cart id When getting cart Then returns cart")]
    public async Task Handle_ValidId_ReturnsCart()
    {
        // Given
        var handler = new GetCartByIdHandler(_repo, _mapper);
        var command = CartHandlerTestData.GenerateValidGetByIdCommand();

        var cart = new Cart { Id = command.Id };
        _repo.GetByIdAsync(command.Id, default).Returns(cart);

        // When
        var result = await handler.Handle(command, default);

        // Then
        result.Should().NotBeNull();
        result!.Id.Should().Be(cart.Id);
        await _repo.Received(1).GetByIdAsync(cart.Id, default);
    }

    [Fact(DisplayName = "Given invalid cart id When getting cart Then returns null")]
    public async Task Handle_InvalidId_ReturnsNull()
    {
        // Given
        var handler = new GetCartByIdHandler(_repo, _mapper);
        var command = CartHandlerTestData.GenerateInvalidGetByIdCommand();
        _repo.GetByIdAsync(command.Id, default).ReturnsNull();

        // When
        var result = await handler.Handle(command, default);

        // Then
        result.Should().BeNull();
    }
}