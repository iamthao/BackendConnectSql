using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using Framework.Service.Diagnostics;
using QuickspatchApi.Attributes;
using ServiceLayer.Interfaces;

namespace QuickspatchApi.Controllers
{
    [RoutePrefix("api/Tracking")]
    public class TrackingController : ApplicationControllerBase
    {
        private readonly IDiagnosticService _diagnosticService;
        private readonly ITrackingService _trackingService;

        public TrackingController(IDiagnosticService diagnosticService, ITrackingService trackingService)
            : base(diagnosticService, null)        {
            _diagnosticService = diagnosticService;
            _trackingService = trackingService;
        }

        [HttpPost]
        [Route("UpdateTrackingHistory")]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View, QuickspatchClientType = QuickspatchClientType.MobileClient)]
        public IHttpActionResult UpdateTrackingHistory(TrackingRequestDto trackingRequestDto)
        {
            _trackingService.UpdateTrackingHistory(trackingRequestDto);
            return Ok();
        }
    }
}