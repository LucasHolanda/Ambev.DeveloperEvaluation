namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSalesQueryDto
    {
        public List<SaleDto> Sales { get; set; } = new();
        public int TotalCount { get; set; }
    }
}