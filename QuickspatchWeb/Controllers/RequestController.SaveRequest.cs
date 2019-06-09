using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using QuickspatchWeb.Attributes;
using QuickspatchWeb.Models;

namespace QuickspatchWeb.Controllers
{
	public partial class RequestController
	{
	    public ActionResult SaveRequest()
	    {
            return View(new ViewModelBase());
	    }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Request, OperationAction = OperationAction.View)]
        public ActionResult GetRequestCourierForCreate(int courierId)
        {
            return Json(_requestService.GetRequestCourierForCreate(courierId), JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Request, OperationAction = OperationAction.View)]
        public ActionResult GetLatLngForLocation(int locationId)
        {
            dynamic data = _requestService.GetLatLngForLocation(locationId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
	}
}