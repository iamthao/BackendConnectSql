using System;
using System.Linq.Expressions;

namespace Framework.QueryEngine
{
    public class OrderBy<TEntity, TOrderby>
    {
        public bool IsDescendencing { get; set; }

        public Expression<Func<TEntity, TOrderby>> KeySelector { get; set; }
    }
}