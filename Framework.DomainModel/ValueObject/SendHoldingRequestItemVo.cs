using System.Collections.Generic;

namespace Framework.DomainModel.ValueObject
{
    public class SendHoldingRequestItemVo : ReadOnlyGridVo
    {
        public int HoldingRequestSelectedId { get; set; }
        public int? CourierId { get; set; }
        public bool IsStat { get; set; }
        public System.DateTime? SendingTime { get; set; }
        public int? ExpiredTime { get; set; }
    }

    public class SendListHoldingRequestItemVo : ReadOnlyGridVo
    {
        public List<int> HoldingRequestSelectedIds { get; set; }
        public int? CourierId { get; set; }
        public bool IsStat { get; set; }
        public System.DateTime? SendingTime { get; set; }
        public int? ExpiredTime { get; set; }
    }
}