using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Common;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Framework.Mapping;
using Framework.Utility;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;

namespace ServiceLayer
{
    public class TrackingService : MasterFileService<Tracking>, ITrackingService
    {
        private readonly ITrackingRepository _trackingRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly ICourierRepository _courierRepository;
        private readonly IGoogleService _googleService;

        public TrackingService(ITenantPersistenceService tenantPersistenceService,
            ITrackingRepository trackingRepository, IRequestRepository requestRepository, ICourierRepository courierRepository, IGoogleService googleService,
            IBusinessRuleSet<Tracking> businessRuleSet = null)
            : base(trackingRepository, trackingRepository, tenantPersistenceService, businessRuleSet)
        {
            _trackingRepository = trackingRepository;
            _requestRepository = requestRepository;
            _courierRepository = courierRepository;
            _googleService = googleService;
        }

        public dynamic GetListTrackingData(int courierId, DateTime filterDateTime, int? requestId)
        {
            var fromdt = (new DateTime(filterDateTime.Year, filterDateTime.Month, filterDateTime.Day, 0, 0, 0)).ToUtcTimeFromClientTime();
            var todt = (new DateTime(filterDateTime.Year, filterDateTime.Month, filterDateTime.Day, 23, 59, 59)).ToUtcTimeFromClientTime();
            if (filterDateTime > DateTime.MinValue)
            {
                fromdt = DateTimeHelper.GetStartDateTime(filterDateTime);
                todt = DateTimeHelper.GetEndDateTime(filterDateTime);
            }
            

            var query = _trackingRepository.Get(p=> p.Request != null
                && p.TimeTracking != null
                && p.TimeTracking.Value >= fromdt
                && p.TimeTracking.Value <= todt)
                .OrderBy(p=>p.TimeTracking)
                .Select(p => new TrackingVo()
                {
                    RequestId = p.Request.Id,
                    IsActiveRequest = (p.Courier.CurrentReq == p.Request.Id && p.Request.Status == (int)StatusRequest.Started),
                    RequestNo = p.Request.RequestNo,
                    Address = p.Address,
                    Direction = p.Direction.GetValueOrDefault(0),
                    Distance = p.Distance,
                    Velocity = p.Velocity ?? 0,
                    TimeTracking = p.TimeTracking.GetValueOrDefault(DateTime.UtcNow).ToClientTime("hh:mm tt"),
                    IsFinish = (p.Request.Status == (int)StatusRequest.Completed || p.Request.Status == (int)StatusRequest.Cancelled),
                    CourierId = p.Request.CourierId,
                    //Implement show courier name
                    FirstName = p.Request.CourierId != null ? p.Request.Courier.User.FirstName: "",
                    MiddleName = p.Request.CourierId != null ? p.Request.Courier.User.MiddleName : "",
                    LastName = p.Request.CourierId != null ? p.Request.Courier.User.LastName : "",
                });
            
            if (courierId != 0)
            {
                query = query.Where(p => p.CourierId == courierId);
            }

            if (requestId != null)
            {
                query = query.Where(p => p.RequestId == requestId);
            }

            var dataDirection = new List<GoogleMapApiHelper.LocationVo>();
            var dataDirectionPoints = new List<GoogleMapApiHelper.LocationVo>();
            if (requestId != null)
            {
                var req = _requestRepository.GetById(requestId.GetValueOrDefault());
                var dataFromGoogle = req != null ?_googleService.GetDirection(new GoogleMapPoint(req.LocationFromObj.Lat.GetValueOrDefault(), req.LocationFromObj.Lng.GetValueOrDefault()),
                    new GoogleMapPoint(req.LocationToObj.Lat.GetValueOrDefault(), req.LocationToObj.Lng.GetValueOrDefault())) : new GoogleMapResultObject() { Status = GoogleMapStatus.ZERO_RESULTS.ToString() };
                if (dataFromGoogle.Status == GoogleMapStatus.OK.ToString())
                {
                    dataDirection = dataFromGoogle.routes[0].overview_polyline.Points.DecodePolylinePoints(); 
                }
                else
                {
                    dataDirectionPoints.Add(
                        new GoogleMapApiHelper.LocationVo() { Latitude = req.LocationFromObj.Lat.GetValueOrDefault(), Longitude = req.LocationFromObj.Lng.GetValueOrDefault() });
                    dataDirectionPoints.Add(
                        new GoogleMapApiHelper.LocationVo() { Latitude = req.LocationToObj.Lat.GetValueOrDefault(), Longitude = req.LocationToObj.Lng.GetValueOrDefault() });
                }
            }
            

            var data = query.ToList();
            return new { Data = data.ToList(), Direction = dataDirection, TotalRowCount = data.Count, DataDirectionPoints = dataDirectionPoints };
        }

        public void UpdateTrackingHistory(TrackingRequestDto trackingRequestDto)
        {
            foreach (var trackingDto in trackingRequestDto.TrackingDtos)
            {
                var tracking = trackingDto.MapTo<Tracking>();
                tracking.Address = tracking.Address.Replace("92", "\\");
                bool hasInsert = false;
                if (trackingDto.RequestIds != null && trackingDto.RequestIds.Count > 0)
                {
                    foreach (var requestId in trackingDto.RequestIds)
                    {

                        var objRequest = _requestRepository.GetById(requestId);
                        if (objRequest != null)
                        {
                            var trackingClone = tracking.Clone() as Tracking;
                            objRequest.Trackings.Add(trackingClone);
                            hasInsert = true;
                        }
                    }
                }
                if (!hasInsert)
                {
                    _trackingRepository.Add(tracking);
                }
            }
            _requestRepository.Commit();
            _trackingRepository.Commit();
        }

     
    }

}