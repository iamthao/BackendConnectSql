using System;
using QuickspatchWeb.Models.FranchiseeConfiguration;

namespace QuickspatchWeb.Models.FranchiseeTenant
{
    public class DashboardFranchiseeTenantShareViewModel : DashboardSharedViewModel
    {
        public string Name { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Address1 { get; set; }
        public string OfficePhone { get; set; }
        public string Address2 { get; set; }
        public string FaxNumber { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string State { get; set; }
        public string LicenseKey { get; set; }
        public DateTime? StartActiveDate { get; set; }
        public DateTime? EndActiveDate { get; set; }
        public string FranchiseeContact { get; set; }
        public string PrimaryContactPhone { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactFax { get; set; }
        public string PrimaryContactCellNumber { get; set; }
        public string Logo { get; set; }
        public int? IndustryId { get; set; }
        public bool IsActive { get; set; }
        public int? NumberOfCourier { get; set; }
    }
}