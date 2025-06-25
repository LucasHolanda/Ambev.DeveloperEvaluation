using Ambev.DeveloperEvaluation.Domain.Aggregates;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface ICartRepository : IBaseRepository<Cart>
    {
        Task<Cart?> AddCartWithProductsAsync(Cart cart, CancellationToken cancellationToken = default);
        Task<Cart?> UpdateCartAsync(Cart cart, CancellationToken cancellationToken = default);
        Task<bool> DeleteCartProductAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> DeleteAllCartAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Cart?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Cart?> GetValidCartWithProductsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Cart>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    }
}
