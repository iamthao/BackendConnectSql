using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Text;
using Database.Persistance.Tenants;
using Framework.DomainModel.Common;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;
using Framework.Utility;
using Repositories.Interfaces;

namespace Repositories
{
    public class EntityFrameworkRequestRepository : EntityFrameworkTenantRepositoryBase<Request>, IRequestRepository
    {

        public EntityFrameworkRequestRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {
            SearchColumns.Add("RequestNo");
            SearchColumns.Add("CourierSearch");
            SearchColumns.Add("LocationFromName");
            SearchColumns.Add("LocationToName");
        }


        public override IQueryable<ReadOnlyGridVo> BuildQueryToGetDataForGrid(IQueryInfo queryInfo)
        {
            var requestQueryInfo = (RequestQueryInfo)queryInfo;

            var startDate = requestQueryInfo.StartDate.ToUniversalTime();
            var endDate = requestQueryInfo.EndDate.ToUniversalTime();
            Expression<Func<Request, bool>> expression = s => s.CreatedOn >= startDate && s.CreatedOn <= endDate;

            var finalResult = GetAll().Where(expression).Select(request => new RequestGridVo
            {
                Id = request.Id,
                RequestNo = request.RequestNo,
                CourierId = request.CourierId,
                FirstNameCourier = request.Courier != null && request.Courier.User != null ? request.Courier.User.FirstName : "",
                MiddleNameCourier = request.Courier != null && request.Courier.User != null ? request.Courier.User.MiddleName : "",
                LastNameCourier = request.Courier != null && request.Courier.User != null ? request.Courier.User.LastName : "",
                CourierSearch = request.Courier != null && request.Courier.User != null ? (request.Courier.User.FirstName + " " + request.Courier.User.LastName + (!string.IsNullOrEmpty(request.Courier.User.MiddleName) ? " " + request.Courier.User.MiddleName : "")) : "",
                LocationFromId = request.LocationFrom,
                LocationToId = request.LocationTo,
                LocationFromName = request.LocationFromObj != null ? request.LocationFromObj.Name : "",
                LocationToName = request.LocationToObj != null ? request.LocationToObj.Name : "",
                Type = request.IsStat == true ? "Priority Job" : "Normal",
                StatusId = request.Status,
                TimeNoFormat = request.SendingTime,
                SendingTime = request.SendingTime,
                StartTime = request.StartTime,
                StartTimeNoFormat = request.StartTime,
                EndTime = request.EndTime,
                EndTimeNoFormat = request.EndTime,
                Note = request.Description.Length > 200 ? request.Description.Substring(0, 200) + "..." : request.Description,
                CreatedDateNoFormat = request.CreatedOn,
                FirstNameCreatedBy = request.CreatedBy != null ? request.CreatedBy.FirstName : "",
                MiddleNameCreatedBy = request.CreatedBy != null ? request.CreatedBy.MiddleName : "",
                LastNameCreatedBy = request.CreatedBy != null ? request.CreatedBy.LastName : "",
                IsExpired = (request.IsExpired ?? false),
                IsSchedule = request.HistoryScheduleId != null,
                IsAgreed = (request.IsAgreed ?? false),
                IsWarning = (request.IsWarning ?? false),
                CreatedOn = request.CreatedOn
            }).OrderBy(requestQueryInfo.SortString);//.OrderByDescending(o=>o.CreatedOn);

            return finalResult;
        }

