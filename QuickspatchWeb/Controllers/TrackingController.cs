using System;
using System.Web.Mvc;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using Framework.Service.Diagnostics;
using QuickspatchWeb.Attributes;
using QuickspatchWeb.Models;
using QuickspatchWeb.Models.Tracking;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Authentication;

namespace QuickspatchWeb.Controllers
{
    public class TrackingController : ApplicationControllerGeneric<Tracking, DashboardTrackingDataViewModel>
    {
        private readonly IGridConfigService _gridConfigService;
        private readonly ITrackingService _trackingService;

        public TrackingController(IAuthenticationService authenticationService,
            IDiagnosticService diagnosticService,
            ITrackingService trackingService,
            IGridConfigService gridConfigService)
            : base(authenticationService, diagnosticService, trackingService)
        {
            _gridConfigService = gridConfigService;
            _trackingService = trackingService;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Tracking, OperationAction = OperationAction.View)]
        public ActionResult Index()
        {
            var viewModel = new DashboardTrackingIndexViewModel();
            return View(viewModel);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Tracking, OperationAction = OperationAction.View)]
        public JsonResult GetListTrackingData(int courierId, DateTime? filterDateTime, int? requestId)
        {
            var queryData = _trackingService.GetListTrackingData(courierId, filterDateTime ?? DateTime.MinValue, requestId);
            var clientsJson = Json(queryData, JsonRequestBehavior.AllowGet);

            return clientsJson;
        }
        
    }
}