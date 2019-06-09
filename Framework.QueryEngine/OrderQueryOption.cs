using System.Linq;

namespace Framework.QueryEngine
{
    //public class OrderQueryOption<TEntity, TOrderby> : IQueryOption<TEntity>
    //{
    //    public OrderQueryOption(IQueryOption<TEntity> component, OrderBy<TEntity, TOrderby> order)
    //    {
    //        Component = component;
    //        Order = order;
    //    }

    //    public IQueryOption<TEntity> Component { get; private set; }

    //    public OrderBy<TEntity, TOrderby> Order { get; private set; }

    //    //private IList<OrderBy<TEntity, TEntity>> Orders { get; set; } //todo latter


    //    public IQueryable<TEntity> Apply(IQueryable<TEntity> query)
    //    {
    //        var queryResult = Component.Apply(query);

    //        if (Order != null)
    //        {
    //            queryResult = Order.IsDescendencing ? queryResult.OrderByDescending(Order.KeySelector) : queryResult.OrderBy(Order.KeySelector);
    //        }

    //        return queryResult;
    //    }
    //}
}