        protected override void BuildSortExpression(IQueryInfo queryInfo)
        {
            if (queryInfo.Sort == null || queryInfo.Sort.Count == 0)
            {
                queryInfo.Sort = new List<Sort> { new Sort { Field = "EndTime", Dir = "desc" } };
            }
            queryInfo.Sort.ForEach(x =>
            {
                if (x.Field == "Courier")
                {
                    x.Field = "LastNameCourier";
                }
                else if (x.Field == "LocationFromName")
                {
                    x.Field = "LocationFromName";
                }
                else if (x.Field == "LocationToName")
                {
                    x.Field = "LocationToName";
                }
                else if (x.Field == "Time")
                {
                    x.Field = "TimeNoFormat";
                }
                else if (x.Field == "Status")
                {
                    x.Field = "StatusId";
                }
                else if (x.Field == "Type")
                {
                    x.Field = "Type";
                }
                else if (x.Field == "Note")
                {
                    x.Field = "Note";
                }
                else if (x.Field == "CreatedDate")
                {
                    x.Field = "CreatedDateNoFormat";
                }
                else if (x.Field == "CreatedBy")
                {
                    x.Field = "LastNameCreatedBy";
                }
                else
                {
                    x.Field = String.Format("{0}", x.Field);
                }
            });
        }

        public dynamic GetRequestListByCourier(QueryInfo queryInfo)
        {
            if (queryInfo.Sort == null || queryInfo.Sort.Count == 0)
            {
                queryInfo.Sort = new List<Sort> { new Sort { Field = "EndTime", Dir = "asc" } };
            }
            BuildSortExpression(queryInfo);
            //Thao : Ngay bat dau vs ket thuc 1 ngay cua client
            var startnow = DateTimeHelper.GetStartDateTime(DateTime.UtcNow);
            var endnow = DateTimeHelper.GetEndDateTime(DateTime.UtcNow);

            // build querry to get data from db           
            var query = GetAll().Where(o => o.SendingTime >= startnow && o.SendingTime <= endnow).Select(o => new RequestGridVo()
            {
                Id = o.Id,
                RequestNo = o.RequestNo,
                CourierId = o.CourierId,
                LocationFromId = o.LocationFrom,
                LocationToId = o.LocationTo,

                //From
                LocationFromName = o.LocationFromObj.Name,
                //Address1From = o.LocationFromObj.Address1,
                //Address2From = o.LocationFromObj.Address2,
                //CityFrom = o.LocationFromObj.City,
                //StateFrom = o.LocationFromObj.StateOrProvinceOrRegion,
                //ZipFrom = o.LocationFromObj.Zip,
                // To
                LocationToName = o.LocationToObj.Name,
                //Address1To = o.LocationToObj.Address1,
                //Address2To = o.LocationToObj.Address2,
                //CityTo = o.LocationToObj.City,
                //StateTo = o.LocationToObj.StateOrProvinceOrRegion,
                //ZipTo = o.LocationToObj.Zip,
                //Request Status
                Note = o.Description,
                StatusId = o.Status,
                TimeNoFormat = o.SendingTime,
                StartTime = o.StartTime,
                EndTime = o.EndTime,
                IsActiveRequest = (o.Courier.CurrentReq == o.Id && o.Status == (int)StatusRequest.Started),
                IsExpired = (o.IsExpired ?? false),
                IsSchedule = o.HistoryScheduleId != null,
                IsWarning = o.IsWarning == true,

            }).OrderBy(queryInfo.SortString);

            if (queryInfo.QueryId != 0)
            {
                query = query.Where(o => o.CourierId == queryInfo.QueryId);
                var data = query.ToList();

                queryInfo.TotalRecords = query.Count();

                return new { Data = data, TotalRowCount = queryInfo.TotalRecords };
            }
            else
            {
                var data = query.ToList();

                queryInfo.TotalRecords = query.Count();

                return new { Data = data, TotalRowCount = queryInfo.TotalRecords };
            }
        }

