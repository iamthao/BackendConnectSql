using Framework.DomainModel.ValueObject;

namespace Database.Persistance.Tenants
{
    public interface ITenantPersistenceService : IPersistenceService<ITenantWorkspace>
    {
        ITenantWorkspace CreateWorkspace(Tenant tenant);
    }
}