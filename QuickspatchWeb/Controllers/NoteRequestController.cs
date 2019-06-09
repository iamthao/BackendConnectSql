using System.Web.Mvc;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using Framework.Service.Diagnostics;
using QuickspatchWeb.Attributes;
using QuickspatchWeb.Models.NoteRequest;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Authentication;
using Framework.Service.Translation;


namespace QuickspatchWeb.Controllers
{
    public class NoteRequestController : ApplicationControllerGeneric<NoteRequest, DashboardNoteRequestDataViewModel>
    {
        private readonly IGridConfigService _gridConfigService;
        private readonly INoteRequestService _noteRequestService;

        public NoteRequestController(IAuthenticationService authenticationService, IDiagnosticService diagnosticService, 
            IGridConfigService gridConfigService, INoteRequestService noteRequestService)
            : base(authenticationService, diagnosticService,noteRequestService)
        {
            _noteRequestService = noteRequestService;
            _gridConfigService = gridConfigService;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Request, OperationAction = OperationAction.View)]
        public JsonResult GetNoteRequest(NoteRequestQueryInfo queryInfo)
        {
            var queryData = _noteRequestService.GetDataForGridMasterfile(queryInfo);
            var clientsJson = Json(queryData, JsonRequestBehavior.AllowGet);
            return clientsJson;
        }
	}
}