        public List<RequestCourierGridVo> GetRequestCourierForCreate(int courierId)
        {
            //Thao : Ngay bat dau vs ket thuc 1 ngay cua client
            var startnow = DateTimeHelper.GetStartDateTime(DateTime.UtcNow); 
            var endnow = DateTimeHelper.GetEndDateTime(DateTime.UtcNow);
            // build querry to get data from db           
            var query = from entity in GetAll().Where(o => o.SendingTime >= startnow && o.SendingTime <= endnow && o.Status != 30 && o.Status != 20)
                        join locationFrom in PersistenceService.CurrentWorkspace.Context.Locations on entity.LocationFrom equals locationFrom.Id
                        join locationTo in PersistenceService.CurrentWorkspace.Context.Locations on entity.LocationTo equals locationTo.Id
                        join user in PersistenceService.CurrentWorkspace.Context.Users.Where(o => o.Id == courierId) on entity.CourierId equals user.Id
                        orderby entity.StartTime ascending
                        select new RequestCourierGridVo
                        {
                            Id = entity.Id,
                            RequestNo = entity.RequestNo,
                            CourierId = entity.CourierId,
                            FirstName = user.FirstName,
                            MiddleName = user.MiddleName,
                            LastName = user.LastName,
                            SendingTime = entity.SendingTime,
                            StartTime = entity.StartTime,
                            EndTime = entity.EndTime,
                            StatusId = entity.Status,
                            LocationFromId = entity.LocationFrom,
                            LocationFromName = locationFrom.Name,
                            LocationFromLat = locationFrom.Lat,
                            LocationFromLng = locationFrom.Lng,
                            LocationToId = entity.LocationTo,
                            LocationToName = locationTo.Name,
                            LocationToLat = locationTo.Lat,
                            LocationToLng = locationTo.Lng,
                            IsWarning = entity.IsWarning == true,
                        };
            return query.ToList();
        }

        public LatLngVo GetLatLngForLocation(int locationId)
        {
            return
                PersistenceService.CurrentWorkspace.Context.Locations.Where(o => o.Id == locationId)
                    .Select(o => new LatLngVo
                    {
                        Lat = o.Lat,
                        Lng = o.Lng,
                    }).FirstOrDefault();
        }

        public List<RequestDto> GetRequestForToday(int userId, int utcClient)
        {
            var currentDateParse = DateTime.UtcNow;
            var clientDate = DateTime.UtcNow.AddMilliseconds(utcClient);
            var currentPrevious = currentDateParse.AddMilliseconds(-1 *
                                                 clientDate.Subtract(new DateTime(clientDate.Year, clientDate.Month, clientDate.Day)).TotalMilliseconds);

            //var currentPrevious = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).AddSeconds(-1*utcClient);
            var finalResult =
                GetAll()
                    .OrderByDescending(o => o.Id)
                    .Where(o => o.SendingTime != null && o.SendingTime <= currentDateParse && o.SendingTime >= currentPrevious &&
                            o.Courier.User.Id == userId && o.Status != (int)StatusRequest.Declined && o.Status != (int)StatusRequest.Completed && o.Status != (int)StatusRequest.Cancelled && (o.IsExpired == null || o.IsExpired == false))
                    .Select(o => new RequestDto
                    {
                        RequestNumber = o.RequestNo,
                        Id = o.Id,
                        Description = o.Description,
                        StartTime = o.StartTime,
                        EndTime = o.EndTime,
                        ReceivedTime = o.ReceivedTime,
                        AcceptedTime = o.AcceptedTime,
                        RejectedTime = o.RejectedTime,
                        ActualStartTime = o.ActualStartTime,
                        ActualEndTime = o.ActualEndTime,
                        PriorityNumber = o.PriorityNumber,
                        IsScheduleCreated = o.HistoryScheduleId != null,
                        Locationfrom = new LocationDto
                        {
                            Id = o.LocationFromObj.Id,
                            Name = o.LocationFromObj.Name,
                            Address1 = o.LocationFromObj.Address1,
                            Address2 = o.LocationFromObj.Address2,
                            City = o.LocationFromObj.City,
                            State = o.LocationFromObj.StateOrProvinceOrRegion,
                            Zip = o.LocationFromObj.Zip,
                            Lat = o.LocationFromObj.Lat == null ? 0 : o.LocationFromObj.Lat.Value,
                            Lng = o.LocationFromObj.Lng == null ? 0 : o.LocationFromObj.Lng.Value,
                        },
                        Locationto = new LocationDto
                        {
                            Id = o.LocationToObj.Id,
                            Name = o.LocationToObj.Name,
                            Address1 = o.LocationToObj.Address1,
                            Address2 = o.LocationToObj.Address2,
                            City = o.LocationToObj.City,
                            State = o.LocationToObj.StateOrProvinceOrRegion,
                            Zip = o.LocationToObj.Zip,
                            Lat = o.LocationToObj.Lat == null ? 0 : o.LocationToObj.Lat.Value,
                            Lng = o.LocationToObj.Lng == null ? 0 : o.LocationToObj.Lng.Value,
                        },
                        RequestType = o.Status,
                        IsStat = o.IsStat != null && o.IsStat.Value,
                        NoteRequests = o.NoteRequests.Select(p => new NoteDto
                        {
                            Description = p.Description,
                            Date = p.CreateTime
                        }).OrderByDescending(s => s.Date).ToList()
                    });

            return finalResult.ToList();
        }

