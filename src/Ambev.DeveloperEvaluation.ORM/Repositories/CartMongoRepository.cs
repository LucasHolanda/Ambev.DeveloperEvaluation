using Ambev.DeveloperEvaluation.Domain.Aggregates;
using Ambev.DeveloperEvaluation.Domain.Repositories.Mongo;
using Ambev.DeveloperEvaluation.ORM.MongoDb;
using Ambev.DeveloperEvaluation.ORM.MongoDb.Repositories;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class CartMongoRepository : BaseMongoRepository<Cart>, ICartMongoRepository
    {
        public CartMongoRepository(MongoDbContext context) : base(context) { }

        public async Task<Cart?> AddCartWithProductsAsync(Cart cart, CancellationToken cancellationToken = default)
        {
            await AddAsync(cart, cancellationToken);
            return await GetByIdAsync(cart.MongoId, cancellationToken);
        }

        public async Task<Cart?> UpdateCartAsync(Cart cart, CancellationToken cancellationToken = default)
        {
            var result = await UpdateAsync(cart, cancellationToken);

            if (result)
                return await GetByIdAsync(cart.MongoId, cancellationToken);

            return null;
        }

        public async Task<IEnumerable<Cart>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Cart>.Filter.Gte(c => c.CreatedAt, startDate) &
                         Builders<Cart>.Filter.Lte(c => c.CreatedAt, endDate);

            return await _collection.Find(filter).ToListAsync(cancellationToken);
        }

        public async Task<Cart?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Cart>.Filter.Eq(c => c.UserId, userId);
            return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> DeleteCartProductAsync(Guid cartProductId, CancellationToken cancellationToken = default)
        {
            var update = Builders<Cart>.Update.PullFilter(
                c => c.CartProducts, cp => cp.Id == cartProductId);

            var result = await _collection.UpdateOneAsync(
                c => c.CartProducts.Any(cp => cp.Id == cartProductId),
                update,
                cancellationToken: cancellationToken);

            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAllCartAsync(Guid cartId, CancellationToken cancellationToken = default)
        {
            return await DeleteAsync(cartId, cancellationToken);
        }
    }
}