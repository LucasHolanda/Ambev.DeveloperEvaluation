using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface ISaleRepository : IBaseRepository<Sale>
    {
        Task<IEnumerable<Sale?>> GetAllByQuery(QueryParameters queryParameters, CancellationToken cancellationToken = default);
        Task<bool> UpdateSaleAndItemsAsync(Sale Sale, CancellationToken cancellationToken = default);
        Task<Sale?> AddSaleWithItemsAsync(Sale Sale, CancellationToken cancellationToken = default);
        Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default);
        Task<Sale?> GetWithItemsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Sale?> GetWithItemsAndProductsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Sale>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<IEnumerable<Sale>> GetActiveSalesAsync(CancellationToken cancellationToken = default);
    }
}
