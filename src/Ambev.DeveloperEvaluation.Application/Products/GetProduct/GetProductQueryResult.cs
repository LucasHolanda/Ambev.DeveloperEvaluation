namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct
{
    public class GetProductQueryResult
    {
        public int TotalCount { get; set; }
        public List<ProductResult> Products { get; set; } = new();
    }
}
