using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DomainModel.DataTransferObject
{
    public class GetPaymentInfoItemDto
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string AccountNumber { get; set; }
        public string SubscriptionStatus { get; set; }
        public string SubscriptionIntervalUnit { get; set; }
        public string SubscriptionIntervalLength { get; set; }
        public string SubscriptionAmount { get; set; }
        public string SubscriptionStartDateUtc { get; set; }
        public string ResultCode { get; set; }
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string RequestId { get; set; }
        public string SubmitedOn { get; set; }

    }
}
