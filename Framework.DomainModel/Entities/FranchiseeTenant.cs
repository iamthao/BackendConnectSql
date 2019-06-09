using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Framework.DomainModel.Entities.Mapping;

namespace Framework.DomainModel.Entities
{
    public class FranchiseeTenant : Entity
    {
        public FranchiseeTenant()
        {
            FranchiseeModule = new List<FranchiseeModule>();
            PackageHistories = new List<PackageHistory>();
        }

        public string Name { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string State { get; set; }
        public string OfficePhone { get; set; }
        public string FaxNumber { get; set; }
        public string LicenseKey { get; set; }
        public DateTime? StartActiveDate { get; set; }
        public DateTime? EndActiveDate { get; set; }
        public int? NumberOfCourier { get; set; }
        public int? IndustryId { get; set; }
        public bool IsActive { get; set; }
        public int? CurrentPackageId { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMessage { get; set; }

        public int? TotalNotificationTrial { get; set; }
        public int? TotalNotificationSuccess { get; set; }
        public int? TotalNotificationError { get; set; }
        public int? TotalNotificationBeforPayment { get; set; }

        public DateTime? CloseDate { get; set; }
        public string QuestionClose { get; set; }
        public string DescriptionClose { get; set; }
        public string AccountNumber { get; set; }
        public decimal? RemainingAmount { get; set; }
        public DateTime? NextBillingDate { get; set; }
        public int? PackageNextBillingDate { get; set; }
        public bool? AlertExtendedPackage { get; set; }

        public DateTime? StartDateSuccess { get; set; }
        public DateTime? EndDateSuccess { get; set; }
        public virtual ICollection<FranchiseeModule> FranchiseeModule { get; set; }
        public virtual ICollection<PackageHistory> PackageHistories { get; set; }
        public virtual Industry Industry { get; set; }


        [NotMapped ]
        public string FranchiseeContact { get; set; }
        [NotMapped]
        public string PrimaryContactPhone { get; set; }
        [NotMapped]
        public string PrimaryContactEmail { get; set; }
        [NotMapped]
        public string PrimaryContactFax { get; set; }
        [NotMapped]
        public string PrimaryContactCellNumber { get; set; }
  
    }
}