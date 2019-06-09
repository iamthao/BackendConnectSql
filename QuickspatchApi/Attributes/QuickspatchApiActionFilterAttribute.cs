using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;
using Database.Persistance.Tenants;
using Framework.Exceptions;
using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;

namespace QuickspatchApi.Attributes
{
    public class QuickspatchApiActionFilterAttribute : ActionFilterAttribute
    {
        private ITenantWorkspace _tenantWorkspace;
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var tenantPersistenceService = actionContext.Request.GetDependencyScope().GetService(typeof(ITenantPersistenceService)) as ITenantPersistenceService;
            if (tenantPersistenceService!=null)
                _tenantWorkspace = tenantPersistenceService.CreateWorkspace();
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var modelState = actionExecutedContext.ActionContext.ModelState;
            if (actionExecutedContext.Exception != null)
            {
                if (actionExecutedContext.Exception is BusinessRuleException)
                {
                    var ex = actionExecutedContext.Exception as BusinessRuleException;
                    foreach (var error in ex.FailedRules)
                    {
                        //TO work with multiple field names for one erorr
                        if (error.ValidationResults == null || error.ValidationResults.Count == 0)
                        {
                            modelState.AddModelError((error.PropertyNames != null && error.PropertyNames.Length > 0) ? error.PropertyNames[0] : string.Empty, error.Message);
                        }
                        else
                        {
                            foreach (var dataError in error.ValidationResults)
                            {
                                modelState.AddModelError(dataError.MemberNames.ToString(), dataError.ErrorMessage);
                            }
                        }
                    }
                }
            }
            try
            {
                //var tenantPersistenceService = actionExecutedContext.Request.GetDependencyScope().GetService(typeof(ITenantPersistenceService)) as ITenantPersistenceService;
                //DeploymentService.GetCurrentTenant()
                //if (tenantPersistenceService != null)
                    //tenantPersistenceService.CurrentWorkspace.Dispose();
                _tenantWorkspace.Dispose();
            }
            catch (Exception)
            {
                //do nothing
            }
        }
    }
}