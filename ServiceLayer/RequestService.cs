using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Transactions;
using AutoMapper;
using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Common;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;
using Framework.Mapping;
using Framework.Service.Diagnostics;
using Framework.Service.Translation;
using Framework.Utility;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;

namespace ServiceLayer
{
    public class RequestService : MasterFileService<Request>, IRequestService
    {
        private readonly IRequestRepository _requestRepository;
        private readonly ISystemEventRepository _systemEventRepository;
        private readonly IRequestHistoryRepository _requestHistoryRepository;
        private readonly IHoldingRequestRepository _holdingRequestRepository;
        private readonly ICourierRepository _courierRepository;
        private readonly IStaticValueService _staticValueService;
        private readonly INoteRequestRepository _noteRequestRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly ISystemConfigurationRepository _systemConfigurationRepository;
        private readonly IGoogleService _googleService;
        private readonly ILocationRepository _locationRepository;
        private readonly IDiagnosticService _diagnosticService;

        public RequestService(ITenantPersistenceService tenantPersistenceService,
            IRequestRepository requestRepository,
            IHoldingRequestRepository holdingRequestRepository,
            IRequestHistoryRepository requestHistoryRepository,
            ISystemEventRepository systemEventRepository,
            ICourierRepository courierRepository,
            IStaticValueService staticValueService,
            INoteRequestRepository noteRequestRepository,
            ISystemConfigurationRepository systemConfigurationRepository,
            IScheduleRepository scheduleRepository, IGoogleService googleService,
            ILocationRepository locationRepository,
            IDiagnosticService diagnosticService,
            IBusinessRuleSet<Request> businessRuleSet = null)
            : base(requestRepository, requestRepository, tenantPersistenceService, businessRuleSet)
        {
            _requestRepository = requestRepository;
            _holdingRequestRepository = holdingRequestRepository;
            _requestHistoryRepository = requestHistoryRepository;
            _systemEventRepository = systemEventRepository;
            _courierRepository = courierRepository;
            _staticValueService = staticValueService;
            _noteRequestRepository = noteRequestRepository;
            _systemConfigurationRepository = systemConfigurationRepository;
            _scheduleRepository = scheduleRepository;
            _googleService = googleService;
            _locationRepository = locationRepository;
            _diagnosticService = diagnosticService;
        }

        public override Request Add(Request entity)
        {
            ValidateBusinessRules(entity);
            //ValidationSameRankRequest(entity);
            WarningInfoVo warningInfoVo;
            var listRequest = WarningProcess(entity, out warningInfoVo);
            var result = listRequest.FirstOrDefault(o => o.Id == entity.Id);
            //if (result != null && result.IsWarning.GetValueOrDefault() && !result.Confirm.GetValueOrDefault())
            //{
            //    warningInfoVo.IsUpdate = false;
            //    result.WarningInfo = warningInfoVo;
            //    return result;
            //}

            foreach (var request in listRequest)
            {
                if (request.Id == 0)
                {
                    result = request;
                    _requestRepository.Add(result);
                }
                else
                {
                    _requestRepository.Update(request);
                }
            }
            _requestRepository.Commit();
            return result;
            //var request = base.Add(entity);

            //if (entity.SaveSystemEvent ?? true)
            //{
            //    var courier = _courierRepository.GetById(entity.CourierId.GetValueOrDefault()).User;

            //    _systemEventRepository.Add(EventMessage.RequestCreated,
            //            new Dictionary<EventMessageParam, string>()
            //                {
            //                    {EventMessageParam.Request, request.RequestNo},
            //                    {EventMessageParam.Courier, courier.LastName + " " + courier.FirstName + (string.IsNullOrEmpty(courier.MiddleName) ? "" : " " + courier.MiddleName)},
            //                });
            //    _systemEventRepository.Commit();
            //}

            //return request;
        }



        public override Request Update(Request model)
        {
            ValidateBusinessRules(model);
            //ValidationSameRankRequest(model);
            WarningInfoVo warningInfoVo;
            var listRequest = WarningProcess(model, out warningInfoVo);
            var result = listRequest.FirstOrDefault(o => o.Id == model.Id);
            //if (result != null && result.IsWarning.GetValueOrDefault() && !result.Confirm.GetValueOrDefault())
            //{
            //    warningInfoVo.IsUpdate = true;
            //    result.WarningInfo = warningInfoVo;
            //    return result;
            //}

            foreach (var request in listRequest)
            {
                if (request.Id == model.Id)
                {
                    result = request;
                    _requestRepository.Update(result);
                }
                else
                {
                    _requestRepository.Update(request);
                }
            }
            _requestRepository.Commit();
            return result;
        }

