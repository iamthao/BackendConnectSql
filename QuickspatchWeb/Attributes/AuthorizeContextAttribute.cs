
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using Framework.DomainModel.Interfaces;
using Framework.Exceptions;
using Framework.Service.Diagnostics;
using ServiceLayer.Authorization;
using ServiceLayer.Common;
using ConfigValues;

namespace QuickspatchWeb.Attributes
{
    public class AuthorizeContextAttribute : AuthorizeAttribute
    {
        private IDiagnosticService _diagnosticService;
        public OperationAction OperationAction { get; set; }
        public DocumentTypeKey DocumentTypeKey { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = httpContext.User as IQuickspatchPrincipal;
            if (user == null)
            {
                return false;
            }
            _diagnosticService = DependencyResolver.Current.GetService<IDiagnosticService>();
            _diagnosticService.Info("Begin AuthorizeCore");
            if (DocumentTypeKey == DocumentTypeKey.None)
            {
                return true;
            }
            //return true;
            var objectAuthorization = DependencyResolver.Current.GetService<IOperationAuthorization>();
            List<UserRoleFunction> userRoleFunctions;
            var exception = new UnAuthorizedAccessException("UnAuthorizedAccessText")
            {
                QuickspatchUserName = user.User != null ? user.User.UserName : null
            };
            //Check this franchisee can use some module from license key
            if (ConstantValue.DeploymentMode == DeploymentMode.Franchisee)
            {
                if (!string.IsNullOrEmpty(user.User.UserRole.AppRoleName) && user.User.UserRole.AppRoleName.Equals("GlobalAdmin", StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }

                if (MenuExtractData.Instance.ModuleForFranchisee != null && MenuExtractData.Instance.ModuleForFranchisee.ListModuleDocumentTypeOperations.Count>0)
                {
                    // Check this document type key have permision in this module for franchisee
                    var hasPermissionForModule =
                        MenuExtractData.Instance.ModuleForFranchisee.ListModuleDocumentTypeOperations.Any(
                            o => o.DocumentTypeId == (int) DocumentTypeKey && o.OperationId == (int) OperationAction);
                    if (!hasPermissionForModule)
                        throw exception;
                }
            }
            var permission = objectAuthorization.VerifyAccess(DocumentTypeKey, OperationAction, out userRoleFunctions);
            httpContext.Items["UserRoleFunctions"] = userRoleFunctions;
            if (!permission)
                throw exception;

            //_diagnosticService.Info("AuthorizeCore Completed.");

            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                // Fire back an unauthorized response
                filterContext.HttpContext.Response.StatusCode = 403;
            }
            else
                base.HandleUnauthorizedRequest(filterContext);
        }
    }
}