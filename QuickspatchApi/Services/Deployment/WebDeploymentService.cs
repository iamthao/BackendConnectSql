using ConfigValues;
using Database.Persistance.Tenants;
using Framework.DomainModel.ValueObject;

namespace QuickspatchApi.Services.Deployment
{
    public class WebDeploymentService : IDeploymentService
    {
        public Tenant GetCurrentTenant()
        {
            return new Tenant
            {
                Name = ConstantValue.ConnectionStringAdminDb
            };

        }
    }
}