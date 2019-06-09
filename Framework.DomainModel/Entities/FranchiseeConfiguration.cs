using System;
using System.Collections.Generic;

namespace Framework.DomainModel.Entities
{
    public class FranchiseeConfiguration : Entity
    {
        public string FranchiseeId { get; set; }
        public string Name { get; set; }
        public string FranchiseeContact { get; set; }
        public string PrimaryContactPhone { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactFax { get; set; }
        public string PrimaryContactCellNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string State { get; set; }
        public string OfficePhone { get; set; }
        public string FaxNumber { get; set; }
        public byte[] Logo { get; set; }
        public string LicenseKey { get; set; }
        public bool? IsQuickTour { get; set; }
        public int? IndustryId { get; set; }
        public int? LocationFromId { get; set; }
        public int? LocationToId { get; set; }
        public virtual Industry Industry { get; set; }
    }
}