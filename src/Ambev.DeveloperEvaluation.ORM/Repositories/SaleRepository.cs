using Ambev.DeveloperEvaluation.Domain.Aggregates;
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

        public Task<IEnumerable<Sale>> GetByBranchIdAsync(int branchId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
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

        public Task<Sale?> GetWithItemsAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}