using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace PlanIt.Repositories.Extentions
{
    public static class QueriableExtentions
    {
        private static readonly Type ENUMERABLE_TYPE = typeof(Enumerable);
        private const string SINGLE_METHOD_NAME = "Single";

        public static IQueryable<TEntity> Include<TEntity>(this IQueryable<TEntity> sequence, string path) where TEntity : class
        {
            DbQuery<TEntity> objectQuery = sequence as DbQuery<TEntity>;
            if (objectQuery != null)
            {
                return objectQuery.Include(path);
            }
            return sequence;
        }

        public static DbQuery<TSource> Include<TSource, TResult>(this IQueryable<TSource> sequence, Expression<Func<TSource, TResult>> path) where TSource : class
        {
            DbQuery<TSource> query = sequence as DbQuery<TSource>;
            if (query == null)
            {
                throw new ArgumentNullException("Query can not be null");
            }

            var expression = path.Body;

            var propertyPathVisitor = new PropertyPathVisitor();

            var propertyPath = propertyPathVisitor.GetPropertyPath(expression);

            return query.Include(propertyPath);
        }
    }
}
