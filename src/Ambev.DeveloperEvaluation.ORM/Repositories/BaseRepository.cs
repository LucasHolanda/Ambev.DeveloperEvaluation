using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        private static IQueryable<T> ApplyFilters(IQueryable<T> query, Dictionary<string, string> filters)
        {
            foreach (var filter in filters)
            {
                var key = filter.Key;
                var value = filter.Value;

                // Range filters: _minPrice, _maxPrice, _minDate, etc
                if (key.StartsWith("_min", StringComparison.OrdinalIgnoreCase))
                {
                    var prop = key.Substring(4);
                    query = query.WhereDynamic(prop, value, "gte");
                }
                else if (key.StartsWith("_max", StringComparison.OrdinalIgnoreCase))
                {
                    var prop = key.Substring(4);
                    query = query.WhereDynamic(prop, value, "lte");
                }
                // String contains (wildcards)
                else if (value.Contains("*"))
                {
                    var pattern = value.Replace("*", "");
                    var isStart = value.StartsWith("*");
                    var isEnd = value.EndsWith("*");

                    query = query.WhereDynamicLike(key, pattern, isStart, isEnd);
                }
                else
                {
                    // Check if key exists in the entity
                    if (!typeof(T).GetProperty(key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)?.CanRead ?? true)
                        throw new ArgumentException($"Property '{key}' does not exist on type '{typeof(T).Name}'");

                    query = query.WhereDynamic(key, value, "eq");
                }
            }

            return query;
        }

        private static IQueryable<T> ApplyOrdering(IQueryable<T> query, string? orderParam)
        {
            if (string.IsNullOrWhiteSpace(orderParam))
                return query;

            var orders = orderParam.Split(',', StringSplitOptions.RemoveEmptyEntries);
            bool first = true;

            foreach (var ord in orders)
            {
                var trimmed = ord.Trim();
                var parts = trimmed.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var field = parts[0];
                var desc = parts.Length > 1 && parts[1].Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = query.OrderByDynamic(field, desc, thenBy: !first);
                first = false;
            }

            return query;
        }

        public async Task<List<T>> GetByQueryParameters(
            QueryParameters parameters,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _dbSet.AsQueryable();

            if (include != null)
                query = include(query);

            query = ApplyFilters(query, parameters.Filters);
            query = ApplyOrdering(query, parameters._order);

            query = query
                .Skip((parameters._page - 1) * parameters._size)
                .Take(parameters._size);

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<int> CountByQueryParametersAsync(QueryParameters parameters, CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _dbSet;
            query = BaseRepository<T>.ApplyFilters(query, parameters.Filters);
            return await query.CountAsync(cancellationToken);
        }

        public async Task<int> CountByIdAsync(Guid Id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.CountAsync(c => c.Id == Id, cancellationToken);
        }

        public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync([id], cancellationToken);
        }

        public virtual async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }

        public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _dbSet.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _dbSet.Where(e => e.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return true;
        }
    }
}