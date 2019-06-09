using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using Database.Persistance.Tenants;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;
using Repositories.Interfaces;

namespace Repositories
{
    public class EntityFrameworkHoldingRequestRepository : EntityFrameworkTenantRepositoryBase<HoldingRequest>, IHoldingRequestRepository
    {
        public EntityFrameworkHoldingRequestRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {
            SearchColumns.Add("LocationFrom");
            SearchColumns.Add("LocationTo");
            SearchColumns.Add("Description");
        }

        public dynamic GetListHoldingRequest(IQueryInfo queryInfo)
        {
            var requestQueryInfo = (HoldingRequestQueryInfo)queryInfo;
            var startDate = requestQueryInfo.StartDate.ToUniversalTime();
            var endDate = requestQueryInfo.EndDate.ToUniversalTime();
            //Expression<Func<HoldingRequest, bool>> expression = s => s.CreatedOn >= startDate && s.CreatedOn <= endDate;

            BuildSortExpression(queryInfo);
            var searchString = SearchStringForGetData(queryInfo);
            var queryResult = (from entity in GetAll().Where(o => o.SendDate >= startDate && o.SendDate <= endDate)
                               select new { entity }).Select(s => new HoldingRequestVo()
                {
                    Id = s.entity.Id,
                    LocationFromId = s.entity.LocationFrom,
                    LocationFrom = s.entity.Location.Name,
                    LocationToId = s.entity.LocationTo,
                    LocationTo = s.entity.Location1.Name,
                    Description = s.entity.Description,
                    EndTime = s.entity.EndTime,
                    EndTimeNoFormat = s.entity.EndTime,
                    StartTime = s.entity.StartTime,
                    StartTimeNoFormat = s.entity.StartTime,
                    SendDate = s.entity.SendDate
                }).Where(searchString);

            var data = queryResult.OrderBy(queryInfo.SortString).ToList();
            queryInfo.TotalRecords = queryResult.Count();
            return new { Data = data, TotalRowCount = queryInfo.TotalRecords };
        }

        protected override void BuildSortExpression(IQueryInfo queryInfo)
        {
            if (queryInfo.Sort == null || queryInfo.Sort.Count == 0)
            {
                queryInfo.Sort = new List<Sort> { new Sort { Field = "SendDate", Dir = "desc" } };
            }
        }
    }
}