using Database.Persistance.Tenants;
using Framework.DomainModel.Entities;
using Repositories.Interfaces;

namespace Repositories
{
    public class EntityFrameworkModuleDocumentTypeOperationRepository : EntityFrameworkTenantRepositoryBase<ModuleDocumentTypeOperation>, IModuleDocumentTypeOperationRepository
    {
        public EntityFrameworkModuleDocumentTypeOperationRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {
        }
    }
}