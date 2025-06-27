using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

public static class ProductHandlerTestData
{
    private static readonly Faker<UpdateProductCommand> updateProductFaker = new Faker<UpdateProductCommand>()
        .RuleFor(p => p.Id, f => f.Random.Guid())
        .RuleFor(p => p.Title, f => f.Commerce.ProductName())
        .RuleFor(p => p.Price, f => f.Random.Decimal(1, 1000))
        .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
        .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
        .RuleFor(p => p.Image, f => f.Image.PicsumUrl());

    private static readonly Faker<CreateProductCommand> createProductFaker = new Faker<CreateProductCommand>()
        .RuleFor(p => p.Title, f => f.Commerce.ProductName())
        .RuleFor(p => p.Price, f => f.Random.Decimal(1, 1000))
        .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
        .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
        .RuleFor(p => p.Image, f => f.Image.PicsumUrl());

    public static CreateProductCommand GenerateValidCreateCommand()
        => createProductFaker.Generate();

    public static CreateProductCommand GenerateInvalidCreateCommand_EmptyTitle()
    {
        var cmd = createProductFaker.Generate();
        cmd.Title = string.Empty;
        return cmd;
    }

    public static GetProductByIdCommand GenerateValidGetCommand() => new GetProductByIdCommand(Guid.NewGuid());

    public static GetProductByIdCommand GenerateInvalidGetCommand() => new GetProductByIdCommand(Guid.Empty); // Id vazio

    public static UpdateProductCommand GenerateValidUpdateCommand() => updateProductFaker.Generate();

    public static UpdateProductCommand GenerateInvalidUpdateCommand()
    {
        var cmd = updateProductFaker.Generate();
        cmd.Title = string.Empty; // inválido: título vazio
        return cmd;
    }

    public static DeleteProductByIdCommand GenerateValidDeleteCommand() => new DeleteProductByIdCommand(Guid.NewGuid());

    public static DeleteProductByIdCommand GenerateInvalidDeleteCommand() => new DeleteProductByIdCommand(Guid.Empty); // Id vazio
}