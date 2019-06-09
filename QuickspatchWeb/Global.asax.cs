using System;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Framework.DomainModel.Interfaces;
using QuickspatchWeb.App_Start;
using QuickspatchWeb.Models.Mapping;
using Framework.Service.StartUp;

namespace QuickspatchWeb
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            ContainerConfig.RegisterDependencies();
            // Register for auto mapper
            IStartupTask task = new AutoMapperStartupTask();
            task.Execute();
        }

        private void Application_OnPostAuthenticateRequest(object sender, EventArgs e)
        {
            // Get a reference to the current User
            var principal = User as IQuickspatchPrincipal;
            var formsAuthCookie = HttpContext.Current.Request.Cookies[ClaimsDeclaration.AuthenticationCookie];

            if (formsAuthCookie != null)
            {
                principal = HttpContext.Current.Cache[formsAuthCookie.Value] as IQuickspatchPrincipal;
                if (principal != null)
                {
                    HttpContext.Current.User = principal;
                    Thread.CurrentPrincipal = principal;
                }
            }

            // If we are dealing with an authenticated forms authentication request
            if (principal == null)
            {
                if (HttpContext.Current.Request.Url.AbsolutePath.ToLower().Contains("authentication"))
                {
                    return;
                }
                var contextWrapper = new HttpContextWrapper(HttpContext.Current);
                if (contextWrapper.Request.IsAjaxRequest())
                {
                    contextWrapper.Response.StatusCode = 403; // Authentication error.                 
                }
            }
        }
    }
}