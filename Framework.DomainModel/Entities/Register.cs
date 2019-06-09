using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DomainModel.Entities
{
    public class Register : Entity
    {
        public string Code { get; set; }
        public string CompanyName { get; set; }
        public string ApiDomainName { get; set; }
        public string DomainName { get; set; }
        public string DatabaseName { get; set; }
        public int? IndustryId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string PasswordHashcode { get; set; }
        public bool? IsConfirm { get; set; }
        public bool? IsDone { get; set; }
        public string LicenseKey { get; set; }
        public DateTime? StartActiveDate { get; set; }
        public DateTime? EndActiveDate { get; set; }
        public int? NumberOfCourier { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public int DeploymentId { get; set; }
        //public virtual Industry Industry { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public int? FirstRequestId { get; set; }
        public DateTime? FirstPayDate { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMessage { get; set; }
    }
}
