using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuickspatchWeb.Models.FranchiseeConfiguration
{
    public class TransactionDetailViewModel  : ViewModelBase
    {
        public string TransactionId { get; set; }
        public string TransactionType { get; set; }

        public string SettleAmount { get; set; }
        public string Status { get; set; }
       

        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }
}