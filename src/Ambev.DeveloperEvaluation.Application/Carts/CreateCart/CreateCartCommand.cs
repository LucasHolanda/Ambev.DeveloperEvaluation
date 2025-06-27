using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart
{
    public class CreateCartCommand : IRequest<CartDto>
    {
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }        
        public List<CartProductCommand> CartProducts { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }

    public class CartProductCommand
    {
        public Guid Id { get; set; }
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public CreateProductCommand Product { get; set; } = new();
    }
}