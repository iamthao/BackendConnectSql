using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq.Expressions;
using Database.Persistance.Tenants;
using Framework.DomainModel.Common;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;
using Framework.Utility;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;

namespace Repositories
{
    public class EntityFrameworkCourierRepository : EntityFrameworkTenantRepositoryBase<Courier>, ICourierRepository
    {
        public EntityFrameworkCourierRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {
            DisplayColumnForCombobox = "User.FirstName";
            Includes.Add("User");
            SearchColumns.Add("FullName");
            SearchColumns.Add("UserName");
            SearchColumns.Add("Email");
            SearchColumns.Add("HomePhone");
            SearchColumns.Add("MobilePhone");
            SearchColumns.Add("CarNo");

        }       

        public Courier GetCourierInfo(int id)
        {
            return GetAll()
                .Include(o => o.User)   
                .FirstOrDefault(o => o.Id == id);         
        }

        public dynamic GetListCourierForDashboard(QueryInfo queryInfo)
        {
            //BuildSortExpression(queryInfo);

            //Build querry
            var querry = GetAll().Where(o=>o.User.IsActive).Select(o => new CourierGridVo()
            {
                //FullName = o.User != null ? o.User.FirstName + " " + o.User.LastName + (string.IsNullOrEmpty(o.User.MiddleName) ? "" : " " + o.User.MiddleName) : "", 
                FirstName = o.User.FirstName,
                MiddleName = o.User.MiddleName,
                LastName = o.User.LastName,
                FullName = o.User.FirstName + " " + o.User.LastName + (!string.IsNullOrEmpty(o.User.MiddleName)?" "+o.User.MiddleName:""),
                Status = o.Status,
                Id = o.Id,
                Imei = o.Imei
            });

            //filter by searchString that user input
            if (!string.IsNullOrEmpty(queryInfo.SearchString))
            {
                querry = querry.Where(o => o.FullName.Contains(queryInfo.SearchString) );
            }

            // Sort querry by status
            querry = querry.OrderByDescending(o => o.Status);          
            

            // get the total 
            queryInfo.TotalRecords = querry.Count();

            //Get data and convert to list
            var data = querry.ToList();
            
            return new { Data = data, TotalRowCount = queryInfo.TotalRecords };
        }

        public override List<LookupItemVo> GetLookup(LookupQuery query, Func<Courier, LookupItemVo> selector)
        {
            var conditionQuery = query.Query;
            query.Query = "";

            var result = base.GetLookup(query, selector);
            if (!string.IsNullOrEmpty(conditionQuery))
            {
                result = result.Where(p => p.DisplayName.ToLower().Contains(conditionQuery.ToLower())).ToList();
            }
            return result;
        }

        public dynamic GetCouriersForSchedule(QueryInfo queryInfo)
        {
            var totalRowCount = 0;
            var l = GetDataFromStoredProcedure<CourierScheduleGridVo>(GetConnectionString(), "udsGetCouriersForSchedule", queryInfo, ref totalRowCount);

            return new { Data = l, TotalRowCount = totalRowCount };
        }

        public dynamic GetAutoAssignCourier()
        {
            var totalRowCount = 0;
            var l = GetDataFromStoredProcedure<AutoAssignCourier>(GetConnectionString(), "udsGetAutoAssignCourier", null, ref totalRowCount);

            return new { Data = l, TotalRowCount = totalRowCount };
        }
        protected override string BuildLookupCondition(LookupQuery query)
        {
            var where = new StringBuilder();
            @where.Append("(");
            var innerWhere = new List<string>();
            var queryDisplayName = (String.Format("(User.FirstName.Contains(\"{0}\") OR User.LastName.Contains(\"{0}\") OR User.MiddleName.Contains(\"{0}\"))", query.Query)+" AND User.IsActive=true");
            innerWhere.Add(queryDisplayName);
            @where.Append(String.Join(" OR ", innerWhere.ToArray()));
            @where.Append(")");
            if (query.HierachyItems != null)
            {
                foreach (var parentItem in query.HierachyItems.Where(parentItem => parentItem.Value != string.Empty && parentItem.Value != "-1"
                                                                                    && parentItem.Value != "0" && !parentItem.IgnoredFilter))
                {
                    var filterValue = parentItem.Value.Replace(",", string.Format(" OR {0} = ", parentItem.Name));
                    @where.Append(string.Format(" AND ( {0} = {1})", parentItem.Name, filterValue));
                }
            }
            return @where.ToString();
        }

