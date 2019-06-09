using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuickspatchWeb.Models.FranchiseeConfiguration
{
    public class PaymentTransactionItemViewModel
    {
        public string TransactionId { get; set; }
        public string SubmittedOnUtc { get; set; }
        public string TransactionStatus { get; set; }
        public string Product { get; set; }
        public string InvoiceNumber { get; set; }
        public int RequestId { get; set; }

        public string SubmittedDate
        {
            get
            {
                var datetime = Convert.ToDateTime(SubmittedOnUtc);
                return datetime.ToShortDateString() + " " + datetime.ToString("HH:mm:ss");
            }
        }
    }
}