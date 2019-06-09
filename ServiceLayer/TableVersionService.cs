using System.Collections.Generic;
using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;
using Framework.DomainModel.ValueObject;

namespace ServiceLayer
{
    public class TableVersionService : MasterFileService<TableVersion>,ITableVersionService
    {
        private readonly ITableVersionRepository  _tableVersionRepository;

        public TableVersionService(ITenantPersistenceService tenantPersistenceService, ITableVersionRepository tableVersionRepository,
            IBusinessRuleSet<TableVersion> businessRuleSet = null)
            : base(tableVersionRepository, tableVersionRepository, tenantPersistenceService, businessRuleSet)
        {
            _tableVersionRepository = tableVersionRepository;
        }

        
    }
}
