using Ambev.DeveloperEvaluation.Domain.Aggregates;

namespace Ambev.DeveloperEvaluation.Domain.Repositories.Mongo
{
    public interface ICartMongoRepository : IBaseMongoRepository<Cart>
    {
        Task<Cart?> GetByCarProductIdAsync(Guid cartProductId, CancellationToken cancellationToken = default);
        Task<Cart?> AddCartWithProductsAsync(Cart cart, CancellationToken cancellationToken = default);
        Task<Cart?> UpdateCartAsync(Cart cart, CancellationToken cancellationToken = default);
        Task<bool> DeleteCartProductAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> DeleteAllCartAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Cart?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Cart>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    }
}
