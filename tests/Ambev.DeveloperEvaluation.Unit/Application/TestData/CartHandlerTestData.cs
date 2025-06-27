using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;
using Ambev.DeveloperEvaluation.Application.Carts.GetCart;
using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

public static class CartHandlerTestData
{
    private static readonly Faker<CreateCartCommand> createCartHandlerFaker = new Faker<CreateCartCommand>()
    .RuleFor(c => c.UserId, f => f.Random.Guid())
    .RuleFor(c => c.Date, f => f.Date.Past(1))
    .RuleFor(c => c.CartProducts, f => new List<CartProductCommand> {
            new CartProductCommand{ ProductId = f.Random.Guid(), Quantity = f.Random.Int(1, 5),
                Product = new CreateProductCommand
                {
                    Title = f.Commerce.ProductName(),
                    Price = f.Finance.Amount(1.0m, 100.0m)
                }
            }

    });

    private static readonly Faker<GetCartByIdCommand> getCartByIdFaker = new Faker<GetCartByIdCommand>()
        .RuleFor(c => c.Id, f => f.Random.Guid());

    private static readonly Faker<UpdateCartCommand> updateCartFaker = new Faker<UpdateCartCommand>()
        .RuleFor(c => c.Id, f => f.Random.Guid())
        .RuleFor(c => c.CartProducts, f => new List<CartProductCommand>
        {
            new CartProductCommand
            {
                ProductId = f.Random.Guid(),
                Quantity = f.Random.Int(1, 5),
                Product = new CreateProductCommand
                {
                    Title = f.Commerce.ProductName(),
                    Price = f.Finance.Amount(1.0m, 100.0m)
                }
            }
        });

    private static readonly Faker<DeleteCartCommand> deleteCartFaker = new Faker<DeleteCartCommand>()
        .RuleFor(c => c.Id, f => f.Random.Guid());


    /// <summary>
    /// Generates a valid CreateCartCommand with randomized data.
    /// </summary>
    public static CreateCartCommand GenerateValidCreateCommand()
    {
        return createCartHandlerFaker.Generate();
    }

    /// <summary>
    /// Generates an invalid CreateCartCommand (empty CartProducts).
    /// </summary>
    public static CreateCartCommand GenerateInvalidCreateCommand_EmptyCartProducts()
    {
        var command = createCartHandlerFaker.Generate();
        command.CartProducts = new List<CartProductCommand>();
        return command;
    }

    public static GetCartByIdCommand GenerateValidGetByIdCommand() => getCartByIdFaker.Generate();

    public static GetCartByIdCommand GenerateInvalidGetByIdCommand() => new GetCartByIdCommand(); // Id vazio

    public static UpdateCartCommand GenerateValidUpdateCommand() => updateCartFaker.Generate();

    public static UpdateCartCommand GenerateInvalidUpdateCommand()
    {
        var cmd = updateCartFaker.Generate();
        cmd.CartProducts = new(); // inválido: sem produtos
        return cmd;
    }

    public static DeleteCartCommand GenerateValidDeleteCommand() => deleteCartFaker.Generate();

    public static DeleteCartCommand GenerateInvalidDeleteCommand() => new DeleteCartCommand(); // Id vazio
}