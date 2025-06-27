using System.Linq.Expressions;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.Domain.Common
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WhereDynamic<T>(this IQueryable<T> source, string propertyName, string value, string op)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var prop = Expression.PropertyOrField(param, propertyName);
            ConstantExpression constant;
            if (prop.Type == typeof(Guid))
            {
                constant = Expression.Constant(Guid.Parse(value));
            }
            else
                constant = Expression.Constant(Convert.ChangeType(value, prop.Type));

            Expression? comparison = op switch
            {
                "eq" => Expression.Equal(prop, constant),
                "gte" => Expression.GreaterThanOrEqual(prop, constant),
                "lte" => Expression.LessThanOrEqual(prop, constant),
                _ => null
            };

            var lambda = Expression.Lambda<Func<T, bool>>(comparison!, param);
            return source.Where(lambda);
        }

        public static IQueryable<T> WhereDynamicLike<T>(this IQueryable<T> source, string propertyName, string pattern, bool startsWith, bool endsWith)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var prop = Expression.PropertyOrField(param, propertyName);

            if (prop.Type != typeof(string)) return source;

            MethodInfo? method = startsWith && endsWith
                ? typeof(string).GetMethod("Contains", new[] { typeof(string) })
                : startsWith
                    ? typeof(string).GetMethod("EndsWith", new[] { typeof(string) })
                    : typeof(string).GetMethod("StartsWith", new[] { typeof(string) });

            var constant = Expression.Constant(pattern, typeof(string));
            var call = Expression.Call(prop, method!, constant);
            var lambda = Expression.Lambda<Func<T, bool>>(call, param);

            return source.Where(lambda);
        }

        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string propertyName, bool descending, bool thenBy = false)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var prop = Expression.PropertyOrField(param, propertyName);
            var lambda = Expression.Lambda(prop, param);

            string method = thenBy
                ? (descending ? "ThenByDescending" : "ThenBy")
                : (descending ? "OrderByDescending" : "OrderBy");

            var types = new Type[] { source.ElementType, lambda.Body.Type };
            var mce = Expression.Call(typeof(Queryable), method, types, source.Expression, Expression.Quote(lambda));

            return source.Provider.CreateQuery<T>(mce);
        }
    }
}
