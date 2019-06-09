using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Framework.Exceptions;
using Framework.Service.Diagnostics;
using QuickspatchWeb.Attributes;
using ServiceLayer.Interfaces.Authentication;

namespace QuickspatchWeb.Controllers
{
    public class ErrorController : ApplicationControllerBase
    {
        private readonly IDiagnosticService _diagnosticService;

        public ErrorController(IAuthenticationService authenticationService,
                               IDiagnosticService diagnosticService
            )
            : base(authenticationService, diagnosticService, null)
        {
            _diagnosticService = diagnosticService;
        }


        public virtual ActionResult Index()
        {
            var handleErrorInfo = ViewData.Model as HandleErrorInfo;

            if (handleErrorInfo != null)
            {
                if (!HttpContext.Request.IsAjaxRequest())
                {
                    return Exception(handleErrorInfo.Exception, handleErrorInfo.ControllerName,
                        handleErrorInfo.ActionName);
                }
                var feedback = HandleAjaxRequestException(handleErrorInfo.Exception);
                return Json(feedback, JsonRequestBehavior.AllowGet);
            }
            return Exception(new Exception("Don't know exception"));
        }


        public virtual ActionResult Exception(Exception exception, string controller = null, string action = null)
        {
            var oxProjectHandleErrorInfo = CreateAdvantageHandleErrorInfo(exception, controller, action);
            try
            {
                ExceptionHandlingResult exceptionHandlingResult;
                HandleException(exception, out exceptionHandlingResult);

                oxProjectHandleErrorInfo.ErrorMessage = exceptionHandlingResult.ErrorMessage;
                oxProjectHandleErrorInfo.StackTrace = exceptionHandlingResult.StackTrace;
                if (!IsProductionMode)
                {
                    oxProjectHandleErrorInfo.AddErrorMessages(exceptionHandlingResult.ModelStateErrors);
                }
            }
            catch (Exception)
            {
                //do nothing, if the error happen system will be in the loop
            }

            return View("~/Views/Error/Exception.cshtml", oxProjectHandleErrorInfo);
        }

        public virtual ActionResult UnAuthorizedAccess(Exception exception)
        {
            _diagnosticService.Error(exception);
            return View("~/Views/Error/UnAuthorizedAccess.cshtml", exception as UnAuthorizedAccessException);
        }


        public virtual ActionResult HTTP500(Exception exception)
        {
            _diagnosticService.Error(exception);
            return Exception(exception);
        }

        public virtual ActionResult HTTP404(Exception exception)
        {

            _diagnosticService.Error(exception);
            return View("~/Views/Error/HTTP404.cshtml", exception as UnAuthorizedAccessException);
        }

        public virtual JsonResult GetJsonError(Exception ex, IEnumerable<string> errors)
        {
            var modelStateErrors = errors as IList<string> ?? errors.ToList();
            var feedbackViewModel = BuildFeedBackViewModel(ex, modelStateErrors);
            return Json(feedbackViewModel, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        private static QuickspatchHandleErrorInfo CreateAdvantageHandleErrorInfo(Exception exception, string controller = null,
            string action = null)
        {
            QuickspatchHandleErrorInfo advantageHandleErrorInfo;
            if (!string.IsNullOrEmpty(action) && !string.IsNullOrEmpty(controller))
            {

                advantageHandleErrorInfo = new QuickspatchHandleErrorInfo(exception, controller, action);
            }
            else
            {
                advantageHandleErrorInfo = new QuickspatchHandleErrorInfo(exception);
            }

            return advantageHandleErrorInfo;
        }
    }
    /// <summary>
    ///     This is data object that is used for Normal request and Ajax request
    /// </summary>
    public class ExceptionHandlingResult
    {
        public ExceptionHandlingResult()
        {
            ModelStateErrors = new List<string>();
        }

        public string ErrorMessage { get; set; }

        public string StackTrace { get; set; }

        /// <summary>
        ///     Contains an array of validation errors on the current form.
        /// </summary>
        public IList<string> ModelStateErrors { get; set; }

        /// <summary>
        ///     Add a list of errors to model state
        /// </summary>
        /// <param name="error"></param>
        public void AddModelStateError(string error)
        {
            ModelStateErrors.Add(error);
        }

        /// <summary>
        ///     Add errors to model state
        /// </summary>
        /// <param name="errors"></param>
        public void AddModelStateErrors(params string[] errors)
        {
            foreach (var error in errors)
            {
                ModelStateErrors.Add(error);
            }
        }
    }
}