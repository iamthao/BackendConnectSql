using System.Collections.Generic;
using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Framework.Repositories;

namespace Repositories.Interfaces
{
    public interface IStateRepository : IRepository<State>, IQueryableRepository<State>
    {
        List<LookupItemVo> GetAllStateForLookUp();
    }
}
