using System.Collections.Generic;
using Framework.DomainModel.Common;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Framework.Repositories;

namespace Repositories.Interfaces
{
    public interface ISystemEventRepository : IRepository<SystemEvent>, IQueryableRepository<SystemEvent>
    {
        List<SystemEventGridVo> GetEventsDashboard();
        void Add(EventMessage eventMessage, Dictionary<EventMessageParam, string> dictionParam);
        dynamic GetNotifyDecline(QueryInfo queryInfo);
    }
}
