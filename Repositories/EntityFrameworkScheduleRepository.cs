using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq.Expressions;
using Database.Persistance.Tenants;
using Framework.DomainModel.Common;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using Framework.Utility;

namespace Repositories
{
    public class EntityFrameworkScheduleRepository : EntityFrameworkTenantRepositoryBase<Schedule>, IScheduleRepository
    {
        public EntityFrameworkScheduleRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {
        }

        public dynamic GetSchedulesOfCourier(int courierId)
        {
            var offset = DateTimeHelper.GetClientTimeZone();
            var currentDateParse = DateTime.UtcNow;
            var clientDate = currentDateParse.AddMinutes(offset);
            var startNowTime = currentDateParse.AddMilliseconds(-1 *
                                                 clientDate.Subtract(new DateTime(clientDate.Year, clientDate.Month, clientDate.Day)).TotalMilliseconds);
            var finalResult = GetAll().Where(o => o.CourierId == courierId ).Select(o => new ScheduleOfCourierGridVo
            {
                Id = o.Id,
                Name = o.Name,
                From = o.LocationFromObj != null ? o.LocationFromObj.Name : "",
                To = o.LocationToObj != null ? o.LocationToObj.Name : "",
                StartTime = o.StartTime,
                EndTime = o.EndTime,
                FrequencyEncode = o.Frequency,
                DurationStart = o.DurationStart == null ? startNowTime : o.DurationStart,
                DurationEnd = o.DurationEnd,
                IsWarning = o.IsWarning == true,
                IsNoDurationEnd = o.IsNoDurationEnd,
                CopyCreatedOn = o.CreatedOn
            });
            var data = finalResult.ToList().OrderBy(o => o.StartTime.ToString("yyyyMMddHHmm")).ThenBy(o => o.CopyCreatedOn.GetValueOrDefault().ToString("yyyyMMddHHmmss"));
            return new { Data = data, TotalRowCount = finalResult.Count() };
        }

        public dynamic GetDetailScheduleWeekly(int courierId, DateTime? fromDate, DateTime? toDate, int timezone)
        {

            var finalResult = GetAll().Where(o => o.CourierId == courierId).Select(o => new DetailScheduleWeeklyOfCourierGridVo
            {
                Id = o.Id,
                Name = o.Name,
                From = o.LocationFromObj != null ? o.LocationFromObj.Name : "",
                To = o.LocationToObj != null ? o.LocationToObj.Name : "",
                FrequencyEncode = o.Frequency,
                FromDate = fromDate,
                ToDate = toDate,
                DurationStart = o.DurationStart,
                DurationEnd = o.DurationEnd,
                TimeZone = -timezone,
                IsSchedule = true
            });

            var f = fromDate != null ? ((DateTime)fromDate).AddMinutes(timezone) : DateTime.Now.AddMinutes(timezone);
            var t = toDate != null ? ((DateTime)toDate).AddMinutes(timezone) : DateTime.Now.AddMinutes(timezone);
            var finalResult1 = (from o in TenantPersistenceService.CurrentWorkspace.Context.Requests
                                where o.CourierId == courierId && (o.CreatedOn >= f&&o.CreatedOn<=t)
                select new DetailScheduleWeeklyOfCourierGridVo
                {
                    Id = -o.Id,
                    Name = o.RequestNo,
                    From = o.LocationFromObj != null ? o.LocationFromObj.Name : "",
                    To = o.LocationToObj != null ? o.LocationToObj.Name : "",
                    FrequencyEncode = "",
                    FromDate = fromDate,
                    ToDate = toDate,
                    DurationStart = ((DateTime)o.CreatedOn),
                    DurationEnd = ((DateTime)o.CreatedOn),
                    CreatedOn=o.CreatedOn,
                    TimeZone = -timezone,
                    IsSchedule=false,
                    HistoryScheduleId = o.HistoryScheduleId
                });

            var data = finalResult.ToList();
            var data1 = finalResult1.ToList();
            data.AddRange(data1);

            return new { Data = data, TotalRowCount = finalResult.Count() };
        }