        public override IQueryable<ReadOnlyGridVo> BuildQueryToGetDataForGrid(IQueryInfo queryInfo)
        {
            var queryResult = (from entity in GetAll()
                               select new { entity }).Select(s => new CourierGridVo
                               {
                                   Id = s.entity.Id,
                                   UserName =  s.entity.User.UserName,
                                   Password =  s.entity.User.Password,
                                   FirstName = s.entity.User.FirstName,
                                   LastName = s.entity.User.LastName,
                                   MiddleName = s.entity.User.MiddleName,
                                   FullName = s.entity.User.FirstName + " " + s.entity.User.LastName + (!string.IsNullOrEmpty(s.entity.User.MiddleName)?" "+s.entity.User.MiddleName:""),
                                    
                                   Email = s.entity.User.Email,
                                   HomePhone = s.entity.User.HomePhone,
                                   MobilePhone = s.entity.User.MobilePhone,
                                   CarNo = s.entity.CarNo,
                                   IsActive = s.entity.User.IsActive,
                                   Status = s.entity.Status,
                                   Imei =s.entity.Imei
                               }).OrderBy(queryInfo.SortString);

            return queryResult;
        }

        protected override void BuildSortExpression(IQueryInfo queryInfo)
        {
            if (queryInfo.Sort == null || queryInfo.Sort.Count == 0)
            {
                queryInfo.Sort = new List<Sort> { new Sort { Field = "FirstName", Dir = "" } };
               
            }
            queryInfo.Sort.ForEach(x =>
            {
                if (x.Field == "FullName")
                {
                    x.Field = "FirstName";
                }
                else if (x.Field == "HomePhoneInFormat")
                {
                    x.Field = "HomePhone";
                }
                else if (x.Field == "MobilePhoneInFormat")
                {
                    x.Field = "MobilePhone";
                }
                else
                {
                    x.Field = string.Format("{0}", x.Field);
                }
            });
        }

        public List<LookupItemVo> GetLookupForTracking(LookupQuery query, Func<Courier, LookupItemVo> selector)
        {
            var lookupWhere = BuildLookupCondition(query);

            var lookupList = GetAll().AsNoTracking().Where(p=> p.Trackings.Any()).Where(lookupWhere);

            if (!string.IsNullOrEmpty(query.CustomeFilterKey) && query.CustomeFilterKey == "TrackingDate")
            {
                if (string.IsNullOrEmpty(query.CustomeFilterValue))
                {
                    return new List<LookupItemVo>();
                }
                var trackingDate = DateTime.Parse(query.CustomeFilterValue);
                var fromdt = (new DateTime(trackingDate.Year, trackingDate.Month, trackingDate.Day, 0, 0, 0)).ToUtcTimeFromClientTime();
                var todt = (new DateTime(trackingDate.Year, trackingDate.Month, trackingDate.Day, 23, 59, 59)).ToUtcTimeFromClientTime();
                if (trackingDate > DateTime.MinValue)
                {
                    fromdt = DateTimeHelper.GetStartDateTime(trackingDate);
                    todt = DateTimeHelper.GetEndDateTime(trackingDate);
                }

                lookupList =
                    lookupList.Where(p => p.Trackings.Any(o => o.TimeTracking != null && o.TimeTracking.Value >= fromdt && o.TimeTracking.Value <= todt) && p.Requests.Any());
            }
                

            var currentRecord = GetAll().AsNoTracking().Where(x => x.Id == query.Id);
            if (!query.IncludeCurrentRecord && currentRecord.SingleOrDefault() != null) 
            {
                return currentRecord.Select(selector).ToList();
            }

            if (!string.IsNullOrEmpty(query.Query) || !query.IncludeCurrentRecord)
            {
                currentRecord = Enumerable.Empty<Courier>().AsQueryable();
            }

            var lookupAnonymous = lookupList
                .Union(currentRecord)
                .OrderBy(DisplayColumnForCombobox)
                .Skip(0)
                .Take(query.Take)
                .Select(selector);
            return lookupAnonymous.ToList();
        }
        
