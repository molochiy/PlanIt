﻿using System;
using System.Linq;
using System.Linq.Expressions;
using PlanIt.Repositories.Extentions;

namespace PlanIt.Repositories.Abstract
{
    public abstract class BaseRepository
    {
        #region Private Fields

        private readonly IDataContextFactory _dataContextFactory;

        #endregion Private Fields

        #region Protected Properties

        protected IDataContextFactory DataContextFactory
        {
            get { return _dataContextFactory; }
        }

        #endregion Protected Properties

        #region Constructors

        public BaseRepository(IDataContextFactory dataContextFactory)
        {
            this._dataContextFactory = dataContextFactory;
        }

        #endregion Constructors

        #region Helpers

        protected Expression<Func<TEntity, bool>> GetSelectByKeyCriteria<TKey, TEntity>(string keyPropertyName, TKey key) where TEntity : class
        {
            var entity = Expression.Parameter(typeof(TEntity), "r");

            var keyProperty = Expression.Property(entity, keyPropertyName);

            var keyValue = Expression.Constant(key);

            var equal = Expression.Equal(keyProperty, keyValue);

            var lambda = Expression.Lambda<Func<TEntity, bool>>(equal, entity);

            return lambda;
        }

        protected IQueryable<TEntity> IncludePaths<TEntity>(Expression<Func<TEntity, object>>[] paths, IQueryable<TEntity> query) where TEntity : class
        {
            if (paths != null)
            {
                foreach (var path in paths)
                {
                    query = query.Include(path);
                }
            }
            return query;
        }

        #endregion Helpers

    }
}
