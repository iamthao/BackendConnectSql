using System;

namespace QuickspatchWeb.Models.FranchiseeConfiguration
{
    public class DashboardFranchiseeConfigurationShareViewModel : DashboardSharedViewModel
    {
        public string FranchiseeId { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string OfficePhone { get; set; }
        public string Address2 { get; set; }
        public string FaxNumber { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string State { get; set; }
        public string LicenseKey { get; set; }
        public string FranchiseeContact { get; set; }
        public string PrimaryContactPhone { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactFax { get; set; }
        public string PrimaryContactCellNumber { get; set; }
        public string Logo { get; set; }
        public DateTime? StartActiveDate { get; set; }
        public DateTime? EndActiveDate { get; set; }
    }
}