        public WarningInfoVo GetWarningInfo(int requestId, int courierId)
        {
            var listRequestDatetimeNow = _requestRepository.GetListRequestByCourier(requestId,courierId);
            listRequestDatetimeNow = listRequestDatetimeNow.OrderBy(o => o.StartTime).ToList();
            var request = listRequestDatetimeNow.FirstOrDefault(o => o.Id == requestId);
            if (request != null && request.IsWarning.GetValueOrDefault())
            {
                Request preivousRequest = null;
                foreach (var item in listRequestDatetimeNow)
                {
                    if (listRequestDatetimeNow.Count == 1 || (preivousRequest == null && item.Id == requestId))
                    {
                        return new WarningInfoVo
                        {
                            RequestNo = item.RequestNo,
                            FromAddress = CaculatorHelper.GetFullAddress(item.LocationFromObj.Address1, item.LocationFromObj.Address2,item.LocationFromObj.City, item.LocationFromObj.StateOrProvinceOrRegion, item.LocationFromObj.Zip),
                            ToAddress = CaculatorHelper.GetFullAddress(item.LocationToObj.Address1, item.LocationToObj.Address2, item.LocationToObj.City,item.LocationToObj.StateOrProvinceOrRegion, item.LocationToObj.Zip),
                            DistanceFromTo = item.EstimateDistance.GetValueOrDefault(),
                            TimeFromTo = item.EstimateTime.GetValueOrDefault(),
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            SendingTime = item.SendingTime.GetValueOrDefault()
                        };
                    }
                    if (item.Id == requestId && preivousRequest != null)
                    {
                        return new WarningInfoVo
                        {
                            PreviousRequestNo = preivousRequest.RequestNo,
                            PreviousToAddress = CaculatorHelper.GetFullAddress(preivousRequest.LocationToObj.Address1, preivousRequest.LocationToObj.Address2, preivousRequest.LocationToObj.City, preivousRequest.LocationToObj.StateOrProvinceOrRegion, preivousRequest.LocationToObj.Zip),
                            PreviousFromAddress = CaculatorHelper.GetFullAddress(preivousRequest.LocationFromObj.Address1, preivousRequest.LocationFromObj.Address2, preivousRequest.LocationFromObj.City, preivousRequest.LocationFromObj.StateOrProvinceOrRegion, preivousRequest.LocationFromObj.Zip),
                            PreviousToAddressName = preivousRequest.LocationToObj.Name.Length > 25 ? preivousRequest.LocationToObj.Name.Remove(25).Insert(25,"...") :  preivousRequest.LocationToObj.Name,
                            PreviousFromAddressName = preivousRequest.LocationFromObj.Name.Length > 25 ? preivousRequest.LocationFromObj.Name.Remove(25).Insert(25, "...") : preivousRequest.LocationFromObj.Name,                           
                            PreviousSendingTime = preivousRequest.SendingTime,
                            PreviousStartTime = preivousRequest.StartTime,
                            PreviousEndTime = preivousRequest.EndTime,
                            RequestNo = item.RequestNo,
                            FromAddress = CaculatorHelper.GetFullAddress(item.LocationFromObj.Address1, item.LocationFromObj.Address2, item.LocationFromObj.City, item.LocationFromObj.StateOrProvinceOrRegion, item.LocationFromObj.Zip),
                            ToAddress = CaculatorHelper.GetFullAddress(item.LocationToObj.Address1, item.LocationToObj.Address2, item.LocationToObj.City, item.LocationToObj.StateOrProvinceOrRegion, item.LocationToObj.Zip),
                            FromAddressName = item.LocationFromObj.Name.Length > 25 ? item.LocationFromObj.Name.Remove(25).Insert(25, "...") : item.LocationFromObj.Name,                           
                            ToAddressName = item.LocationToObj.Name.Length > 25? item.LocationToObj.Name.Remove(25).Insert(25, "...") : item.LocationToObj.Name,                                                    
                            DistanceEndFrom = item.DistanceEndFrom.GetValueOrDefault(),
                            DistanceFromTo = item.EstimateDistance.GetValueOrDefault(),
                            TimeEndFrom = item.TimeEndFrom.GetValueOrDefault(),
                            TimeFromTo = item.EstimateTime.GetValueOrDefault(),
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            SendingTime = item.SendingTime.GetValueOrDefault()
                        };

                    }
                    preivousRequest = item;
                }
            }
            
            return new WarningInfoVo();
        }

        #region Validation same rank create request.

