using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Domain.Aggregates;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class CancelSaleHandlerTests
{
    private readonly ISaleRepository _repo;

    public CancelSaleHandlerTests()
    {
        _repo = Substitute.For<ISaleRepository>();
    }

    [Fact(DisplayName = "Given valid cancel sale command When cancelling sale Then returns cancelled sale")]
    public async Task Handle_ValidCommand_CancelsSale()
    {
        // Given
        var handler = new CancelSaleHandler(_repo);
        var command = SaleHandlerTestData.GenerateValidCancelCommand();
        var sale = new Sale { Id = command.Id };

        _repo.GetWithItemsAsync(command.Id, default).Returns(sale);
        _repo.UpdateSaleAndItemsAsync(Arg.Any<Sale>(), default).Returns(true);

        // When
        var result = await handler.Handle(command, default);

        // Then
        result.Should().BeTrue();
        await _repo.Received(1).GetWithItemsAsync(command.Id, default);
        await _repo.Received(1).UpdateSaleAndItemsAsync(Arg.Any<Sale>(), default);
    }

    [Fact(DisplayName = "Given invalid cancel sale command When cancelling sale Then throws validation exception")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        // Given
        var handler = new CancelSaleHandler(_repo);
        var command = new CancelSaleCommand(); // Invalid

        // When
        var act = () => handler.Handle(command, default);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }
}