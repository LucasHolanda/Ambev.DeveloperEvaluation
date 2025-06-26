using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories.Mongo;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.ORM.MongoDb.Repositories;

public class BaseMongoRepository<T> : IBaseMongoRepository<T> where T : BaseEntity
{
    public readonly IMongoCollection<T> _collection;
    public BaseMongoRepository(MongoDbContext context)
    {
        _collection = context.GetCollection<T>(typeof(T).Name);
    }

    public async Task<long> CountByIdAsync(Guid id, CancellationToken cancellationToken = default)
    => await _collection.CountDocumentsAsync(c => c.Id == id, cancellationToken: cancellationToken);


    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _collection.Find(_ => true).ToListAsync(cancellationToken);

    public async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _collection.Find(c => c.Id == id).FirstOrDefaultAsync(cancellationToken);

    public async Task AddAsync(T baseMongo, CancellationToken cancellationToken = default)
        => await _collection.InsertOneAsync(baseMongo, new InsertOneOptions { BypassDocumentValidation = true }, cancellationToken);

    public async Task<bool> UpdateAsync(T baseMongo, CancellationToken cancellationToken = default)
    {
        var result = await _collection.ReplaceOneAsync(c => c.Id == baseMongo.Id, baseMongo, new ReplaceOptions { BypassDocumentValidation = true }, cancellationToken);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _collection.DeleteOneAsync(c => c.Id == id, cancellationToken);
        return result.DeletedCount > 0;
    }
}