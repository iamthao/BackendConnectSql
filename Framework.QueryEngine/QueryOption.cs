using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Framework.QueryEngine
{
    public class QueryOption<TEntity> : IQueryOption<TEntity> where TEntity : class
    {
        public QueryOption()
        {
            IncludeExpressions = new List<Expression<Func<TEntity, object>>>();
        }

        public Expression<Func<TEntity, bool>> Filter { get; set; }

        public bool IsNoTracking { get; set; }

        public IList<Expression<Func<TEntity, object>>> IncludeExpressions { get; set; }

        public IQueryable<TEntity> Apply(IQueryable<TEntity> query)
        {
            IQueryable<TEntity> queryResult = null;

            if (IncludeExpressions != null && IncludeExpressions.Count > 0)
            {
                foreach (var includeExpression in IncludeExpressions)
                {
                    queryResult = query.Include(includeExpression);
                }
            }
            else
            {
                queryResult = query;
            }

            if (IsNoTracking)
            {
                queryResult = query.AsNoTracking();
            }

            if (Filter != null)
            {
                queryResult = query.Where(Filter);
            }

            return queryResult;
        }
    }
}