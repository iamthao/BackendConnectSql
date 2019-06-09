using System.Web.Mvc;

namespace QuickspatchWeb.Services.Interface
{
    public interface IRenderViewToString
    {
        string RenderRazorViewToString<T>(string viewPath, T model, ViewDataDictionary viewData, ControllerContext controllerContext, TempDataDictionary tempData);
    }
}