        public dynamic GetPieChartData(int? courierId)
        {
            //courierId = courierId == 0 ? null : courierId;
            var startnow = DateTimeHelper.GetStartDateTime(DateTime.UtcNow);
            var endnow = DateTimeHelper.GetEndDateTime(DateTime.UtcNow);

            var query =
                from o in Get(p => (courierId == null || p.CourierId == courierId) && p.SendingTime >= startnow && p.SendingTime <= endnow)
                group o by o.Status
                    into g
                    select new DashboardRequestChartVo()
                    {
                        Id = g.Key,
                        Value = g.Count()
                    };

            return new { Data = query };
        }

        public dynamic GetCurrentDataRequests(IQueryInfo queryInfo)
        {
            var dashboardRequestQueryInfo = queryInfo as DashboardRequestQueryInfo;
            //Thao : Ngay bat dau vs ket thuc 1 ngay cua client
            var startNowTime = DateTimeHelper.GetStartDateTime(DateTime.UtcNow);
            var endNowTime = DateTimeHelper.GetEndDateTime(DateTime.UtcNow);

            var finalResult = Get(
                t => t.SendingTime >= startNowTime && t.SendingTime <= endNowTime)
                .Select(request => new RequestGridVo
                {
                    Id = request.Id,
                    RequestNo = request.RequestNo,
                    CourierId = request.CourierId,
                    CourierSearch = request.Courier != null && request.Courier.User != null ? (request.Courier.User.FirstName + " " + request.Courier.User.LastName + (!string.IsNullOrEmpty(request.Courier.User.MiddleName) ? " " + request.Courier.User.MiddleName : "")) : "",
                    LocationFromName = request.LocationFromObj != null ? request.LocationFromObj.Name : "",
                    LocationToName = request.LocationToObj != null ? request.LocationToObj.Name : "",
                    TimeNoFormat = request.SendingTime,
                    StatusId = request.Status,
                    IsAgreed = request.IsAgreed ?? false,
                    IsSchedule = request.HistoryScheduleId != null,
                    IsWarning = request.IsWarning == true,
                    StartTime = request.StartTime,
                    EndTime = request.EndTime,
                });
            if (dashboardRequestQueryInfo != null)
            {
                if (dashboardRequestQueryInfo.Sort == null || dashboardRequestQueryInfo.Sort.Count == 0)
                {
                    queryInfo.Sort = new List<Sort> { new Sort { Field = "EndTime", Dir = "asc" } };
                }
                queryInfo.Sort.ForEach(x =>
                {
                    x.Field = String.Format("{0}", x.Field);
                });

                if (!Get(p => p.Status == dashboardRequestQueryInfo.StatusId && p.SendingTime >= startNowTime && p.SendingTime <= endNowTime).Any())
                //&& p.SendingTime.Value.Day == currentDate.Day &&
                //p.SendingTime.Value.Month == currentDate.Month &&
                //p.SendingTime.Value.Year == currentDate.Year).Any())
                {
                    var temp = Get(p => p.SendingTime != null && p.SendingTime >= startNowTime && p.SendingTime <= endNowTime)
                    ;
                    //p.SendingTime.Value.Day == currentDate.Day &&
                    //p.SendingTime.Value.Month == currentDate.Month &&
                    //p.SendingTime.Value.Year == currentDate.Year);

                    if (temp.Any())
                    {
                        var query = (temp
                        .GroupBy(o => o.Status)
                        .Select(g => new DashboardRequestChartVo()
                        {
                            Id = g.Key,
                            Value = g.Count()
                        })).OrderByDescending(x => x.Value).ToList().First();

                        dashboardRequestQueryInfo.StatusId = query.Id;
                    }

                    finalResult = finalResult.Where(p => p.StatusId == dashboardRequestQueryInfo.StatusId);
                }
            }

            var data = finalResult.Where(p => (dashboardRequestQueryInfo.CourierId == null || p.CourierId == dashboardRequestQueryInfo.CourierId)
                    && p.StatusId == dashboardRequestQueryInfo.StatusId)
                .OrderBy(queryInfo.SortString)
                .Skip(queryInfo.Skip)
                .Take(queryInfo.Take).ToList();
            queryInfo.TotalRecords = finalResult.Count(p => p.StatusId == dashboardRequestQueryInfo.StatusId);

            return new { Data = data, TotalRowCount = queryInfo.TotalRecords };
        }