        private void ValidationSameRankRequest(Request entity)
        {
            //Start - End date in date
            var startNowTime = DateTime.Parse(DateTime.UtcNow.ToString("MM/dd/yyyy")).ToUniversalTime();
            var endNowTime = DateTime.Parse(DateTime.UtcNow.ToString("MM/dd/yyyy")).AddMinutes(1740).ToUniversalTime();

            var listRequestDatetimeNow =
                _requestRepository.Get(
                    o => o.CourierId == entity.CourierId && o.StartTime >= startNowTime && o.EndTime <= endNowTime).ToList();
            // don't any request for this courier.
            if (listRequestDatetimeNow.Count == 0)
            {
                return;
            }
            if (entity.Id != 0)
            {
                Func<Request, bool> condition = o => o.Id == entity.Id;
                if (listRequestDatetimeNow.FirstOrDefault(condition) != null)
                {
                    listRequestDatetimeNow.First(condition).LocationFrom = entity.LocationFrom;
                    listRequestDatetimeNow.First(condition).LocationTo = entity.LocationTo;
                    listRequestDatetimeNow.First(condition).SendingTime = entity.SendingTime;
                    listRequestDatetimeNow.First(condition).StartTime = entity.StartTime;
                    listRequestDatetimeNow.First(condition).EndTime = entity.EndTime;
                    listRequestDatetimeNow.First(condition).NoteRequests = entity.NoteRequests;
                }
            }
            foreach (var item in listRequestDatetimeNow)
            {
                if ((item.StartTime >= entity.StartTime && item.EndTime <= entity.StartTime) ||
                    (item.StartTime >= entity.EndTime && item.EndTime <= entity.EndTime))
                {
                    throw new Exception(string.Format(SystemMessageLookup.GetMessage("OutRangeRequest"), item.RequestNo));
                }
            }
        }
        #endregion

        #region Calculator Locgic Warning

