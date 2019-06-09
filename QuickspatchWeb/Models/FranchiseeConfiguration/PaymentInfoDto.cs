using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuickspatchWeb.Models.FranchiseeConfiguration
{
    public class PaymentInfoDto
    {
        public string ProductKey { get; set; }
        public string SecretKey { get; set; }
        public string AdditionInfo { get; set; }
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
        public int IsRecurrence { get; set; }
        public int RecurrenceInterval { get; set; }
        
        public List<RegisterProduct> Items { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
        public string AccountNumber { get; set; }
        public int? TransactionType { get; set; }
        public int  RequestId { get; set; }

        public DateTime StartDate { get; set; }
        public decimal TrialAmount { get; set; }
    }

    public class RegisterProduct
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int ItemQuantity { get; set; }
        public decimal ItemPrice { get; set; }
    }
}