        public RequestDeliveryAgreementVo GetListDeliveryAgreementVo(IQueryInfo queryInfo)
        {
            var result = GetById(queryInfo.QueryId);
            var listTracking =
                PersistenceService.CurrentWorkspace.Context.Trackings.Where(o => o.RequestId == queryInfo.QueryId).Select(o => o.Distance).ToList();
            double? requestDistance = 0;

            if (listTracking.Count > 0)
            {
                requestDistance += listTracking.Sum(d => d.GetValueOrDefault());
            }
            return new RequestDeliveryAgreementVo()
            {
                Id = result.Id,
                RequestNo = result.RequestNo,
                Signature = result.Signature != null ? Convert.ToBase64String(result.Signature, 0, result.Signature.Length) : string.Empty,
                RequestAgreed = result.IsAgreed ?? false,
                RequestFrom = result.LocationFromObj.Address1
                    + " " + result.LocationFromObj.Address2
                    + ", " + result.LocationFromObj.City
                    + ", " + result.LocationFromObj.StateOrProvinceOrRegion
                    + ", " + result.LocationFromObj.Zip,
                RequestTo = result.LocationToObj.Address1
                    + " " + result.LocationToObj.Address2
                    + ", " + result.LocationToObj.City
                    + ", " + result.LocationToObj.StateOrProvinceOrRegion
                    + ", " + result.LocationToObj.Zip,
                RequestTimes = result.ActualStartTime == null || result.ActualEndTime == null ? 0 : (result.ActualEndTime - result.ActualStartTime).Value.Minutes,
                RequestDistance = requestDistance.MetersToMiles(2).ToString("N"),
                RequestFromName = result.LocationFromObj.Name,
                RequestToName = result.LocationToObj.Name,
                IsAgreed = result.IsAgreed,
            };
        }

