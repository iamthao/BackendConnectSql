using System;
using System.Collections.Generic;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;

namespace ServiceLayer.Interfaces
{
    public interface IRequestService : IMasterFileService<Request>
    {
        dynamic GetListHoldingRequest(IQueryInfo queryInfo);
        bool ReAssignCourier(int id, int courierId);
        string CancelRequest(int id);
        dynamic GetRequestListByCourier(QueryInfo queryInfo);
        ListRequestDto GetRequestForToday(int userId, int utcClient);
        void UpdateRequestStatus(UpdateRequestDto requestDto);
        void UpdateListRequestStatus(RequestStatusDto requestStatusDto);
        dynamic GetPieChartData(int? courierId);
        dynamic GetCurrentDataRequests(IQueryInfo queryInfo);
        void AddNoteRequest(UpdateSingleNoteRequestDto requestDto);
        void UpdateListRequestForService(IList<Request> listRequest);
        void AddListRequestForService(IList<Request> listRequest);
        dynamic GetListRequestForReport(int courierId, DateTime fromDate, DateTime toDate);
        dynamic GetNotesDetail(int requestId);
        dynamic GetRequestForTracking(int? requestId = null, string requestNo = null);
        dynamic GetPictureAndNoteRequestComplete(int requestId);
        dynamic GetListTrackingDataFromTo(int fromId, int toId);
        WarningInfoVo GetWarningInfo(int requestId, int courierId);
        dynamic GetRequestCourierForCreate(int courierId);
        dynamic GetLatLngForLocation(int locationId);
    }
}