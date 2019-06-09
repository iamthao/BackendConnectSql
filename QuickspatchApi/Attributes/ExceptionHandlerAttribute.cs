using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http.Filters;
using QuickspatchApi.Controllers;

namespace QuickspatchApi.Attributes
{
    public class ExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext exceptionContext)
        {
            var exception = exceptionContext.Exception;
            if (exception == null) return;

            // Action method exceptions will be wrapped in a
            // TargetInvocationException since they're invoked using 
            // reflection, so we have to unwrap it.
            if (exception is TargetInvocationException)
            {
                exception = exception.InnerException;
            }
            var errors = exceptionContext.ActionContext.ModelState
                .Values
                .SelectMany(v => v.Errors)
                .Select(v => v.ErrorMessage)
                .Distinct();
            var errorController = exceptionContext.Request.GetDependencyScope().GetService(typeof(ErrorController)) as ErrorController;
            //var errorController = new ErrorController {ControllerContext = exceptionContext};
            var jsonResult = errorController.GetJsonError(exceptionContext.Exception, errors);
            exceptionContext.Response =
                exceptionContext.Request.CreateResponse(HttpStatusCode.InternalServerError, jsonResult);
            //base.OnException(exceptionContext);
        }

    }
}