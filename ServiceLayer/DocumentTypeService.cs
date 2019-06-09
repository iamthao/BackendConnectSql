using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;

namespace ServiceLayer
{
    public class DocumentTypeService : MasterFileService<DocumentType>, IDocumentTypeService
    {
        private readonly IDocumentTypeRepository _documentTypeRepository;
        public DocumentTypeService(ITenantPersistenceService tenantPersistenceService, IDocumentTypeRepository documentTypeRepository,
            IBusinessRuleSet<DocumentType> businessRuleSet = null)
            : base(documentTypeRepository, documentTypeRepository, tenantPersistenceService, businessRuleSet)
        {
            _documentTypeRepository = documentTypeRepository;
        }
    }
}