using System;

namespace QuickspatchWeb.Models.FranchiseeConfiguration
{
    public class FranchiseeInfoShareViewModel 
    {

        public string FranchiseeId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string OfficePhone { get; set; }
        public string FaxNumber { get; set; }

        public string LicenseKey { get; set; }
        public string FranchiseeContact { get; set; }
        public string PrimaryContactPhone { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactFax { get; set; }
        public string PrimaryContactCellNumber { get; set; }
        public string Logo { get; set; }
        public string StartActiveDate { get; set; }
        public string EndActiveDate { get; set; }
    }
}