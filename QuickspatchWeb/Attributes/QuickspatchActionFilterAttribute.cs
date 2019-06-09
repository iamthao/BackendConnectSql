using System;
using System.Web.Http;
using System.Web.Mvc;
using Database.Persistance.Tenants;
using Framework.Exceptions;

namespace QuickspatchWeb.Attributes
{
    public class QuickspatchActionFilterAttribute : ActionFilterAttribute
    {
        private readonly ITenantPersistenceService _tenantPersistenceService;
        private ITenantWorkspace _tenantWorkspace;
        public QuickspatchActionFilterAttribute()
        {
            var dependencyResolver = GlobalConfiguration.Configuration.DependencyResolver;
            _tenantPersistenceService =
               DependencyResolver.Current.GetService<ITenantPersistenceService>(); 
            
        }
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var tenantPersistenceService =
              DependencyResolver.Current.GetService<ITenantPersistenceService>();
            _tenantWorkspace = tenantPersistenceService.CreateWorkspace();
        }

        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            var modelState = ((Controller)actionExecutedContext.Controller).ModelState;
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
                if (_tenantWorkspace != null)
                    _tenantWorkspace.Dispose();
            }
            catch (Exception)
            {
                //do nothing
            }
        }
    }
}