        public List<RequestReportVo> GetListRequestForReport(int courierId, DateTime fromDate, DateTime toDate)
        {
            var fromDateEndDay = fromDate;
            var toDateEndDay = toDate;
            if (fromDate > DateTime.MinValue && toDate > DateTime.MinValue)
            {
                fromDateEndDay = DateTimeHelper.GetStartDateTime(fromDate);
                toDateEndDay = DateTimeHelper.GetEndDateTime(toDate);
            }
            
            var distance = from tracking in TenantPersistenceService.CurrentWorkspace.Context.Trackings
                           group tracking by tracking.RequestId into grp
                           orderby grp.Sum(o => o.Distance)
                           select new { RequestId = grp.Key, Distance = grp.Sum(o => o.Distance) };

            if (courierId == 0)
            {
                var query1 = from entity in TenantPersistenceService.CurrentWorkspace.Context.Requests
                             where entity.SendingTime >= fromDateEndDay &&
                                     entity.SendingTime <= toDateEndDay //&&
                             //entity.Status != (int)StatusRequest.Abandoned &&
                             //entity.Status != (int)StatusRequest.Declined &&
                             //entity.Status != (int)StatusRequest.Cancelled
                             join dis in distance on entity.Id equals dis.RequestId into distanceInto
                             from dis1 in distanceInto.DefaultIfEmpty()
                             select new RequestReportVo
                             {
                                 Id = entity.Id,
                                 RequestNo = entity.RequestNo,
                                 Email = entity.Courier.User.Email,
                                 FirstName = entity.Courier.User.FirstName,
                                 MiddleName = entity.Courier.User.MiddleName,
                                 LastName = entity.Courier.User.LastName,
                                 HomePhone = entity.Courier.User.HomePhone,
                                 MobilePhone = entity.Courier.User.MobilePhone,
                                 RequestDate = entity.SendingTime,
                                 LocationFrom = entity.LocationFromObj.Name,
                                 LocationTo = entity.LocationToObj.Name,
                                 StartTime = entity.StartTime,
                                 EndTime = entity.EndTime,
                                 ActualStartTime = entity.ActualStartTime,
                                 ActualEndTime = entity.ActualEndTime,
                                 EstimateDistance = entity.EstimateDistance,
                                 //EstimateTime = entity.EstimateTime,
                                 ActualDistance = dis1.Distance

                             };

                return query1.ToList();
                //return new { Data = query1.ToList(), TotalRowCount = query1.ToList().Count() };
            }

            var query = from entity in TenantPersistenceService.CurrentWorkspace.Context.Requests
                        where entity.CourierId == courierId &&
                        entity.CreatedOn >= fromDateEndDay &&
                        entity.CreatedOn <= toDateEndDay //&&
                        //entity.Status != (int)StatusRequest.Abandoned &&
                        //entity.Status != (int)StatusRequest.Declined &&
                        //entity.Status != (int)StatusRequest.Cancelled
                        join dis in distance on entity.Id equals dis.RequestId into distanceInto
                        from dis1 in distanceInto.DefaultIfEmpty()
                        select new RequestReportVo
                        {
                            Id = entity.Id,
                            RequestNo = entity.RequestNo,
                            Email = entity.Courier.User.Email,
                            FirstName = entity.Courier.User.FirstName,
                            MiddleName = entity.Courier.User.MiddleName,
                            LastName = entity.Courier.User.LastName,
                            HomePhone = entity.Courier.User.HomePhone,
                            MobilePhone = entity.Courier.User.MobilePhone,
                            RequestDate = entity.CreatedOn,
                            LocationFrom = entity.LocationFromObj.Name,
                            LocationTo = entity.LocationToObj.Name,
                            StartTime = entity.StartTime,
                            EndTime = entity.EndTime,
                            ActualStartTime = entity.ActualStartTime,
                            ActualEndTime = entity.ActualEndTime,
                            EstimateDistance = entity.EstimateDistance,
                            //EstimateTime = entity.EstimateTime,

                            ActualDistance = dis1.Distance

                        };

            return query.ToList();          
            //return new { Data = query.ToList(), TotalRowCount = query.ToList().Count() };
        }

