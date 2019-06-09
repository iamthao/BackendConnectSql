using Framework.DomainModel;
using QuickspatchWeb.Models;
using Framework.Service.Diagnostics;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Authentication;

namespace QuickspatchWeb.Controllers
{
    public abstract class ApplicationControllerBase : ApplicationControllerGeneric<Entity, MasterfileViewModelBase<Entity>>
    {
        //
        // GET: /ApplicationControllerBase/

        protected ApplicationControllerBase(IAuthenticationService authenticationService, IDiagnosticService diagnosticService)
            : base(authenticationService, diagnosticService, null)
        {
        }

        protected ApplicationControllerBase(IAuthenticationService authenticationService, IDiagnosticService diagnosticService, IMasterFileService<Entity> masterfileService)
            : base(authenticationService, diagnosticService, masterfileService)
        {
        }

    }
}