using System.Collections.Generic;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;
using Framework.Repositories;

namespace Repositories.Interfaces
{
    public interface IHoldingRequestRepository : IRepository<HoldingRequest>, IQueryableRepository<HoldingRequest>
    {
        dynamic GetListHoldingRequest(IQueryInfo queryInfo);
    }
}