        public Request GetRequestWithCourier(int requestId)
        {
            return
                GetAll()
                    .Where(o => o.Id == requestId)
                    .Include(o => o.Courier)
                    .Include(o => o.Courier.User)
                    .Include(o => o.CreatedBy)
                    .FirstOrDefault();
        }


        public RequestGridVo GetRequestForTracking(int? requestId = null, string requestNo = null)
        {
            IQueryable<Request> query = null;
            if (requestId == null && string.IsNullOrEmpty(requestNo))
            {
                return null;
            }
            if (requestId.GetValueOrDefault() > 0)
            {
                query = GetAll().Where(o => o.Id == requestId);
            }

            if (!string.IsNullOrEmpty(requestNo))
            {
                query = GetAll().Where(o => o.RequestNo.Equals(requestNo));
            }

            if (query == null)
            {
                return null;
            }

            return query.Select(request => new RequestGridVo
            {
                Id = request.Id,
                RequestNo = request.RequestNo,
                CourierId = request.CourierId,
                FirstNameCourier =
                    request.Courier != null && request.Courier.User != null ? request.Courier.User.FirstName : "",
                MiddleNameCourier =
                    request.Courier != null && request.Courier.User != null ? request.Courier.User.MiddleName : "",
                LastNameCourier =
                    request.Courier != null && request.Courier.User != null ? request.Courier.User.LastName : "",
                CourierSearch =
                    request.Courier != null && request.Courier.User != null
                        ? (request.Courier.User.FirstName + " " + request.Courier.User.LastName +
                           (!string.IsNullOrEmpty(request.Courier.User.MiddleName)
                               ? " " + request.Courier.User.MiddleName
                               : ""))
                        : "",
                LocationFromId = request.LocationFrom,
                LocationToId = request.LocationTo,
                LocationFromName = request.LocationFromObj != null ? request.LocationFromObj.Name : "",
                LocationToName = request.LocationToObj != null ? request.LocationToObj.Name : "",
                Type = request.IsStat == true ? "Priority Job" : "Normal",
                StatusId = request.Status,
                TimeNoFormat = request.SendingTime,
                SendingTime = request.SendingTime,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Note =
                    request.Description.Length > 200
                        ? request.Description.Substring(0, 200) + "..."
                        : request.Description,
                CreatedDateNoFormat = request.CreatedOn,
                FirstNameCreatedBy = request.CreatedBy != null ? request.CreatedBy.FirstName : "",
                MiddleNameCreatedBy = request.CreatedBy != null ? request.CreatedBy.MiddleName : "",
                LastNameCreatedBy = request.CreatedBy != null ? request.CreatedBy.LastName : "",
                IsExpired = (request.IsExpired ?? false),
                IsSchedule = request.HistoryScheduleId != null,
                IsAgreed = (request.IsAgreed ?? false),
            }).FirstOrDefault();

        }

        public PictureAndNoteVo GetPictureAndNoteRequestComplete(int requestId)
        {
            var result = GetById(requestId);
            if (result != null)
            {

                return new PictureAndNoteVo
                {
                    Note = result.CompleteNote,
                    Picture = result.CompletePicture != null ? Convert.ToBase64String(result.CompletePicture, 0, result.CompletePicture.Length) : string.Empty,
                };
            }
            return null;
        }

        public List<Request> GetListRequestByCourier(int requestId, int courierId)
        {
            //Thao : Ngay bat dau vs ket thuc 1 ngay cua client
            var startNowTime = DateTimeHelper.GetStartDateTime(DateTime.UtcNow);
            var endNowTime = DateTimeHelper.GetEndDateTime(DateTime.UtcNow);

            return
                GetAll()
                    .Include(o => o.LocationFromObj)
                    .Include(o => o.LocationToObj)
                    .Where(o => o.CourierId == courierId && o.StartTime >= startNowTime && o.EndTime <= endNowTime && o.Status != 30 && o.Status != 20)
                    .ToList();
        }
    }
}