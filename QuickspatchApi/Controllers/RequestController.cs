using System.Web.Http;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using Framework.Service.Diagnostics;
using QuickspatchApi.Attributes;
using ServiceLayer.Interfaces;

namespace QuickspatchApi.Controllers
{
    [RoutePrefix("api/Request")]
    public class RequestController : ApplicationControllerBase
    {
        private readonly IDiagnosticService _diagnosticService;
        private readonly IRequestService _requestService;
        private readonly IFranchiseeTenantService _franchiseeTenantService;

        public RequestController(IDiagnosticService diagnosticService, IFranchiseeTenantService franchiseeTenantService, IRequestService requestService)
            : base(diagnosticService, null)
        {
            _diagnosticService = diagnosticService;
            _franchiseeTenantService = franchiseeTenantService;
            _requestService = requestService;
        }

        [HttpPost]
        [Route("GetRequest")]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View, QuickspatchClientType = QuickspatchClientType.MobileClient)]
        public IHttpActionResult GetRequest(UtcDto dtoBase)
        {
            return Ok(_requestService.GetRequestForToday(dtoBase.Id,dtoBase.UtcTimeZone));
        }

        [HttpPost]
        [Route("UpdateRequestStatus")]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View, QuickspatchClientType = QuickspatchClientType.MobileClient)]
        public IHttpActionResult UpdateRequestStatus(UpdateRequestDto requestDto)
        {
            _requestService.UpdateRequestStatus(requestDto);
            return Ok();
        }


        [HttpPost]
        [Route("AddNoteRequest")]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View, QuickspatchClientType = QuickspatchClientType.MobileClient)]
        public IHttpActionResult AddNoteRequest(UpdateSingleNoteRequestDto requestDto)
        {
            _requestService.AddNoteRequest(requestDto);
            return Ok();
        }


        [HttpPost]
        [Route("UpdateListRequestStatus")]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View, QuickspatchClientType = QuickspatchClientType.MobileClient)]
        public IHttpActionResult UpdateListRequestStatus(RequestStatusDto requestStatusDto)
        {
            _requestService.UpdateListRequestStatus(requestStatusDto);
            return Ok();
        }

        //[HttpPost]
        //[Route("UpdateNoteRequest")]
        //[AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View, QuickspatchClientType = QuickspatchClientType.MobileClient)]
        //public IHttpActionResult UpdateListRequestStatus(RequestStatusDto requestStatusDto)
        //{
        //    _requestService.UpdateListRequestStatus(requestStatusDto);
        //    return Ok();
        //}

    }
}