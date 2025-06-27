using Ambev.DeveloperEvaluation.Application.Sales.GetSaleById;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class GetSaleByIdHandlerTests
{
    private readonly ISaleRepository _repo;
    private readonly IMapper _mapper;

    public GetSaleByIdHandlerTests()
    {
        _repo = Substitute.For<ISaleRepository>();
        var config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(GetSaleByIdHandler).Assembly));
        _mapper = config.CreateMapper();
    }

    [Fact(DisplayName = "Given valid sale id When getting sale Then returns sale")]
    public async Task Handle_ValidId_ReturnsSale()
    {
        // Given
        var handler = new GetSaleByIdHandler(_repo, _mapper);
        var command = SaleHandlerTestData.GenerateValidGetCommand();
        var sale = new Sale { Id = command.Id };
        _repo.GetWithItemsAndProductsAsync(command.Id, default).Returns(sale);

        // When
        var result = await handler.Handle(command, default);

        // Then
        result.Should().NotBeNull();
        result!.Id.Should().Be(sale.Id);
        await _repo.Received(1).GetWithItemsAndProductsAsync(sale.Id, default);
    }

    [Fact(DisplayName = "Given invalid sale id When getting sale Then returns null")]
    public async Task Handle_InvalidId_ReturnsNull()
    {
        // Given
        var handler = new GetSaleByIdHandler(_repo, _mapper);
        var invalidCommand = SaleHandlerTestData.GenerateInvalidGetCommand();

        // When
        var result = await handler.Handle(invalidCommand, default);

        // Then
        result.Should().BeNull();
    }
}