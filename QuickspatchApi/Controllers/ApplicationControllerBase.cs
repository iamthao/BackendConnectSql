using Framework.DomainModel;
using Framework.DomainModel.DataTransferObject;
using Framework.Service.Diagnostics;
using ServiceLayer.Interfaces;

namespace QuickspatchApi.Controllers
{
    public abstract class ApplicationControllerBase : ApplicationControllerGeneric<Entity, DtoBase>
    {
        //
        // GET: /ApplicationControllerBase/

        protected ApplicationControllerBase(IDiagnosticService diagnosticService)
            : base(diagnosticService, null)
        {
        }

        protected ApplicationControllerBase(IDiagnosticService diagnosticService, IMasterFileService<Entity> masterfileService)
            : base(diagnosticService, masterfileService)
        {
        }

    }
}