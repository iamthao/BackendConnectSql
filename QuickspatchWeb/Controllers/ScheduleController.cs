using System;
using System.Collections.Generic;
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
using QuickspatchWeb.Models.Schedule;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Authentication;

namespace QuickspatchWeb.Controllers
{
    public class ScheduleController : ApplicationControllerGeneric<Schedule, DashboardScheduleDataViewModel>
    {  
        private readonly IScheduleService _scheduleService;
        private readonly IGridConfigService _gridConfigService;
        public ScheduleController(IAuthenticationService authenticationService, IDiagnosticService diagnosticService, IGridConfigService gridConfigService,
            IScheduleService scheduleService)
            : base(authenticationService, diagnosticService, scheduleService)
        {
            _scheduleService = scheduleService;
            _gridConfigService = gridConfigService;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Schedule, OperationAction = OperationAction.View)]
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetSchedulesOfCourier(int courierId)
        {
            var listScheduleOfCourier = _scheduleService.GetSchedulesOfCourier(courierId);
            return Json(listScheduleOfCourier, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDetailScheduleWeekly(int courierId, DateTime fromDate, DateTime toDate, int timezone)
        {
            var listScheduleOfCourier = _scheduleService.GetDetailScheduleWeekly(courierId, fromDate, toDate, timezone);
            return Json(listScheduleOfCourier, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDetailScheduleMonthly(int courierId, DateTime fromDate, DateTime toDate, int timezone)
        {
            var listScheduleOfCourier = _scheduleService.GetDetailScheduleMonthly(courierId, fromDate, toDate, timezone);
            return Json(listScheduleOfCourier, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Schedule, OperationAction = OperationAction.Add)]
        public ActionResult Create()
        {
            var viewModel = new DashboardScheduleDataViewModel
            {
                SharedViewModel = new DashboardScheduleShareViewModel
                {
                    CreateMode = true
                }
            };
            return View(viewModel);
        }

        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Schedule, OperationAction = OperationAction.Add)]
        public ActionResult Create(ScheduleParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);
            var entity = viewModel.MapTo<Schedule>();
            var savedEntity = MasterFileService.Add(entity);
            if (savedEntity.WarningInfo != null && !savedEntity.Confirm.GetValueOrDefault())
            {
                return Json(new { savedEntity.Id, entity.WarningInfo, Error = "ErrorWarning" }, JsonRequestBehavior.DenyGet);
            }
            return Json(new { savedEntity.Id }, JsonRequestBehavior.DenyGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Schedule, OperationAction = OperationAction.Update)]
        public ActionResult Update(int id)
        {
            var viewModel = GetMasterFileViewModel(id);
            return View(viewModel);
        }

        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Schedule, OperationAction = OperationAction.Update)]
        public ActionResult Update(ScheduleParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);

            byte[] lastModified = null;

            if (ModelState.IsValid)
            {
                var entity = MasterFileService.GetById(viewModel.SharedViewModel.Id);
               
                var mappedEntity = viewModel.MapPropertiesToInstance(entity);
                lastModified = MasterFileService.Update(mappedEntity).LastModified;
                if (entity.WarningInfo != null && !entity.Confirm.GetValueOrDefault())
                {
                    return Json(new { entity.Id, LastModified = lastModified, entity.WarningInfo, Error = "ErrorWarning" }, JsonRequestBehavior.DenyGet);
                }
                return Json(new { entity.Id, LastModified = lastModified}, JsonRequestBehavior.DenyGet);
            }

            return Json(new { Error = string.Empty, Data = new { LastModified = lastModified } }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Schedule, OperationAction = OperationAction.Delete)]
        public JsonResult Delete(int id)
        {
            //MasterFileService.DeleteById(id);
            _scheduleService.DeleteSchedule(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Schedule, OperationAction = OperationAction.View)]
        public ActionResult Warning(string data)
        {
            var result = EncryptHelper.Base64Decode(data);
            var obj = JsonConvert.DeserializeObject<WarningScheduleVo>(result);
            return View(obj);
        }
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Schedule, OperationAction = OperationAction.View)]
        public ActionResult WarningInfo(int scheduleId, int courierId)
        {
            WarningScheduleVo obj = _scheduleService.GetWarningInfo(scheduleId, courierId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Schedule, OperationAction = OperationAction.View)]
        public ActionResult WarningDetail(string data)
        {
            var result = EncryptHelper.Base64Decode(data);
            var obj = JsonConvert.DeserializeObject<WarningScheduleVo>(result);
            return View(obj);
        }
    }
}