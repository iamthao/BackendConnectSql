using Database.Persistance.Tenants;
using Framework.DomainModel.Entities;
using Repositories.Interfaces;

namespace Repositories
{
    public class EntityFrameworkDocumentTypeRepository : EntityFrameworkTenantRepositoryBase<DocumentType>, IDocumentTypeRepository
    {
        public EntityFrameworkDocumentTypeRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {
            SearchColumns.Add("Name");
            DisplayColumnForCombobox = "Name";
        }
    }
}