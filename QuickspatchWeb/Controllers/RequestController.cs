using System;
using System.Collections.Generic;
using System.Net;
using System.Transactions;
using System.Web.Mvc;
using Framework.DomainModel.Common;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using Framework.DomainModel.ValueObject;
using Framework.Mapping;
using Framework.Service.Diagnostics;
using Framework.Service.Translation;
using Framework.Utility;
using Newtonsoft.Json;
using QuickspatchWeb.Attributes;
using QuickspatchWeb.Models;
using QuickspatchWeb.Models.HoldingRequest;
using QuickspatchWeb.Models.Request;
using QuickspatchWeb.Services.Interface;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Authentication;

namespace QuickspatchWeb.Controllers
{
    public partial class RequestController : ApplicationControllerGeneric<Request, DashboardRequestDataViewModel>
    {
        private readonly IRequestService _requestService;
        private readonly IRenderViewToString _renderViewToString;
        private readonly IStaticValueService _staticValueService;
        private readonly IGridConfigService _gridConfigService;
        private readonly INoteRequestService _noteRequestService;
        private readonly ISystemEventService _systemEventService;
        private readonly ILocationService _locationService;
        private readonly IGoogleService _googleService;
        private readonly ISystemConfigurationService _systemConfigurationService;
        private readonly IHoldingRequestService _holdingRequestService;
        private readonly IUserService _userService;
        private readonly IDiagnosticService _diagnosticService;


