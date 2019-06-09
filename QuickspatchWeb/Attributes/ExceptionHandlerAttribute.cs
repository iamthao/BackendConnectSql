using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using Framework.DomainModel.Interfaces;
using QuickspatchWeb.Controllers;
using Framework.Exceptions;
using Framework.Service.Diagnostics;
using Framework.Web;
using QuickspatchWeb.Models;

namespace QuickspatchWeb.Attributes
{
    /// <summary>
    ///     This custom handler to by pass the default exception controller .
    /// </summary>
    public class ExceptionHandlerAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext exceptionContext)
        {
            //handle exception here
            var controller = exceptionContext.Controller as Controller;
            if (controller == null || exceptionContext.ExceptionHandled) return;

            var exception = exceptionContext.Exception;
            if (exception == null) return;

            // Action method exceptions will be wrapped in a
            // TargetInvocationException since they're invoked using 
            // reflection, so we have to unwrap it.
            if (exception is TargetInvocationException)
            {
                exception = exception.InnerException;
            }
            

            var contains = false;
            const string json = "application/json";
            if (exceptionContext.HttpContext.Request.AcceptTypes != null)
                if (exceptionContext.HttpContext.Request.AcceptTypes.Contains(json))
                {
                    contains = true;
                }
            if (exceptionContext.HttpContext.Request.IsAjaxRequest() && contains)
            {
                var controllerContext = exceptionContext as ControllerContext;

                var errors = controllerContext.Controller
                                                              .ViewData
                                                              .ModelState
                                                              .Values
                                                              .SelectMany(v => v.Errors)
                                                              .Select(v => v.ErrorMessage)
                                                              .Distinct();


                // if request was an Ajax request, respond with json with Error field
                var errorController = DependencyResolver.Current.GetService<ErrorController>();
                errorController.ControllerContext = exceptionContext;

                //var errorController = new ErrorController {ControllerContext = exceptionContext};
                var jsonResult = errorController.GetJsonError(exceptionContext.Exception, errors);
                if (exception is InvalidClaimsException && jsonResult.Data is FeedbackViewModel)
                {
                    var invalidClaimsException = (InvalidClaimsException) exception;
                    var modelError = (FeedbackViewModel) jsonResult.Data;
                    modelError.Data =   new {Type = "InvalidLicenseKey", KeyCode = invalidClaimsException.KeyAuthentication};
                    jsonResult.Data = modelError;
                }
                exceptionContext.Result = jsonResult;
            }
            else
            {
                var errorController = DependencyResolver.Current.GetService<ErrorController>();
                errorController.ControllerContext = exceptionContext;
                ActionResult actionResult = null;

                if (exceptionContext.Exception is UnAuthorizedAccessException)
                {
                    actionResult = errorController.UnAuthorizedAccess(exceptionContext.Exception);
                }
                else if (exceptionContext.HttpContext.Response.StatusCode == 200)
                {
                    actionResult = errorController.HTTP404(exceptionContext.Exception);
                }
                else
                {
                    actionResult = errorController.Exception(exceptionContext.Exception);
                }

                exceptionContext.Result = actionResult;

                exceptionContext.HttpContext.Response.Clear();
                // Internal error server
                //exceptionContext.HttpContext.Response.StatusCode = 500;

                // Certain versions of IIS will sometimes use their own error page when
                // they detect a server error. Setting this property indicates that we
                // want it to try to render ASP.NET MVC's error page instead.
                exceptionContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            }

            exceptionContext.ExceptionHandled = true;
        }
    }

    public class QuickspatchHandleErrorInfo : HandleErrorInfo
    {
        private string _errorMessage = string.Empty;
        private string _stackTrace = string.Empty;

        public QuickspatchHandleErrorInfo(Exception exception, string controller, string action)
            : base(exception, controller, action)
        {
            ErrorMessage = string.Empty;
            StackTrace = string.Empty;
            GetUserInformation(exception);
        }

        public QuickspatchHandleErrorInfo(Exception exception)
            : base(exception, "ignore", "ignore")
        {
            ErrorMessage = string.Empty;
            StackTrace = string.Empty;
            GetUserInformation(exception);

        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

        public string StackTrace
        {
            get { return _stackTrace; }
            set { _stackTrace = value; }
        }

        public string ClaimUserName { get; set; }
        public string AdvantageVersion
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
        }

        public void AddErrorMessages(IList<string> modelStateErrors)
        {
            var errors = new StringBuilder();
            errors.Append(ErrorMessage + Environment.NewLine);

            foreach (var error in modelStateErrors)
            {
                errors.Append(error + Environment.NewLine);
            }

            ErrorMessage = errors.ToString();
        }

        private void GetUserInformation(Exception exception)
        {
            var advantageContext = DependencyResolver.Current.GetService<IQuickspatchHttpContext>();
            var advantageUser = advantageContext.User as IQuickspatchPrincipal;
            var advantageEx = exception as QuickspatchException;
            ClaimUserName = (advantageUser == null) ? string.Empty : advantageUser.User.UserName;

            var log = DependencyResolver.Current.GetService<IDiagnosticService>();

            if (advantageEx != null && string.IsNullOrEmpty(ClaimUserName))
            {
                log.Info(string.Format("advantageEx {0}", advantageEx.QuickspatchUserName));
                ClaimUserName = advantageEx.QuickspatchUserName;
            }
        }
    }
}