        public dynamic GetPositionCurrentOfCourier(int courierId)
        {
            var query = (from courier in GetAll().Where(o => o.Id == courierId)
                //join trac in TenantPersistenceService.CurrentWorkspace.Context.Trackings on courier.Id equals
                //    trac.CourierId into trackings
                //from tracking in trackings.DefaultIfEmpty().OrderByDescending(o => o.TimeTracking)
                join req in TenantPersistenceService.CurrentWorkspace.Context.Requests on courier.CurrentReq equals
                    req.Id into requests
                from request in requests.DefaultIfEmpty()
                select new CourierOnlineVo()
                {
                    CourierId = courier.Id,
                    CurrentRequestId = courier.CurrentReq,
                    CurrentRequestNo = request != null && request.Status == (int)StatusRequest.Started ? request.RequestNo : "",
                    Lat = courier.CurrentLat,
                    Lng = courier.CurrentLng,
                    CurrentVelocityNoConvert = courier.CurrentVelocity,
                    Avatar = courier.User != null ? courier.User.Avatar : null,
                    FirstName = courier.User != null ? courier.User.FirstName : "",
                    MiddleName = courier.User != null ? courier.User.MiddleName : "",
                    LastName = courier.User != null ? courier.User.LastName : ""
                }).FirstOrDefault();
            return query;
        }

        public dynamic GetAllCourierOnlineLocation()
        {
            var query = (from courier in GetAll().Where(o => o.Status == (int)StatusCourier.Online)
                         //join trac in TenantPersistenceService.CurrentWorkspace.Context.Trackings on courier.Id equals
                         //    trac.CourierId into trackings
                         //from tracking in trackings.DefaultIfEmpty().OrderByDescending(o => o.TimeTracking)
                         join req in TenantPersistenceService.CurrentWorkspace.Context.Requests on courier.CurrentReq equals
                             req.Id into requests
                         from request in requests.DefaultIfEmpty()
                         select new CourierOnlineVo()
                         {
                             CourierId = courier.Id,
                             CurrentRequestId = courier.CurrentReq,
                             CurrentRequestNo = request != null ? request.RequestNo : "",
                             Lat = courier.CurrentLat,
                             Lng = courier.CurrentLng,
                             CurrentVelocityNoConvert = courier.CurrentVelocity,
                             Avatar = courier.User != null ? courier.User.Avatar : null,
                             FirstName = courier.User != null ? courier.User.FirstName : "",
                             MiddleName = courier.User != null ? courier.User.MiddleName : "",
                             LastName = courier.User != null ? courier.User.LastName : ""
                         }).ToList();
            return new { Data = query, TotalRowCount = query.Count() };
        }

        public List<LookupItemVo> GetLookupForReport(LookupQuery lookupQuery, Func<Courier, LookupItemVo> selector)
        {

            var lookupWhere = BuildLookupCondition(lookupQuery);

            var lookupList = GetAll().AsNoTracking().Where(lookupWhere);

            var currentRecord = GetAll().AsNoTracking().Where(x => x.Id == lookupQuery.Id);
            if (!lookupQuery.IncludeCurrentRecord && currentRecord.SingleOrDefault() != null)
            {
                return currentRecord.Select(selector).ToList();
            }

            if (!string.IsNullOrEmpty(lookupQuery.Query) || !lookupQuery.IncludeCurrentRecord)
            {
                currentRecord = Enumerable.Empty<Courier>().AsQueryable();
            }

            var lookupAnonymous = lookupList
                .Union(currentRecord)
                .OrderBy(DisplayColumnForCombobox)
                .Skip(0)
                .Take(lookupQuery.Take)
                .Select(selector);
            return lookupAnonymous.ToList();
        }
    }
}