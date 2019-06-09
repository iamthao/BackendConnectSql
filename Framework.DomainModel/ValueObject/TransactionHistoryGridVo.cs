using Framework.Utility;

namespace Framework.DomainModel.ValueObject
{
    public class TransactionHistoryGridVo : ReadOnlyGridVo
    {
        public string TransactionId { get; set; }
        public string SubscriptionName { get; set; }
        public int RequestId { get; set; }
    }
}