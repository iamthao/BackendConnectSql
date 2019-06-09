using System.Collections.Generic;
using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;
using Framework.DomainModel.ValueObject;

namespace ServiceLayer
{
    public class TemplateService : MasterFileService<Template>,ITemplateService
    {
        private readonly ITemplateRepository  _templateRepository;

        public TemplateService(ITenantPersistenceService tenantPersistenceService, ITemplateRepository templateRepository,
            IBusinessRuleSet<Template> businessRuleSet = null)
            : base(templateRepository, templateRepository, tenantPersistenceService, businessRuleSet)
        {
            _templateRepository = templateRepository;
        }
    }
}
