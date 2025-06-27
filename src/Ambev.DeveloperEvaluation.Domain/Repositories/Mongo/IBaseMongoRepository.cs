using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Repositories.Mongo
{
    public interface IBaseMongoRepository<T> where T : BaseEntity
    {
        Task<long> CountByIdAsync(Guid mongoId, CancellationToken cancellationToken = default);
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(T baseMongo, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(T baseMongo, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}