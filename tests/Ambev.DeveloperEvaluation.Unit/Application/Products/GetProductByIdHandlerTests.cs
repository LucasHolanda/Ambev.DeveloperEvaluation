using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

public class GetProductByIdHandlerTests
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;

    public GetProductByIdHandlerTests()
    {
        _repo = Substitute.For<IProductRepository>();
        var config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(GetProductByIdHandler).Assembly));
        _mapper = config.CreateMapper();
    }

    [Fact(DisplayName = "Given valid product id When getting product Then returns product")]
    public async Task Handle_ValidId_ReturnsProduct()
    {
        // Given
        var handler = new GetProductByIdHandler(_repo, _mapper);
        var command = ProductHandlerTestData.GenerateValidGetCommand();
        var product = new Product { Id = command.Id };
        _repo.GetByIdAsync(command.Id, default).Returns(product);

        // When
        var result = await handler.Handle(command, default);

        // Then
        result.Should().NotBeNull();
        result.Id.Should().Be(product.Id);
        await _repo.Received(1).GetByIdAsync(command.Id, default);
    }

    [Fact(DisplayName = "Given invalid product id When getting product Then returns null")]
    public async Task Handle_InvalidId_ReturnsNull()
    {
        // Given
        var handler = new GetProductByIdHandler(_repo, _mapper);
        var command = ProductHandlerTestData.GenerateInvalidGetCommand();
        _repo.GetByIdAsync(command.Id, default).ReturnsNull();

        // When
        var act = () => handler.Handle(command, default);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }
}