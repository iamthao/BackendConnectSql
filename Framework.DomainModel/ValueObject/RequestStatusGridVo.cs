using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DomainModel.ValueObject
{
    public class RequestStatusGridVo : ReadOnlyGridVo
    {
        public string Code { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public string CourierName { get; set;}
        public DateTime CompleteTime { get; set; }
        public int CompleteRequests { get; set; }
        public int PendingRequests { get; set; }
        public int CancelRequests { get; set; }
        public int AbandonedRequests { get; set; }
        public int NotSendRequests { get; set; }
        public int WaitingRequests { get; set; }
    }
}
