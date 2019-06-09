using System.Collections.Generic;

namespace Framework.DomainModel.DataTransferObject
{
    public class TrackingRequestDto : DtoBase
    {
        public List<TrackingDto> TrackingDtos { get; set; }
    }
}