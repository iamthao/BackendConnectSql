using System.Collections.Generic;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Framework.Repositories;

namespace Repositories.Interfaces
{
    public interface IStaticValueRepository : IRepository<StaticValue>, IQueryableRepository<StaticValue>
    {
        int GetNewRequestNumber();
        CheckSumChange CheckChangeTable();
    }
}