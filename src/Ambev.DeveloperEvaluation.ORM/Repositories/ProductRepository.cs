using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(DbContext context) : base(context) { }  

        public async Task<IEnumerable<Product>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(p => p.Category == category)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> SearchByTitleAsync(string title, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(p => p.Title.Contains(title))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetByTitleAndCategoryAsync(string title, string category, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(p => p.Title.Contains(title) && p.Category == category)
                .ToListAsync(cancellationToken);
        }

        public async Task<Product?> GetByIdWithRatingAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }
    }
}
