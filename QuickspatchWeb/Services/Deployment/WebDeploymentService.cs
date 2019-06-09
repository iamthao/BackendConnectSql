using System.Configuration;
using System.Web;
using System.Web.Mvc;
using Common;
using ConfigValues;
using Database.Persistance.Tenants;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Repositories.Interfaces;

namespace QuickspatchWeb.Services.Deployment
{
    public class WebDeploymentService : IDeploymentService
    {

        public Tenant GetCurrentTenant()
        {
            // Check web config is camino admin or franchisee
            if (ConstantValue.DeploymentMode == DeploymentMode.Camino)
            {
                // Check if controller call grid save, we don't need change connection string
                if (HttpContext.Current.Request.FilePath == "/GridConfig/Save")
                {
                    return new Tenant
                    {
                        Name = ConstantValue.ConnectionStringAdminDb
                    };
                }
                var franchiseeService = DependencyResolver.Current.GetService<IFranchiseeTenantRepository>();
                // Get Franchisee id => if franchiseeId is null, get database caminoAdmin, else get database for Franchisee
                var franchiseeId = 0;
                var franchiseeIdCookie = HttpContext.Current.Request.Cookies["FranchiseeId"];
                if (franchiseeIdCookie != null)
                {
                    int.TryParse(franchiseeIdCookie.Value, out franchiseeId);
                }
                if (franchiseeId == 0)
                {
                    return new Tenant
                    {
                        Name = ConstantValue.ConnectionStringAdminDb
                    };
                }

                var franchisee = franchiseeService.GetById(franchiseeId);
                if (franchisee == null)
                {
                    return new Tenant
                    {
                        Name = ConstantValue.ConnectionStringAdminDb
                    };

                }
                return new Tenant
                {
                    Server = franchisee.Server,
                    Database = franchisee.Database,
                    Password = franchisee.Password,
                    UserName = franchisee.UserName
                };
            }
            return new Tenant
            {
                Name = ConstantValue.ConnectionStringFranchiseeDb
            };

        }
    }
}