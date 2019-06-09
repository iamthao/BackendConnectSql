
using System;
using System.Collections.Generic;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;

namespace ServiceLayer.Interfaces
{
    public interface ICourierService : IMasterFileService<Courier>
    {
        Courier GetCourierInfo(int id);
        Courier GetCourierWithUsernameAndPassword(UsernameAndPasswordDto userInfo);
        int CheckConnection(CheckConnectDto checkConnectDto);
        void LogOut(int courierId);
        dynamic GetListCourierForDashboard(QueryInfo queryInfo);
        dynamic GetCouriersForSchedule(QueryInfo queryInfo);
        List<LookupItemVo> GetLookupForTracking(LookupQuery query, Func<Courier, LookupItemVo> selector);
        dynamic GetAutoAssignCourier();
        void UpdateListCourierForService(IList<Courier> listCourier);
        dynamic GetPositionCurrentOfCourier(int courierId);
        dynamic GetAllCourierOnlineLocation();
        Courier UpdateWithouCheckBussinessRule(Courier courier);
        List<LookupItemVo> GetLookupForReport(LookupQuery query, Func<Courier, LookupItemVo> selector);
    }
}