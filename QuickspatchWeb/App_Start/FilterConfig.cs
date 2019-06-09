using System.Web.Mvc;
using QuickspatchWeb.Attributes;

namespace QuickspatchWeb.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ExceptionHandlerAttribute());
            filters.Add(new QuickspatchActionFilterAttribute());
        }
    }
}