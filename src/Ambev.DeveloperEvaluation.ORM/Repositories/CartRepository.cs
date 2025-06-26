using Ambev.DeveloperEvaluation.Domain.Aggregates;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class CartRepository : BaseRepository<Cart>, ICartRepository
    {
        public CartRepository(PostgresContext context) : base(context) { }

        public async Task<IEnumerable<Cart>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(c => c.CreatedAt >= startDate && c.CreatedAt <= endDate).ToListAsync(cancellationToken);
        }

        public async Task<Cart?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(c => c.CartProducts)
                .ThenInclude(cp => cp.Product)
                .Where(c => c.UserId == userId).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Cart?> GetValidCartWithProductsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(c => c.CartProducts)
                .ThenInclude(cp => cp.Product)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<bool> DeleteCartProductAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _context.Set<CartProduct>().Where(e => e.Id == id)
               .ExecuteDeleteAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DeleteAllCartAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _context.Set<CartProduct>().Where(e => e.CartId == id)
               .ExecuteDeleteAsync(cancellationToken);

            await DeleteAsync(id, cancellationToken);

            return true;
        }
    }
}