using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using Framework.DomainModel.Interfaces;
using Framework.Service.Diagnostics;
using ServiceLayer.Authorization;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web.Http.Controllers;
using AllowAnonymousAttribute = System.Web.Mvc.AllowAnonymousAttribute;
using AuthorizeAttribute = System.Web.Http.AuthorizeAttribute;
namespace QuickspatchApi.Attributes
{
    public class AuthorizeContextAttribute : AuthorizeAttribute
    {
        private IDiagnosticService _diagnosticService;
        public OperationAction OperationAction { get; set; }
        public DocumentTypeKey DocumentTypeKey { get; set; }
        public QuickspatchClientType QuickspatchClientType { get; set; }
        private const string BasicAuthResponseHeader = "WWW-Authenticate";
        private const string BasicAuthResponseHeaderValue = "Basic";

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (AuthorizationDisabled(actionContext) || AuthorizeRequest(actionContext.ControllerContext.Request))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return;
            }
            var claimsPrincipal = actionContext.Request.GetRequestContext().Principal as ClaimsPrincipal;
            if (claimsPrincipal != null)
            {
                var roleClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimsDeclaration.QuickspatchClientType);

                if (roleClaim != null)
                {
                    var clientType = Convert.ToInt32(roleClaim.Value);
                    if (QuickspatchClientType != (QuickspatchClientType) clientType)
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
                    }
                }
                else
                {
                    var diagnosticService = new DiagnosticService();

                    diagnosticService.Debug(string.Format("Validated ticket failed"));

                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
                }
            }
            else
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.ExpectationFailed);
            }
        }

        private static bool AuthorizationDisabled(HttpActionContext actionContext)
        {
            if (!actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
            {
                return actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
            }
            return true;
        }

        private bool AuthorizeRequest(HttpRequestMessage request)
        {
            var authValue = request.Headers.Authorization;
            if (authValue == null || String.IsNullOrWhiteSpace(authValue.Parameter)
                || String.IsNullOrWhiteSpace(authValue.Scheme)
                || authValue.Scheme != BasicAuthResponseHeaderValue)
            {
                return false;
            }

            var parsedHeader = ParseAuthorizationHeader(authValue.Parameter);
            if (parsedHeader == null)
            {
                return false;
            }
            return true;
        }

        private string[] ParseAuthorizationHeader(string authHeader)
        {
            var credentials = Encoding.ASCII.GetString(Convert.FromBase64String(authHeader)).Split(new[] { ':' });
            if (credentials.Length != 2 || string.IsNullOrEmpty(credentials[0]) || string.IsNullOrEmpty(credentials[1]))
            {
                return null;
            }
            return credentials;
        }
    }
}