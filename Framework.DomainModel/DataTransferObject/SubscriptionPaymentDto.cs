using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DomainModel.DataTransferObject
{
    public class SubscriptionPaymentDto
    {
        public string OccurrenceStartDateUtc { get; set; }
        public string OccurrenceEndDateUtc { get; set; }
        public string SubscriptionStatus { get; set; }
        public string ResultCode { get; set; }
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string RequestId { get; set; }
    }
}
