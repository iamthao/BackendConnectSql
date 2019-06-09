using System.Collections.Generic;
using Framework.DomainModel.Common;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;

namespace ServiceLayer.Interfaces
{
    public interface ISystemEventService : IMasterFileService<SystemEvent>
    {
        dynamic GetEventsDashboard();
        void Add(EventMessage eventMessage, Dictionary<EventMessageParam, string> dictionParam);
        dynamic GetNotifyDecline(QueryInfo queryInfo);
    }
}
