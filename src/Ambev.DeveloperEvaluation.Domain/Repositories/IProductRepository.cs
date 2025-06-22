using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<IEnumerable<Product>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default);
        Task<IEnumerable<Product>> SearchByTitleAsync(string title, CancellationToken cancellationToken = default);
        Task<Product?> GetByIdWithRatingAsync(int id, CancellationToken cancellationToken = default);
    }
}
