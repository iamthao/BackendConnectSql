

namespace Framework.DomainModel.ValueObject
{
    public class StaticValueGridVo : ReadOnlyGridVo
    {
        public int RequestNumber { get; set; }

        
    }
    public class CheckSumChange : ReadOnlyGridVo
    {
        public int CheckSumRequest { get; set; }
        public int CheckSumEventDecline { get; set; }
        public int CheckSumCourier { get; set; }


    }
}
