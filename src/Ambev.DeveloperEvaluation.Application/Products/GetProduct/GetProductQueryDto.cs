namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct
{
    public class GetProductQueryDto
    {
        public int TotalCount { get; set; }
        public List<ProductDto> Products { get; set; } = new();
    }
}
