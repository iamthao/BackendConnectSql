using System;
using System.ComponentModel.DataAnnotations;
using Framework.DomainModel.Entities;

namespace Framework.DomainModel.DataTransferObject
{
    public class FranchisseNameAndLicenseDto : DtoBase
    {
        public string FranchiseeName { get; set; }
        public string LicenseKey { get; set; }
    }

    public class NoteDto : DtoBase
    {
        public string Description { get; set; }
        public DateTime? Date { get; set; }

    }

    public class ActiveDateLicenseKeyDto
    {
        public DateTime? StartActiveDate { get; set; }
        public DateTime? EndActiveDate { get; set; }
    }
    public class FranchiseeTernantDto : DtoBase
    {
        public string FranchiseeId { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string State { get; set; }
        public string OfficePhone { get; set; }
        public string FaxNumber { get; set; }
        public int? IndustryId { get; set; }
        public int? NumberOfCourier { get; set; }
        public DateTime? StartActiveDate { get; set; }
        public DateTime? EndActiveDate { get; set; }
        public string AccountNumber { get; set; }
        public DateTime? CloseDate { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? NextBillingDate { get; set; }
        public int? PackageNextBillingDate { get; set; }
        public bool? AlertExtendedPackage { get; set; } 
    }

    public class FranchiseeTernantCloseAccountDto : DtoBase
    {
        public string FranchiseeName { get; set; }
        public string FranchiseeLicense { get; set; }
        public string Password { get; set; }
        public string Question { get; set; }
        public string Description { get; set; }
    }

    public class FranchiseeTernantCurrentPackageDto : DtoBase
    {
        public string AccountNumber { get; set; }
        public int PackageId { get; set; }
        public bool Active { get; set; }
        public decimal? Amount { get; set; }
        
    }
}