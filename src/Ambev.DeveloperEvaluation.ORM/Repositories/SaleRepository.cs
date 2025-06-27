using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class SaleRepository : BaseRepository<Sale>, ISaleRepository
    {
        public SaleRepository(PostgresContext context) : base(context) { }

        public async Task<Sale?> AddSaleWithItemsAsync(Sale Sale, CancellationToken cancellationToken = default)
        {
            var SaleAdd = await _dbSet.AddAsync(Sale, cancellationToken);
            foreach (var SaleProduct in Sale.SaleItems)
            {
                SaleProduct.SaleId = SaleAdd.Entity.Id;
            }

            await _context.Set<SaleItem>().AddRangeAsync(Sale.SaleItems, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return await GetByIdAsync(SaleAdd.Entity.Id, cancellationToken);
        }

        public Task<IEnumerable<Sale>> GetActiveSalesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException("GetActiveSalesAsync is not implemented.");
        }

        public async Task<IEnumerable<Sale?>> GetAllByQuery(QueryParameters queryParameters, CancellationToken cancellationToken = default)
        {
            return await GetByQueryParameters(queryParameters, src => src.Include(s => s.SaleItems).ThenInclude(si => si.Product), cancellationToken);
        }

        public Task<IEnumerable<Sale>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Sale?> GetWithItemsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(s => s.SaleItems)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<Sale?> GetWithItemsAndProductsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(s => s.SaleItems)
                .ThenInclude(si => si.Product)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<bool> UpdateSaleAndItemsAsync(Sale Sale, CancellationToken cancellationToken = default)
        {
            Sale.UpdatedAt = DateTime.UtcNow;
            _context.Set<Sale>().Update(Sale);
            _context.Set<SaleItem>().UpdateRange(Sale.SaleItems);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}