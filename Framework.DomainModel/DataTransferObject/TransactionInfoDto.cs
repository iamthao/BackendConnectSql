using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DomainModel.DataTransferObject
{
    public class TransactionInfoDto
    {
        public string TransactionId { get; set; }
        public string TransactionType { get; set; }
        public string TransactionStatus { get; set; }
        public string AuthAmount { get; set; }
        public string SettleAmount { get; set; }
        public string Product { get; set; }
        public BillingAddress BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string Items { get; set; }
        public string ResultCode { get; set; }
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string RequestId { get; set; }
    }

    public class BillingAddress
    {
        public string phoneNumber { get; set; }
        public string faxNumber { get; set; }
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string company { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string PropertyChanged { get; set; }
    }

}
