using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuickspatchWeb.Models.FranchiseeConfiguration
{
    public class BillingInfoViewModel : DashboardSharedViewModel
    {
        public string CurrentPlan { get; set; }
        public string AccountStatus { get; set; }
        public string NextBillingDate { get; set; }
        public string AccountOwner { get; set; }
        public string AccountName{ get; set; }
        public string Url { get; set; }
        public bool IsTrial { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string State { get; set; }
        public string NamePaymentInfo { get; set; }
        public string Phone { get; set; }
        public bool IsClosingAccount { get; set; }
    }

    public class AccountBillingIndexViewModel
    {
        public string CurrentPlan { get; set; }
        public string AccountStatus { get; set; }
        public string NextBillingDate { get; set; }
        public string AccountOwner { get; set; }
        public string AccountName { get; set; } 
        public string Url { get; set; }
        public bool IsTrial { get; set; }
        public bool IsClosingAccount { get; set; }
    }

    public class PaymentBillingIndexViewModel
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string State { get; set; }
        public string NamePaymentInfo { get; set; }
        public string Phone { get; set; }
    }

    public class GetInfoBillingIndex
    {
        public string CurrentPlan { get; set; }
        public string AccountStatus { get; set; }
        public string NextBillingDate { get; set; }
        public string AccountOwner { get; set; }
        public string AccountName { get; set; }
        public string Url { get; set; }
        public bool IsTrial { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string State { get; set; }
        public string NamePaymentInfo { get; set; }
        public string Phone { get; set; }
        public bool IsClosingAccount { get; set; }
    }
}