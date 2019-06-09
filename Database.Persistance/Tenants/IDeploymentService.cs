using Framework.DomainModel.ValueObject;

namespace Database.Persistance.Tenants
{
    public interface IDeploymentService
    {
        Tenant GetCurrentTenant();
    }
}