        public RequestController(IAuthenticationService authenticationService,  IGridConfigService gridConfigService,
            IRequestService requestService, IStaticValueService staticValueService, IRenderViewToString renderViewToString, INoteRequestService noteRequestService,
            ISystemEventService systemEventService, ILocationService locationService, IGoogleService googleService,IUserService userService,
            IHoldingRequestService holdingRequestService, ISystemConfigurationService systemConfigurationService, IDiagnosticService diagnosticService)
            : base(authenticationService, diagnosticService, requestService)
        {
            _requestService = requestService;
            _staticValueService = staticValueService;
            _gridConfigService = gridConfigService;
            _renderViewToString = renderViewToString;
            _noteRequestService = noteRequestService;
            _systemEventService = systemEventService;
            _locationService = locationService;
            _googleService = googleService;
            _systemConfigurationService = systemConfigurationService;
            _holdingRequestService = holdingRequestService;
            _userService = userService;
            _diagnosticService = diagnosticService;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Request, OperationAction = OperationAction.View)]
        public ActionResult Index()
        {
            return View(new DashboardRequestIndexViewModel());
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Request, OperationAction = OperationAction.View)]
        public JsonResult GetDataForHoldingRequest(HoldingRequestQueryInfo queryInfo)
        {
            var queryData = _requestService.GetListHoldingRequest(queryInfo);
            var clientsJson = Json(queryData, JsonRequestBehavior.AllowGet);
            return clientsJson;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Request, OperationAction = OperationAction.View)]
        public JsonResult GetListRequest(RequestQueryInfo queryInfo)
        {
            var queryData = _requestService.GetDataForGridMasterfile(queryInfo);
            var clientsJson = Json(queryData, JsonRequestBehavior.AllowGet);
            return clientsJson;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Request, OperationAction = OperationAction.View)]
        public JsonResult GetRequestListByCourier(QueryInfo queryInfo)
        {
            var queryData = _requestService.GetRequestListByCourier(queryInfo);
            var clientsJson = Json(queryData, JsonRequestBehavior.AllowGet);
            return clientsJson;
        }

        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Request, OperationAction = OperationAction.Add)]
        public ActionResult Create(RequestParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);
            var entity = viewModel.MapTo<Request>();

            if (entity.CourierId > 0)
            {
                var courier = _userService.GetById(entity.CourierId.GetValueOrDefault());
                if (courier.IsActive == false)
                {
                    throw new Exception("Cannot assign request to inactive courier");
                }
            }
            
            if (entity.IsStat == true)
            {
                entity.Status = (int)StatusRequest.Sent;
                entity.SendingTime = DateTime.UtcNow;
            }
            else
            {
                entity.Status = (int)StatusRequest.NotSent;
            }
            entity.RequestNo = _staticValueService.GetNewRequestNumber();

            //Tinh estimate distance, estimate time
            var estimateDistance = 0;
            var estimateTime = 0;
            if (entity.LocationFrom > 0 && entity.LocationTo > 0)
            {
                var fromLocation = _locationService.GetById(entity.LocationFrom);
                var fromPoint = new GoogleMapPoint
                {
                    Lat = fromLocation.Lat.GetValueOrDefault(),
                    Lng = fromLocation.Lng.GetValueOrDefault()
                };

                var toLocation = _locationService.GetById(entity.LocationTo);
                var toPoint = new GoogleMapPoint
                {
                    Lat = toLocation.Lat.GetValueOrDefault(),
                    Lng = toLocation.Lng.GetValueOrDefault()
                };
                var dataGoogle = _googleService.GetDistance(fromPoint, toPoint);
                if (dataGoogle.status == "OK")
                {
                    if (dataGoogle.rows[0].elements[0].status == "OK")
                    {
                        estimateDistance = dataGoogle.rows[0].elements[0].distance.value;
                        estimateTime = dataGoogle.rows[0].elements[0].duration.value;
                    }                  
                }
            }

            entity.EstimateDistance = estimateDistance;
            entity.EstimateTime = estimateTime;
            Request savedEntity;
            using (var transaction = new TransactionScope())
            {
                savedEntity = MasterFileService.Add(entity);
                if (viewModel.SharedViewModel is DashboardRequestShareViewModel)
                {
                    var holdingRequestId = ((DashboardRequestShareViewModel)viewModel.SharedViewModel).HoldingRequestId;
                    if (holdingRequestId.GetValueOrDefault() > 0)
                    {
                        _holdingRequestService.DeleteById(holdingRequestId.GetValueOrDefault());
                    }
                } 
                transaction.Complete();
            }
            
            if (savedEntity != null)
            {

                return Json(new {savedEntity.WarningInfo, savedEntity.Id}, JsonRequestBehavior.DenyGet);
            }
            return Json(1, JsonRequestBehavior.DenyGet);
        }


        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Request, OperationAction = OperationAction.Update)]
        public JsonResult Update(RequestParameter parameters)
        {
                     
            var viewModel = MapFromClientParameters(parameters);

            byte[] lastModified = null;
            var error = "";
                var newCourier = ((DashboardRequestShareViewModel) viewModel.SharedViewModel).CourierId;
                var entity = MasterFileService.GetById(viewModel.SharedViewModel.Id);

                if (entity != null && entity.Status != (int) StatusRequest.NotSent)
                {
                    error = string.Format(SystemMessageLookup.GetMessage("RequestCannotUpdate"), entity.Status.GetNameByValue<StatusRequest>());
                }
                else
                {
                    if (entity.CourierId > 0 && newCourier != entity.CourierId)
                    {
                        var courier = _userService.GetById(entity.CourierId.GetValueOrDefault());
                        if (courier.IsActive == false)
                        {
                            throw new Exception("Cannot assign request to inactive courier");
                        }
                    }
                    var mappedEntity = viewModel.MapPropertiesToInstance(entity);
                    if (mappedEntity.IsStat == true)
                    {
                        mappedEntity.Status = (int)StatusRequest.Sent;
                        mappedEntity.SendingTime = DateTime.UtcNow;
                    }
                    else
                    {
                        mappedEntity.Status = (int)StatusRequest.NotSent;
                    }

                    //Tinh estimate distance, estimate time
                    var estimateDistance = 0;
                    var estimateTime = 0;
                    if (entity != null && entity.LocationFrom > 0 && entity.LocationTo > 0)
                    {
                        var fromLocation = _locationService.GetById(entity.LocationFrom);
                        var fromPoint = new GoogleMapPoint
                        {
                            Lat = fromLocation.Lat.GetValueOrDefault(),
                            Lng = fromLocation.Lng.GetValueOrDefault()
                        };

                        var toLocation = _locationService.GetById(entity.LocationTo);
                        var toPoint = new GoogleMapPoint
                        {
                            Lat = toLocation.Lat.GetValueOrDefault(),
                            Lng = toLocation.Lng.GetValueOrDefault()
                        };
                        var dataGoogle = _googleService.GetDistance(fromPoint, toPoint);
                        if (dataGoogle.status == "OK")
                        {
                            if (dataGoogle.rows[0].elements[0].status == "OK")
                            {
                                estimateDistance = dataGoogle.rows[0].elements[0].distance.value;
                                estimateTime = dataGoogle.rows[0].elements[0].duration.value;
                            }
                        }
                    }

                    mappedEntity.EstimateDistance = estimateDistance;
                    mappedEntity.EstimateTime = estimateTime;
            
                    lastModified = MasterFileService.Update(mappedEntity).LastModified;
                }
                return Json(new { entity.WarningInfo, entity.Id, LastModified = lastModified, Error = error }, JsonRequestBehavior.DenyGet);

        }

      
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Request, OperationAction = OperationAction.Update)]
        public JsonResult CancelRequest(int id)
        {
            return Json(_requestService.CancelRequest(id), JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Request, OperationAction = OperationAction.Update)]
        public JsonResult ReAssignCourier(int id, int courierId)
        {
            return Json(_requestService.ReAssignCourier(id, courierId), JsonRequestBehavior.AllowGet);
        }
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Request, OperationAction = OperationAction.View)]
        public JsonResult GetPieChartData(int? courierId)
        {
            var queryData = _requestService.GetPieChartData(courierId);
            var clientsJson = Json(queryData, JsonRequestBehavior.AllowGet);
            return clientsJson;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Request, OperationAction = OperationAction.View)]
        public JsonResult GetCurrentDataRequests(DashboardRequestQueryInfo queryInfo)
        {
            var queryData = _requestService.GetCurrentDataRequests(queryInfo);
            var clientsJson = Json(queryData, JsonRequestBehavior.AllowGet);
            return clientsJson;
        }


        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Request, OperationAction = OperationAction.View)]
        public JsonResult ExportExcel(List<ColumnModel> gridColumns, RequestQueryInfo queryInfo)
        {
            return base.ExportExcelMasterfile(gridColumns, queryInfo);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Request, OperationAction = OperationAction.View)]
        public FileResult DownloadExcelFile(string fileName)
        {
            return base.DownloadExcelMasterFile(fileName);
        }


        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.None)]
        public JsonResult GetSpatchTimeDefault()
        {
            var spatchTimeDefault = 0;
            var configSpatchTimeDefault = _systemConfigurationService.FirstOrDefault(o => o.SystemConfigType == SystemConfigType.DispatchTimeDefault);
            if (configSpatchTimeDefault != null)
            {
                spatchTimeDefault = Convert.ToInt32(configSpatchTimeDefault.Value);
            }


            var a = DateTime.UtcNow;
            var b = DateTime.UtcNow.AddMinutes(spatchTimeDefault);
            var c = DateTime.UtcNow.AddMinutes(spatchTimeDefault).ToClientTimeDateTime();


            var clientsJson = Json(new
            {              
                SpatchTimeDefault = DateTime.UtcNow.AddMinutes(spatchTimeDefault).ToClientTimeDateTime().ToString("hh:mm tt")
            }, JsonRequestBehavior.AllowGet);
            return clientsJson;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Request, OperationAction = OperationAction.View)]
        public ActionResult Warning(string data)
        {
            var result = EncryptHelper.Base64Decode(data);
            var obj = JsonConvert.DeserializeObject<WarningInfoVo>(result);
            return View(obj);
        }
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Request, OperationAction = OperationAction.View)]
        public ActionResult WarningInfo(int requestId, int courierId)
        {
            var obj = _requestService.GetWarningInfo(requestId, courierId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Request, OperationAction = OperationAction.View)]
        public ActionResult WarningDetail(string data)
        {
            var result = EncryptHelper.Base64Decode(data.Replace(" ", "+"));
            var obj = JsonConvert.DeserializeObject<WarningInfoVo>(result);
            return View(obj);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Request, OperationAction = OperationAction.View)]
        public ActionResult PartialHoldingRequest()
        {
            return PartialView("_PartialHoldingRequest");
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Request, OperationAction = OperationAction.View)]
        public ActionResult PartialRequest()
        {
            return PartialView("_PartialRequest");
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Request, OperationAction = OperationAction.View)]
        public ActionResult PartialCreateHoldingRequest(int id)
        {
            if (id > 0)
            {
                var entity = _holdingRequestService.GetById(id);
                var viewModel = entity.MapTo<DashboardHoldingRequestShareViewModel>();
                var fromName = _locationService.GetById(entity.LocationFrom);
                viewModel.LocationFromName = fromName != null ? fromName.Name : "";
                var toName = _locationService.GetById(entity.LocationTo);
                viewModel.LocationToName = toName != null ? toName.Name : "";
                viewModel.CreateMode = false;
                return PartialView("_CreateHoldingRequest", viewModel); 
            }
            return PartialView("_CreateHoldingRequest", new DashboardHoldingRequestShareViewModel{CreateMode = true});
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Request, OperationAction = OperationAction.View)]
        public ActionResult PartialSendHoldingRequest(int id)
        {
            return PartialView("_SendHoldingRequest", new SendHoldingRequestViewModel { Id = id});
        } 

    }
}