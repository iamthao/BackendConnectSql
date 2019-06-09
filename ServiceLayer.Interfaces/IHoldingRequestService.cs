using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;

namespace ServiceLayer.Interfaces
{
    public interface IHoldingRequestService : IMasterFileService<HoldingRequest>
    {
        void SendHoldingRequest(SendHoldingRequestItemVo HoldingRequestService);
        void SendListHoldingRequest(SendListHoldingRequestItemVo listHoldingRequestItem);
    }
}