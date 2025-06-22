using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface ICartRepository : IBaseRepository<Cart>
    {
        Task<Cart?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
        Task<Cart?> GetWithProductsAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Cart>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    }
}