        private List<Request> WarningProcess(Request entity, out WarningInfoVo warningInfoVo)
        {                            
            warningInfoVo = null;
            var listRequestDatetimeNow =
                _requestRepository.GetListRequestByCourier(entity.Id, entity.CourierId.GetValueOrDefault());
            //if (listRequestDatetimeNow.Count == 0)
            //{
            //    return new List<Request> {entity};
            //}
            //With Create

            if (entity.Id == 0)
            {
                listRequestDatetimeNow.Add(entity);
            }
            //With Update
            else
            {
                Func<Request, bool> condition = o => o.Id == entity.Id;
                if (listRequestDatetimeNow.FirstOrDefault(condition) != null)
                {
                    listRequestDatetimeNow.First(condition).LocationFrom = entity.LocationFrom;
                    listRequestDatetimeNow.First(condition).LocationTo = entity.LocationTo;
                    listRequestDatetimeNow.First(condition).SendingTime = entity.SendingTime;
                    listRequestDatetimeNow.First(condition).StartTime = entity.StartTime;
                    listRequestDatetimeNow.First(condition).EndTime = entity.EndTime;
                    listRequestDatetimeNow.First(condition).NoteRequests = entity.NoteRequests;
                }
                else
                {
                    var obj = _requestRepository.GetById(entity.Id);
                    if (obj != null)
                    {
                        obj.LocationFrom = entity.LocationFrom;
                        obj.LocationTo = entity.LocationTo;
                        obj.SendingTime = entity.SendingTime;
                        obj.StartTime = entity.StartTime;
                        obj.EndTime = entity.EndTime;
                        obj.NoteRequests = entity.NoteRequests;
                        obj.CourierId = entity.CourierId;
                        listRequestDatetimeNow.Add(obj);
                    }
                }
            }
            listRequestDatetimeNow = listRequestDatetimeNow.Where(o => o.StartTime > DateTime.UtcNow).OrderBy(o => o.StartTime).ToList();
            
            Request preivousRequest = null;
            foreach (var item in listRequestDatetimeNow)
            {
                if (preivousRequest != null)
                {

                    //if (item.Id == entity.Id)
                    //{
                    //    Location locationFrom = _locationRepository.GetById(item.LocationFrom);
                    //    Location locationTo = _locationRepository.GetById(item.LocationTo);
                    //    DateTime datetimeRank;
                    //    var origin = new GoogleMapPoint { Lat = preivousRequest.LocationToObj.Lat.GetValueOrDefault(), Lng = preivousRequest.LocationToObj.Lng.GetValueOrDefault() };
                    //    var destination = new GoogleMapPoint { Lat = locationFrom.Lat.GetValueOrDefault(), Lng = locationFrom.Lng.GetValueOrDefault() };
                      
                    //    var dataGoogle = _googleService.GetDistance(origin, destination);
                    //    if (dataGoogle.status == "OK")
                    //    {
                    //        if (dataGoogle.rows[0].elements[0].status == "OK")
                    //        {
                    //            item.DistanceEndFrom = dataGoogle.rows[0].elements[0].distance.value;
                    //            item.TimeEndFrom = dataGoogle.rows[0].elements[0].duration.value;
                    //            datetimeRank = preivousRequest.EndTime.AddMinutes((int)((item.TimeEndFrom.GetValueOrDefault() + item.EstimateTime.GetValueOrDefault()) / 60));
                    //            item.IsWarning = datetimeRank > item.EndTime; //|| datetimeRank < item.StartTime;                                
                    //        }
                    //    }
                    //}
                    //else if (preivousRequest.Id == entity.Id)
                    //{
                    //    Location locationTo = _locationRepository.GetById(item.LocationTo);
                    //    DateTime datetimeRank;
                    //    var origin = new GoogleMapPoint { Lat = locationTo.Lat.GetValueOrDefault(), Lng = locationTo.Lng.GetValueOrDefault() };
                    //    var destination = new GoogleMapPoint { Lat = item.LocationFromObj.Lat.GetValueOrDefault(), Lng = item.LocationFromObj.Lng.GetValueOrDefault() };
                    //    var dataGoogle = _googleService.GetDistance(origin, destination);
                    //    if (dataGoogle.status == "OK")
                    //    {
                    //        item.DistanceEndFrom = dataGoogle.rows[0].elements[0].distance.value;
                    //        item.TimeEndFrom = dataGoogle.rows[0].elements[0].duration.value;
                    //        datetimeRank = preivousRequest.EndTime.AddMinutes((int)((item.TimeEndFrom.GetValueOrDefault() + item.EstimateTime.GetValueOrDefault()) / 60));
                    //        item.IsWarning = datetimeRank > item.EndTime;//|| datetimeRank < item.StartTime;
                    //    }
                    //}

                    Location locationFrom = _locationRepository.GetById(item.LocationFrom);
                    Location locationTo = _locationRepository.GetById(item.LocationTo);
                    DateTime datetimeRank;
                    //get location to privious
                    var previousLocationTo = _locationRepository.GetById(preivousRequest.LocationTo);

                    var origin = new GoogleMapPoint { Lat = previousLocationTo.Lat.GetValueOrDefault(), Lng = previousLocationTo.Lng.GetValueOrDefault() };
                    var destination = new GoogleMapPoint { Lat = locationFrom.Lat.GetValueOrDefault(), Lng = locationFrom.Lng.GetValueOrDefault() };

                    var dataGoogle = _googleService.GetDistance(origin, destination);
                    if (dataGoogle.status == "OK")
                    {
                        if (dataGoogle.rows[0].elements[0].status == "OK")
                        {
                            item.DistanceEndFrom = dataGoogle.rows[0].elements[0].distance.value;
                            item.TimeEndFrom = dataGoogle.rows[0].elements[0].duration.value;
                            datetimeRank = preivousRequest.EndTime.AddMinutes((int)((item.TimeEndFrom.GetValueOrDefault() + item.EstimateTime.GetValueOrDefault()) / 60));
                            item.IsWarning = datetimeRank > item.EndTime; //|| datetimeRank < item.StartTime;                                
                        }
                    }

                }
                else
                {
                    //if (listRequestDatetimeNow.Count == 1)
                    //{
                    //    DateTime datetimeRank;
                    //    datetimeRank = item.SendingTime.GetValueOrDefault().AddMinutes((int)(item.EstimateTime.GetValueOrDefault() / 60));
                    //    item.IsWarning = datetimeRank > item.EndTime; //|| datetimeRank < item.StartTime;
                    //    var locationFrom = _locationRepository.GetById(item.LocationFrom);
                    //    var locationTo = _locationRepository.GetById(item.LocationTo);                      
                    //}
                    item.IsWarning = false;


                }
                preivousRequest = item;
            }

            return listRequestDatetimeNow;
        }
        #endregion

        public void UpdateListRequestForService(IList<Request> listRequest)
        {
            foreach (var req in listRequest)
            {
                Repository.Update(req);
            }
            Repository.Commit();
        }

