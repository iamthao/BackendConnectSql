using System;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Framework.Web.Serializer
{
    /// <summary>
    ///     Replacing MVC JavascriptSerializer with JSON.NET JsonSerializer
    ///     see http://wingkaiwan.com/2012/12/28/replacing-mvc-javascriptserializer-with-json-net-jsonserializer/
    /// </summary>
    public class JsonNetResult : JsonResult
    {
        public JsonNetResult()
        {
            Settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Error
            };
        }

        public JsonSerializerSettings Settings { get; private set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
                String.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("JSON GET is not allowed");
            }

            var response = context.HttpContext.Response;
            response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            if (Data != null)
            {
                var scriptSerializer = JsonSerializer.Create(Settings);

                using (var sw = new StringWriter())
                {
                    scriptSerializer.Serialize(sw, Data);
                    response.Write(sw.ToString());
                }
            }
        }
    }
}