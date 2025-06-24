using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleCommand : CreateSaleCommand
    {
        public Guid Id { get; set; }
    }

    public class UpdateSaleItemCommand : CreateSaleItemCommand
    {
        public Guid Id { get; set; }
    }
}