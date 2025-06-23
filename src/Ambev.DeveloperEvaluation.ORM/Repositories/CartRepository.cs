using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class CartRepository : BaseRepository<Cart>, ICartRepository
    {
        public CartRepository(PostgresContext context) : base(context) { }

        public async Task<Cart?> AddCartWithProductsAsync(Cart cart, CancellationToken cancellationToken = default)
        {
            var cartAdd = await _dbSet.AddAsync(cart, cancellationToken);
            foreach (var cartProduct in cart.CartProducts)
            {
                cartProduct.CartId = cartAdd.Entity.Id;
            }

            await _context.Set<CartProduct>().AddRangeAsync(cart.CartProducts, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return await GetWithProductsAsync(cartAdd.Entity.Id, cancellationToken);
        }

        public async Task<Cart?> UpdateCartAsync(Cart cart, CancellationToken cancellationToken = default)
        {
            await _context.Set<CartProduct>().Where(e => e.CartId == cart.Id)
                .ExecuteDeleteAsync(cancellationToken);

            await _context.Set<CartProduct>().AddRangeAsync(cart.CartProducts, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return await GetWithProductsAsync(cart.Id, cancellationToken);
        }

        public async Task<IEnumerable<Cart>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(c => c.CreatedAt >= startDate && c.CreatedAt <= endDate).ToListAsync(cancellationToken);
        }

        public async Task<Cart?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
        }

        public async Task<Cart?> GetWithProductsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(c => c.CartProducts)
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