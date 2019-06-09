using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuickspatchWeb.Models.FranchiseeConfiguration
{
    public class PaymentViewModel
    {
        public string CompanyName { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public string ErrorExistDomain { get; set; }
        public string ErrorExistEmail { get; set; }
        public int DeploymentId { get; set; }

        public int? PackageId { get; set; }
        public bool IsTrial { get; set; }
        public string TranId { get; set; }
        public string AuthCode { get; set; }
        public int? RequestId { get; set; }


        public string ProductKey { get; set; }
        public string SecretKey { get; set; }
        public string Url { get; set; }
        public string PaymentUrl { get; set; }
        public string AdditionInfo { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMessage { get; set; }

        public string ResultCode { get; set; }
        public string StatusMessage { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}