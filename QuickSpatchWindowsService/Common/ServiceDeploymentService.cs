using System.Configuration;
using System.Data.SqlClient;
using Database.Persistance.Tenants;
using Framework.DomainModel.ValueObject;

namespace QuickSpatchWindowsService.Common
{
    public class ServiceDeploymentService : IDeploymentService
    {
        public Tenant GetCurrentTenant()
        {
            var con = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["AdminDb"].ConnectionString);
            return new Tenant
            {
                Server = con.DataSource,
                Database = con.InitialCatalog,
                Password = con.Password,
                UserName = con.UserID
            };
        }
    }
}
