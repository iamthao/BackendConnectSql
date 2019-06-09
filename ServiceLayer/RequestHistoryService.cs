using System.Collections.Generic;
using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Framework.Repositories;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;

namespace ServiceLayer
{
    public class RequestHistoryService : MasterFileService<RequestHistory>, IRequestHistoryService
    {
        private readonly IRequestHistoryRepository _requestHistoryRepository;
        public RequestHistoryService(IRequestHistoryRepository requestHistoryRepository,
            ITenantPersistenceService tenantPersistentService, 
            IBusinessRuleSet<RequestHistory> businessRuleSet = null)
            : base(requestHistoryRepository, requestHistoryRepository, tenantPersistentService, businessRuleSet)
        {
            _requestHistoryRepository = requestHistoryRepository;
        }


        public void AddListRequestHistoryForWindowsService(List<RequestHistory> listRequestHistories)
        {
            foreach (var listRequestHistory in listRequestHistories)
            {
                _requestHistoryRepository.Add(listRequestHistory);
            }
            _requestHistoryRepository.Commit();
        }
    }
}
