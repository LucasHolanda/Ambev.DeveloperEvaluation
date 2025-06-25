using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<int> CountByIdAsync(Guid Id, CancellationToken cancellationToken = default);
        Task<List<T>> GetByQueryParameters(QueryParameters parameters, Func<IQueryable<T>, IQueryable<T>>? include = null, CancellationToken cancellationToken = default);
        Task<int> CountByQueryParametersAsync(QueryParameters parameters, CancellationToken cancellationToken = default);
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);        
    }

}
