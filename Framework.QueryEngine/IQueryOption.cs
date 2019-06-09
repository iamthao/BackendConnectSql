using System.Linq;

namespace Framework.QueryEngine
{
    public interface IQueryOption<TEntity>
    {
        IQueryable<TEntity> Apply(IQueryable<TEntity> query);
    }
}