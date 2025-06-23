namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart
{
    public class GetCartsQueryDto
    {
        public List<CartDto> Carts { get; set; } = new();
        public int TotalCount { get; set; }
    }
}