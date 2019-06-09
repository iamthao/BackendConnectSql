using System;
using Framework.DomainModel.Entities;
using Framework.Repositories;

namespace Repositories.Interfaces
{
    public interface IGridConfigRepository : IRepository<GridConfig>, IQueryableRepository<GridConfig>
    {
        TResult GetGridConfig<TResult>(Func<GridConfig, TResult> selector, int userId, int documentTypeId, string gridInternalName = "");
    }
}