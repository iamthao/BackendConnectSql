using System;
using System.IO;
using System.Web.Mvc;
using QuickspatchWeb.Services.Interface;

namespace QuickspatchWeb.Services.Implement
{
    public class RazorRenderViewToStringService : IRenderViewToString
    {
        public string RenderRazorViewToString<T>(string viewPath, T model, ViewDataDictionary viewData, ControllerContext controllerContext, TempDataDictionary tempData)
        {
            viewData.Model = model;
            using (var writer = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewPath);
                var viewContext = new ViewContext(controllerContext, viewResult.View, viewData, tempData, writer);
                viewResult.View.Render(viewContext, writer);
                return writer.GetStringBuilder().ToString();
            }
        }
    }
}