        public dynamic GetDetailScheduleMonthly(int courierId, DateTime? fromDate, DateTime? toDate, int timezone)
        {

            var finalResult = from entity in GetAll().Where(o => o.CourierId == courierId)
                join locationFrom in PersistenceService.CurrentWorkspace.Context.Locations on entity.LocationFrom equals locationFrom.Id
                join locationTo in PersistenceService.CurrentWorkspace.Context.Locations on entity.LocationTo equals locationTo.Id
              select new DetailScheduleMonthlyOfCourierGridVo
            {
                Id = entity.Id,
                Name = entity.Name,
                FrequencyEncode = entity.Frequency,
                FromDate = fromDate,
                ToDate = toDate,
                DurationStart = entity.DurationStart,
                DurationEnd = entity.DurationEnd,
                TimeZone = -timezone,
                IsSchedule = true,
                From = locationFrom.Name,
                To = locationTo.Name
            };
            var f = fromDate != null ? ((DateTime)fromDate).AddMinutes(timezone) : DateTime.Now.AddMinutes(timezone);
            var t = toDate != null ? ((DateTime)toDate).AddMinutes(timezone) : DateTime.Now.AddMinutes(timezone);
            var finalResult1 = from entity in TenantPersistenceService.CurrentWorkspace.Context.Requests.Where(o=> o.CourierId == courierId && (o.CreatedOn >= f && o.CreatedOn <= t))
                                join locationFrom in PersistenceService.CurrentWorkspace.Context.Locations on entity.LocationFrom equals locationFrom.Id
                                join locationTo in PersistenceService.CurrentWorkspace.Context.Locations on entity.LocationTo equals locationTo.Id
                                select new DetailScheduleMonthlyOfCourierGridVo
                                {
                                    Id = -entity.Id,
                                    Name = entity.RequestNo,
                                    FrequencyEncode = "",
                                    FromDate = fromDate,
                                    ToDate = toDate,
                                    DurationStart = ((DateTime)entity.CreatedOn),
                                    DurationEnd = ((DateTime)entity.CreatedOn),
                                    CreatedOn = entity.CreatedOn,
                                    TimeZone = -timezone,
                                    IsSchedule = false,
                                    From = locationFrom.Name,
                                    To = locationTo.Name,
                                    HistoryScheduleId = entity.HistoryScheduleId
                                };

            var data = finalResult.ToList();
            var data1 = finalResult1.ToList();
            data.AddRange(data1);

            var listRoute = new List<Route>();
            if (data.Count > 0)
            {
                foreach (var o in data)
                {
                    if (o.ListRoute != null && o.ListRoute.Count > 0)
                    {
                        listRoute.AddRange(o.ListRoute);
                    }
                }
            }
            return new { Data = listRoute, TotalRowCount = listRoute.Count() };
        }


        public List<Schedule> GetScheduleByCourier(int courierId, DateTime? durationEnd)
        {
            //A nGhiep
            //if (durationEnd != null)
            //{
            //    return
            //    GetAll()
            //        .Include(o => o.LocationFromObj)
            //        .Include(o => o.LocationToObj)
            //        .Where(
            //            o => o.CourierId == courierId && (o.IsNoDurationEnd == true || o.DurationEnd > durationEnd))
            //        .ToList();
            //}
            //return GetAll()
            //        .Include(o => o.LocationFromObj)
            //        .Include(o => o.LocationToObj)
            //        .Where(
            //            o => o.CourierId == courierId && (o.IsNoDurationEnd == true))
            //        .ToList();

            //Thao
       
             return
                GetAll()
                    .Include(o => o.LocationFromObj)
                    .Include(o => o.LocationToObj)
                    .Where(
                        o => o.CourierId == courierId)
                    .ToList();
        }

       
    }
}