        public void AddListRequestForService(IList<Request> listRequest)
        {
            var currentDate = DateTime.UtcNow;
            foreach (var req in listRequest)
            {
                var req1 = req;
                if (!_requestRepository.CheckExist(
                        p =>
                            p.CreatedOn.Value.Year == currentDate.Year && p.CreatedOn.Value.Month == currentDate.Month &&
                            p.CreatedOn.Value.Day == currentDate.Day
                            && p.HistoryScheduleId == req1.HistoryScheduleId))
                {
                    req.RequestNo = _staticValueService.GetNewRequestNumber();

                    //Tinh estimate distance, estimate time
                    var estimateDistance = 0;
                    var estimateTime = 0;
                    if (req.LocationFrom > 0 && req.LocationTo > 0)
                    {
                        var fromLocation = _locationRepository.GetById(req.LocationFrom);
                        var fromPoint = new GoogleMapPoint
                        {
                            Lat = fromLocation.Lat.GetValueOrDefault(),
                            Lng = fromLocation.Lng.GetValueOrDefault()
                        };

                        var toLocation = _locationRepository.GetById(req.LocationTo);
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
                    req.EstimateTime = estimateTime;
                    req.EstimateDistance = estimateDistance;
                    Repository.Add(req);
                    var courier = _courierRepository.GetById(req.CourierId.GetValueOrDefault()).User;

                    _systemEventRepository.Add(EventMessage.RequestCreated,
                            new Dictionary<EventMessageParam, string>()
                            {
                                {EventMessageParam.Request, req.RequestNo},
                                {EventMessageParam.Courier, courier.LastName + " " + courier.FirstName + (string.IsNullOrEmpty(courier.MiddleName) ? "" : " " + courier.MiddleName)},
                            });
                }
            }
            Repository.Commit();
        }

        public dynamic GetListHoldingRequest(IQueryInfo queryInfo)
        {
            return _holdingRequestRepository.GetListHoldingRequest(queryInfo);
        }

        public bool ReAssignCourier(int id, int courierId)
        {
            using (var tran = new TransactionScope())
            {
                var result = false;
                var request = _requestRepository.GetById(id);
                if (request != null)
                {
                    CreateRequestHistory(request, HistoryRequestActionType.Reassign, request.Status, "ReAssign request");
                    var courier = _courierRepository.GetById(courierId).User;
                    _systemEventRepository.Add(EventMessage.RequestReassigned, new Dictionary<EventMessageParam, string>()
                    {
                        {EventMessageParam.Request, request.RequestNo},
                        {EventMessageParam.FromCourier, request.Courier.User.FullName},
                        {EventMessageParam.ToCourier, courier.FullName}
                    });

                    request.CourierId = courierId;
                    request.SendingTime = DateTime.UtcNow;
                    request.Status = request.IsStat == true ? (int)StatusRequest.Sent : (int)StatusRequest.NotSent;
                    _requestRepository.Update(request);
                    _requestRepository.Commit();
                    result = true;


                }
                tran.Complete();
                return result;
            }
        }

        public string CancelRequest(int id)
        {
            using (var tran = new TransactionScope())
            {
                var result = "";
                var request = _requestRepository.GetById(id);

                if (request != null)
                {
                    if (request.Status == (int)StatusRequest.Completed)
                    {
                        result = SystemMessageLookup.GetMessage("StatusRequestIsCompleted");
                    }
                    else
                    {
                        CreateRequestHistory(request, HistoryRequestActionType.Cancelled, request.Status, "Cancel request");

                        _systemEventRepository.Add(EventMessage.RequestCancelled,
                            new Dictionary<EventMessageParam, string>()
                            {
                                {EventMessageParam.Request, request.RequestNo},
                                {EventMessageParam.Courier, request.Courier.User.FullName},
                            });
                        if (request.Courier.CurrentReq == id)
                        {
                            request.Courier.CurrentReq = 0;
                        }
                        request.Status = (int)StatusRequest.Cancelled;
                        _requestRepository.Update(request);
                        _requestRepository.Commit();

                    }


                }
                tran.Complete();
                return result;
            }
        }

        public dynamic GetRequestListByCourier(QueryInfo queryInfo)
        {
            return _requestRepository.GetRequestListByCourier(queryInfo);
        }

        public ListRequestDto GetRequestForToday(int userId, int utcClient)
        {
            return new ListRequestDto
            {
                RequestDtos = _requestRepository.GetRequestForToday(userId, utcClient),
                RequestHistoryDtos =
                    _requestHistoryRepository.GetHistoryRequestForToday(userId, utcClient).Select(o => o.MapTo<RequestHistoryDto>()).ToList()
            };
        }

        public void UpdateRequestStatus(UpdateRequestDto requestDto)
        {
            var request = _requestRepository.GetById(requestDto.RequestId);
            if (request != null && request.CourierId == requestDto.CourierId &&
                request.Status != (int)StatusRequest.Cancelled &&
                !(request.Status == (int)StatusRequest.Abandoned &&
                  requestDto.RequestType == (int)StatusRequest.Waiting))
            {
                request.Status = requestDto.RequestType;
                if (requestDto.PriorityNumber != 0)
                {
                    request.PriorityNumber = requestDto.PriorityNumber;
                }
                _requestRepository.Update(request);
                _requestRepository.Commit();

            }
        }

        public void UpdateListRequestStatus(RequestStatusDto requestStatusDto)
        {
            foreach (var requestDto in requestStatusDto.UpdateRequestDtos)
            {
                var request = _requestRepository.GetById(requestDto.RequestId);
                if (request != null)
                {
                    //if courier Declined or Completed request
                    if (requestDto.RequestType == (int)StatusRequest.Declined ||
                        requestDto.RequestType == (int)StatusRequest.Completed)
                    {
                        var requestHistory = new RequestHistory
                        {
                            RequestId = requestDto.RequestId,
                            CourierId = requestDto.CourierId,
                            LastRequestStatus = request.Status,
                            ActionType = requestDto.RequestType == (int)StatusRequest.Declined
                                ? (int)HistoryRequestActionType.Declined
                                : (int)HistoryRequestActionType.Completed,
                            Comment = requestDto.RequestType == (int)StatusRequest.Declined
                                ? "Declined request"
                                : "Completed request",
                            TimeChanged = DateTime.UtcNow
                        };
                        _requestHistoryRepository.Add(requestHistory);

                        switch (requestDto.RequestType)
                        {
                            case (int)StatusRequest.Declined:
                                _systemEventRepository.Add(EventMessage.CourierDeclinedRequest,
                                    new Dictionary<EventMessageParam, string>
                                    {
                                        {EventMessageParam.Courier, request.Courier.User.FullName},
                                        {EventMessageParam.Request, request.RequestNo}
                                    });
                                break;
                            case (int)StatusRequest.Completed:
                                _systemEventRepository.Add(EventMessage.CourierCompletedRequest,
                                    new Dictionary<EventMessageParam, string>
                                    {
                                        {EventMessageParam.Courier, request.Courier.User.FullName},
                                        {EventMessageParam.Request, request.RequestNo}
                                    });
                                break;
                        }
                    }
                    //if request still belong courier
                    if (request.CourierId == requestDto.CourierId &&
                        request.Status != (int)StatusRequest.Cancelled &&
                        !(request.Status == (int)StatusRequest.Abandoned &&
                          requestDto.RequestType == (int)StatusRequest.Waiting))
                    {
                        request.ReceivedTime = requestDto.ReceivedTime;
                        request.AcceptedTime = requestDto.AcceptedTime;
                        request.RejectedTime = requestDto.RejectedTime;
                        request.ActualStartTime = requestDto.ActualStartTime;
                        request.ActualEndTime = requestDto.ActualEndTime;
                        request.IsAgreed = requestDto.IsAgreed;
                        request.CompleteNote = requestDto.CompleteNote;
                        switch (requestDto.RequestType)
                        {
                            case (int)StatusRequest.Received:
                                _systemEventRepository.Add(EventMessage.CourierAcceptedRequest,
                                    new Dictionary<EventMessageParam, string>
                                    {
                                        {EventMessageParam.Courier, request.Courier.User.FullName},
                                        {EventMessageParam.Request, request.RequestNo}
                                    });
                                break;
                            case (int)StatusRequest.Started:
                                _systemEventRepository.Add(EventMessage.CourierStartedRequest,
                                    new Dictionary<EventMessageParam, string>
                                    {
                                        {EventMessageParam.Courier, request.Courier.User.FullName},
                                        {EventMessageParam.Request, request.RequestNo}
                                    });
                                break;
                        }

                        request.Status = requestDto.RequestType;
                        if (requestDto.PriorityNumber != 0)
                        {
                            request.PriorityNumber = requestDto.PriorityNumber;
                        }
                        if (requestDto.Signature != null)
                        {
                            request.Signature = Convert.FromBase64String(requestDto.Signature.Replace("\\", ""));
                        }
                        if (requestDto.CompletePicture != null)
                        {
                            request.CompletePicture = Convert.FromBase64String(requestDto.CompletePicture.Replace("\\", ""));
                        }
                        if (requestDto.NoteDto != null)
                        {
                            foreach (var noteDto in requestDto.NoteDto)
                            {
                                request.NoteRequests.Add(new NoteRequest
                                {
                                    Description = noteDto.Description,
                                    CreateTime = noteDto.Date,
                                    CourierId = request.CourierId
                                });

                            }
                        }

                        _requestRepository.Update(request);
                    }

                }
            }
            _requestRepository.Commit();

        }

        public dynamic GetPieChartData(int? courierId)
        {
            return _requestRepository.GetPieChartData(courierId);
        }

        public dynamic GetCurrentDataRequests(IQueryInfo queryInfo)
        {
            return _requestRepository.GetCurrentDataRequests(queryInfo);
        }

        public void AddNoteRequest(UpdateSingleNoteRequestDto requestDto)
        {
            var request = GetById(requestDto.RequestId);
            request.NoteRequests.Add(new NoteRequest
            {
                CourierId = requestDto.CourierId,
                Description = requestDto.NoteDto.Description,
                CreateTime = requestDto.NoteDto.Date
            });
            _requestRepository.Update(request);
            _requestRepository.Commit();

        }

        private void CreateRequestHistory(Request request, HistoryRequestActionType actionType, int lastRequestStatus, string comment = "")
        {
            _requestHistoryRepository.Add(new RequestHistory()
            {
                ActionType = (int)actionType,
                Comment = comment,
                CourierId = request.CourierId.GetValueOrDefault(),
                LastRequestStatus = lastRequestStatus,
                TimeChanged = DateTime.UtcNow,
                RequestId = request.Id
            });
            _requestHistoryRepository.Commit();
        }

        public dynamic GetListRequestForReport(int courierId, DateTime fromDate, DateTime toDate)
        {
            //var fromdt = (fromDate ?? DateTime.MinValue).ToUtcTimeFromClientTime();
            //var todt = toDate ?? DateTimeHelper.SetDateToCurrentDate(DateTime.Now);
            //var query = Get(
            //    p => p.CourierId == courierId && 
            //        p.Status != (int)StatusRequest.Abandoned
            //    && p.Status != (int)StatusRequest.Declined
            //    && p.Status != (int)StatusRequest.Cancelled
            //    && p.CreatedOn != null
            //    && p.CreatedOn.Value >= fromdt
            //    && p.CreatedOn.Value <= todt)
            //    .OrderBy(p => p.CreatedOn)
            //    .Select(p => new RequestReportVo()
            //    {
            //        Id = p.Id,
            //        RequestNo = p.RequestNo,
            //        Email = p.Courier.User.Email,
            //        FirstName = p.Courier.User.FirstName,
            //        MiddleName = p.Courier.User.MiddleName,
            //        LastName = p.Courier.User.LastName,
            //        HomePhone = p.Courier.User.HomePhone.ApplyFormatPhone(),
            //        MobilePhone = p.Courier.User.MobilePhone.ApplyFormatPhone(),
            //        RequestDate = p.CreatedOn,
            //        LocationFrom = p.LocationFromObj.Name,
            //        LocationTo = p.LocationToObj.Name,
            //        ActualDistance = p.Trackings.Sum(x => x.Distance).MetersToMiles(2)
            //    });

             var list =  _requestRepository.GetListRequestForReport(courierId, fromDate, toDate);
             return new { Data = list, TotalRowCount = list.Count() };
        }

        public dynamic GetNotesDetail(int requestId)
        {
            var data = new List<NoteRequestDetail>();
            var totalRowCount = 0;
            var request = _requestRepository.GetRequestWithCourier(requestId);
            if (request != null && request.Courier != null && request.Courier.User != null)
            {
                data = _noteRequestRepository.GetNotesDetail(requestId);
                data.ForEach(o =>
                {
                    o.Tag = "Mobile";
                    if (!string.IsNullOrEmpty(o.Content))
                    {
                        o.Content = WebUtility.HtmlEncode(o.Content);
                    }
                });

                var courierName = request.Courier.User.FullName;
                var userName = "";
                var isSchedule = false;
                if (request.CreatedBy != null)
                {
                    userName = request.CreatedBy.FullName;
                }
                else
                {
                    userName = _scheduleRepository.Get(o => o.Id == request.HistoryScheduleId).Select(o => o.Name).FirstOrDefault();
                    isSchedule = true;
                }

                //Sent
                if (request.SendingTime != null)
                {
                    data.Add(new NoteRequestDetail
                    {
                        CreateTime = request.SendingTime,
                        Title = "Sent",
                        Content = string.Empty,
                        CourierName = userName,
                        Tag = "System",
                        IsSchedule = isSchedule,
                    });
                }

                //Received
                if (request.ReceivedTime != null)
                {
                    data.Add(new NoteRequestDetail
                    {
                        CreateTime = request.ReceivedTime,
                        Title = "Received",
                        Content = string.Empty,
                        CourierName = courierName,
                        Tag = "Mobile",
                    });
                }

                //Accepted
                if (request.AcceptedTime != null)
                {
                    data.Add(new NoteRequestDetail
                    {
                        CreateTime = request.AcceptedTime,
                        Title = "Accepted",
                        Content = string.Empty,
                        CourierName = courierName,
                        Tag = "Mobile",
                    });
                }

                //Rejected
                if (request.RejectedTime != null)
                {
                    data.Add(new NoteRequestDetail
                    {
                        CreateTime = request.RejectedTime,
                        Title = "Rejected",
                        Content = string.Empty,
                        CourierName = courierName,
                        Tag = "Mobile",
                    });
                }

                //Started
                if (request.ActualStartTime != null)
                {
                    data.Add(new NoteRequestDetail
                    {
                        CreateTime = request.ActualStartTime,
                        Title = "Started",
                        Content = string.Empty,
                        CourierName = courierName,
                        Tag = "Mobile",
                    });
                }

                //Complete
                if (request.ActualEndTime != null)
                {
                    data.Add(new NoteRequestDetail
                    {
                        CreateTime = request.ActualEndTime,
                        Title = "Completed",
                        Content = string.Empty,
                        CourierName = courierName,
                        Tag = "Mobile",
                    });
                }
                data = data.OrderBy(o => o.CreateTime).ToList();
                totalRowCount = data.Count;
            }
            return new { Data = data, TotalRowCount = totalRowCount };
        }

        public dynamic GetRequestForTracking(int? requestId = null, string requestNo = null)
        {
            RequestGridVo obj = _requestRepository.GetRequestForTracking(requestId, requestNo);
            return obj;
        }

        public dynamic GetPictureAndNoteRequestComplete(int requestId)
        {
            PictureAndNoteVo obj = _requestRepository.GetPictureAndNoteRequestComplete(requestId);
            if (obj != null && !string.IsNullOrEmpty(obj.Note))
            {
                obj.Note = WebUtility.HtmlEncode(obj.Note);
            }
            return obj;
        }

        public dynamic GetListTrackingDataFromTo(int fromId, int toId)
        {
            var dataDirection = new List<GoogleMapApiHelper.LocationVo>();
            var dataDirectionPoints = new List<GoogleMapApiHelper.LocationVo>();
            var fromLocation = _locationRepository.GetLatLng(fromId);
            var toLocation = _locationRepository.GetLatLng(toId);

            if (fromLocation != null && toLocation != null)
            {

                var dataFromGoogle =
                    _googleService.GetDirection(
                        new GoogleMapPoint(fromLocation.Lat.GetValueOrDefault(), fromLocation.Lng.GetValueOrDefault()),
                        new GoogleMapPoint(toLocation.Lat.GetValueOrDefault(), toLocation.Lng.GetValueOrDefault()));
                if (dataFromGoogle.Status == GoogleMapStatus.OK.ToString())
                {
                    dataDirection = dataFromGoogle.routes[0].overview_polyline.Points.DecodePolylinePoints();
                }
                else
                {
                    dataDirectionPoints.Add(
                        new GoogleMapApiHelper.LocationVo { Latitude = fromLocation.Lat.GetValueOrDefault(), Longitude = fromLocation.Lng.GetValueOrDefault() });
                    dataDirectionPoints.Add(
                        new GoogleMapApiHelper.LocationVo { Latitude = toLocation.Lat.GetValueOrDefault(), Longitude = toLocation.Lng.GetValueOrDefault() });
                }
                return new
                {
                    FromLocation = new
                    {
                        Lat = fromLocation.Lat.GetValueOrDefault(),
                        Lng = fromLocation.Lng.GetValueOrDefault()
                    },
                    ToLocation = new
                    {
                        Lat = toLocation.Lat.GetValueOrDefault(),
                        Lng = toLocation.Lng.GetValueOrDefault()
                    },
                    Direction = dataDirection,
                    DataDirectionPoints = dataDirectionPoints
                };
            }
            if (fromLocation != null)
            {
                return new
                {
                    FromLocation = new
                    {
                        Lat = fromLocation.Lat.GetValueOrDefault(),
                        Lng = fromLocation.Lng.GetValueOrDefault()
                    }
                };
            }
            if (toLocation != null)
            {
                return new
                {
                    ToLocation = new
                    {
                        Lat = toLocation.Lat.GetValueOrDefault(),
                        Lng = toLocation.Lng.GetValueOrDefault()
                    }
                };
            }
            return null;
        }

        public dynamic GetRequestCourierForCreate(int courierId)
        {
            var data = _requestRepository.GetRequestCourierForCreate(courierId);
            return new { Data = data, TotalRowCount = data.Count };
        }

        public dynamic GetLatLngForLocation(int locationId)
        {
            LatLngVo data = _requestRepository.GetLatLngForLocation(locationId);
            return data;
        }
    }
}