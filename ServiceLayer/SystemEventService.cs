using System.Collections.Generic;
using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Common;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;
using Framework.DomainModel.ValueObject;

namespace ServiceLayer
{
    public class SystemEventService : MasterFileService<SystemEvent>,ISystemEventService
    {
        private readonly ISystemEventRepository _systemEventRepository;

        public SystemEventService(ITenantPersistenceService tenantPersistenceService, ISystemEventRepository systemEventRepository,
            IBusinessRuleSet<SystemEvent> businessRuleSet = null)
            : base(systemEventRepository, systemEventRepository, tenantPersistenceService, businessRuleSet)
        {
            _systemEventRepository = systemEventRepository;
        }

        public dynamic GetEventsDashboard()
        {
            var data = _systemEventRepository.GetEventsDashboard();
            return new {Data = data, TotalRowCount = data.Count};
        }


        public void Add(EventMessage eventMessage, Dictionary<EventMessageParam, string> dictionParam)
        {
            _systemEventRepository.Add(eventMessage, dictionParam);
        }

        public dynamic GetNotifyDecline(QueryInfo queryInfo)
        {
            return _systemEventRepository.GetNotifyDecline(queryInfo);
        }
    }
}
