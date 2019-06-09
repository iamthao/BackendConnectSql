using System;
using System.Collections.Generic;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;
using Framework.Repositories;

namespace Repositories.Interfaces
{
    public interface IRequestRepository : IRepository<Request>, IQueryableRepository<Request>
    {
        dynamic GetRequestListByCourier(QueryInfo queryInfo);
        List<RequestDto> GetRequestForToday(int userId, int utcClient);
        dynamic GetPieChartData(int? courierId);
        dynamic GetCurrentDataRequests(IQueryInfo queryInfo);
        RequestDeliveryAgreementVo GetListDeliveryAgreementVo(IQueryInfo queryInfo);
        List<RequestReportVo> GetListRequestForReport(int courierId, DateTime fromDate, DateTime toDate);

        Request GetRequestWithCourier(int requestId);

        RequestGridVo GetRequestForTracking(int? requestId = null, string requestNo = null);

        PictureAndNoteVo GetPictureAndNoteRequestComplete(int requestId);

        List<Request> GetListRequestByCourier(int requestId, int courierId);
        List<RequestCourierGridVo> GetRequestCourierForCreate( int courierId);

        LatLngVo GetLatLngForLocation(int locationId);
    }
}