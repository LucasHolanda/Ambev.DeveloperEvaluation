using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSaleById;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

public static class SaleHandlerTestData
{
    private static readonly Faker<GetSaleByIdCommand> getSaleByIdFaker = new Faker<GetSaleByIdCommand>()
        .RuleFor(s => s.Id, f => f.Random.Guid());

    private static readonly Faker<CancelSaleCommand> cancelSaleFaker = new Faker<CancelSaleCommand>()
        .RuleFor(s => s.Id, f => f.Random.Guid());

    private static readonly Faker<CreateSaleCommand> createSaleFaker = new Faker<CreateSaleCommand>()
        .RuleFor(s => s.CustomerId, f => f.Random.Guid())
        .RuleFor(s => s.CartId, f => f.Random.Guid())
        .RuleFor(s => s.SaleDate, f => f.Date.Past(1))
        .RuleFor(s => s.SaleNumber, f => f.Commerce.Ean13())
        .RuleFor(s => s.TotalAmount, f => f.Random.Decimal(10, 1000))
        .RuleFor(s => s.SaleItems, f => new List<CreateSaleItemCommand>
        {
            new CreateSaleItemCommand
            {
                ProductId = f.Random.Guid(),
                Quantity = f.Random.Int(1, 10),
                UnitPrice = f.Random.Decimal(1, 100)
            }
        });

    public static CreateSaleCommand GenerateValidCreateCommand()
        => createSaleFaker.Generate();

    public static CreateSaleCommand GenerateInvalidCreateCommand_EmptyItems()
    {
        var cmd = createSaleFaker.Generate();
        cmd.SaleItems = new List<CreateSaleItemCommand>();
        return cmd;
    }

    public static GetSaleByIdCommand GenerateValidGetCommand() => getSaleByIdFaker.Generate();

    public static GetSaleByIdCommand GenerateInvalidGetCommand() => new GetSaleByIdCommand(); // Id vazio

    public static CancelSaleCommand GenerateValidCancelCommand() => cancelSaleFaker.Generate();

    public static CancelSaleCommand GenerateInvalidCancelCommand() => new CancelSaleCommand(); // Id vazio
}