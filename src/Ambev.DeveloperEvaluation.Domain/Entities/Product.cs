using Ambev.DeveloperEvaluation.Domain.Aggregates;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Product : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public ProductRating Rating { get; set; } = new();

    public virtual ICollection<CartProduct> CartProducts { get; set; } = new List<CartProduct>();
    public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}

public class ProductRating
{
    public decimal Rate { get; set; }
    public int Count { get; set; }
}