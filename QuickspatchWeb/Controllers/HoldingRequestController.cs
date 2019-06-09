using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Mvc;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using Framework.DomainModel.ValueObject;
using Framework.Mapping;
using Framework.Service.Diagnostics;
using Framework.Utility;
using Newtonsoft.Json;
using QuickspatchWeb.Attributes;
using QuickspatchWeb.Models;
using QuickspatchWeb.Models.HoldingRequest;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Authentication;

namespace QuickspatchWeb.Controllers
{
    public class HoldingRequestController : ApplicationControllerGeneric<HoldingRequest, DashboardHoldingRequestDataViewModel>
    {
        private readonly IHoldingRequestService _holdingRequestService;
        private readonly IGridConfigService _gridConfigService;

        public HoldingRequestController(IAuthenticationService authenticationService, IDiagnosticService diagnosticService, IGridConfigService gridConfigService,
            IHoldingRequestService holdingRequestService)
            : base(authenticationService, diagnosticService, holdingRequestService)
        {
            _holdingRequestService = holdingRequestService;
            _gridConfigService = gridConfigService;
        }
        
        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.HoldingRequest, OperationAction = OperationAction.Add)]
        public int Create(HoldingRequestParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);
            var entity = viewModel.MapTo<HoldingRequest>();
            entity.SendDate = entity.SendDate.GetValueOrDefault().ToUtcTimeFromClientTime();
            var savedEntity = MasterFileService.Add(entity);

            return savedEntity.Id;
        }

        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.HoldingRequest, OperationAction = OperationAction.Update)]
        public ActionResult Update(HoldingRequestParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);

            byte[] lastModified = null;

            if (ModelState.IsValid)
            {
                var entity = MasterFileService.GetById(viewModel.SharedViewModel.Id);
                var mappedEntity = viewModel.MapPropertiesToInstance(entity);
                mappedEntity.SendDate = mappedEntity.SendDate.GetValueOrDefault().ToUtcTimeFromClientTime();
                lastModified = MasterFileService.Update(mappedEntity).LastModified;
            }

            return Json(new { Error = string.Empty, Data = new { LastModified = lastModified } }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.HoldingRequest, OperationAction = OperationAction.Delete)]
        public JsonResult Delete(int id)
        {
            MasterFileService.DeleteById(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.HoldingRequest, OperationAction = OperationAction.Delete)]
        public JsonResult DeleteMulti(string selectedRowIdArray)
        {
            if (String.IsNullOrEmpty(selectedRowIdArray))
                return Json(false, JsonRequestBehavior.AllowGet);

            var listItem = JsonConvert.DeserializeObject<Collection<int>>(selectedRowIdArray);
            if (listItem.Count == 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            _holdingRequestService.DeleteAll(p=>listItem.Contains(p.Id));
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.HoldingRequest, OperationAction = OperationAction.Delete)]
        public JsonResult SendHoldingRequest(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                var holdingRequestItem = JsonConvert.DeserializeObject<SendHoldingRequestItemVo>(data);
                if (holdingRequestItem != null)
                {
                    if (holdingRequestItem.IsStat)
                    {
                        holdingRequestItem.SendingTime = DateTime.UtcNow;
                    }
                    holdingRequestItem.CourierId =
                        holdingRequestItem.CourierId <= 0  ? null : holdingRequestItem.CourierId;
                }
                _holdingRequestService.SendHoldingRequest(holdingRequestItem);
            }
            
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.HoldingRequest, OperationAction = OperationAction.Delete)]
        public JsonResult SendListHoldingRequest(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                var holdingRequestItem = JsonConvert.DeserializeObject<SendListHoldingRequestItemVo>(data);
                if (holdingRequestItem != null)
                {
                    if (holdingRequestItem.IsStat)
                    {
                        holdingRequestItem.SendingTime = DateTime.UtcNow;
                    }

                    holdingRequestItem.CourierId =
                        holdingRequestItem.CourierId <= 0  ? null : holdingRequestItem.CourierId;
                }
                _holdingRequestService.SendListHoldingRequest(holdingRequestItem);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        
    }
}