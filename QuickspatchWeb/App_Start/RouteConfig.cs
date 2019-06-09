using System.Web.Mvc;
using System.Web.Routing;

namespace QuickspatchWeb.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
           // routes.MapRoute(
           //     "MyRoute",
           //      "{Request}/{action}/{statusId}",
           //     new { controller = "Request", action = "GetCurrentDataRequests", statusId = UrlParameter.Optional }
           //);
        }
    }
}