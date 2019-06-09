using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using QuickspatchApi.Attributes;

namespace QuickspatchApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute("http://localhost:55125", "*", "*");
            config.EnableCors(cors);
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            
            ContainerConfig.RegisterDependencies(config);

            config.Filters.Add(new ExceptionHandlerAttribute());
            config.Filters.Add(new QuickspatchApiActionFilterAttribute());

            //Only JSON OUPUT
            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
        }
    }
}
