using System.Collections.Generic;
using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;
using Framework.DomainModel.ValueObject;

namespace ServiceLayer
{
    public class StateService : MasterFileService<State>,IStateService
    {
        private readonly IStateRepository  _stateRepository;

        public StateService(ITenantPersistenceService tenantPersistenceService, IStateRepository stateRepository,
            IBusinessRuleSet<State> businessRuleSet = null)
            : base(stateRepository, stateRepository, tenantPersistenceService, businessRuleSet)
        {
            _stateRepository = stateRepository;
        }

        public List<LookupItemVo> GetAllStateForLookUp()
        {
            var state = _stateRepository.GetAllStateForLookUp();
            return state;
        }
    }
}
