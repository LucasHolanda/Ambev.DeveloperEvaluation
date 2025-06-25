using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Carts
{
    public class CartDto
    {
        public Guid Id { get; set; }
        public int BranchId { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public List<CartProductDto> CartProducts { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }

    public class CartProductDto
    {
        public Guid Id { get; set; }
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}