using System;
using System.Collections.Generic;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Framework.Repositories;

namespace Repositories.Interfaces
{
    public interface ICourierRepository : IRepository<Courier>, IQueryableRepository<Courier>
    {
        Courier GetCourierInfo(int id);
        dynamic GetListCourierForDashboard(QueryInfo queryInfo);
        dynamic GetCouriersForSchedule(QueryInfo queryInfo);
        List<LookupItemVo> GetLookupForTracking(LookupQuery query, Func<Courier, LookupItemVo> selector);
        dynamic GetAutoAssignCourier();
        dynamic GetPositionCurrentOfCourier(int courierId);
        dynamic GetAllCourierOnlineLocation();
        List<LookupItemVo> GetLookupForReport(LookupQuery lookupQuery, Func<Courier, LookupItemVo> selector);
    }
}