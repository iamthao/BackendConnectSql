using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DomainModel.DataTransferObject
{
    public class PaymentTransactionItem
    {
        public string TransactionId { get; set; }
        public string SubmittedOnUtc { get; set; }
        public string TransactionStatus { get; set; }
        public string Product { get; set; }
        public string InvoiceNumber { get; set; }
        public int RequestId { get; set; }
    }

    public class PaymentTransactionItemDto
    {
        public List<PaymentTransactionItem> PaymentTransactionItems { get; set; }
        public string ResultCode { get; set; }
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public int RequestId { get; set; }
    }
}
