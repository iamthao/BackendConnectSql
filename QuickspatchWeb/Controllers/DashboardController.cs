using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using QuickspatchWeb.Attributes;
using ServiceLayer.Interfaces;

namespace QuickspatchWeb.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ISystemEventService _systemEventService;
        public DashboardController(ISystemEventService systemEventService)
        {
            _systemEventService = systemEventService;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Dashboard, OperationAction = OperationAction.View)]
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Dashboard, OperationAction = OperationAction.View)]
        public JsonResult GetEvents()
        {
            return Json(_systemEventService.GetEventsDashboard(), JsonRequestBehavior.AllowGet);
        }
    }
}