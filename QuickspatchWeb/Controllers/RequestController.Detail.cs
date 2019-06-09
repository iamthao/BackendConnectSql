using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using QuickspatchWeb.Attributes;

namespace QuickspatchWeb.Controllers
{
    public partial class RequestController
    {
        public ActionResult Detail()
        {
            return View();
        }
        public ActionResult DetailTracking()
        {
            return View();
        }
        public ActionResult DetailRequest()
        {
            return View();
        }
        public ActionResult DetailComplete()
        {
            return View();
        }
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Request, OperationAction = OperationAction.View)]
        public ActionResult GetNotesDetail(int requestId)
        {
            dynamic queryData = _requestService.GetNotesDetail(requestId);
            var clientsJson = Json(queryData, JsonRequestBehavior.AllowGet);
            return clientsJson;
        }
        
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Request, OperationAction = OperationAction.View)]
        public ActionResult GetRequestForTracking(int requestId)
        {
            dynamic queryData = _requestService.GetRequestForTracking(requestId);
            var clientsJson = Json(queryData, JsonRequestBehavior.AllowGet);
            return clientsJson;
        }
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Request, OperationAction = OperationAction.View)]
        public ActionResult GetRequestNoForTracking(string requestNo)
        {
            dynamic queryData = _requestService.GetRequestForTracking(requestNo: requestNo);
            var clientsJson = Json(queryData, JsonRequestBehavior.AllowGet);
            return clientsJson;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Request, OperationAction = OperationAction.View)]
        public ActionResult GetPictureAndNoteRequestComplete(int requestId)
        {
            dynamic queryData = _requestService.GetPictureAndNoteRequestComplete(requestId);
            var clientsJson = Json(queryData, JsonRequestBehavior.AllowGet);
            return clientsJson;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Request, OperationAction = OperationAction.View)]
        public ActionResult GetListTrackingDataFromTo(int fromId, int toId)
        {
            dynamic queryData = _requestService.GetListTrackingDataFromTo(fromId, toId);
            var clientsJson = Json(queryData, JsonRequestBehavior.AllowGet);
            return clientsJson;
        }
    }
}