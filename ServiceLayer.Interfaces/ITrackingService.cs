using System;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;

namespace ServiceLayer.Interfaces
{
    public interface ITrackingService : IMasterFileService<Tracking>
    {
        dynamic GetListTrackingData(int courierId, DateTime filterDateTime, int? requestId);
        void UpdateTrackingHistory(TrackingRequestDto trackingRequestDto);
        //dynamic GetAllCourierOnlineLocation();
    }
}