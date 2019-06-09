using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Framework.Service.StartUp;
using ServiceLayer.Mapping;

namespace QuickspatchApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            // Register for auto mapper
            IStartupTask task = new AutoMapperStartupTask();
            task.Execute();
        }
    }
}
