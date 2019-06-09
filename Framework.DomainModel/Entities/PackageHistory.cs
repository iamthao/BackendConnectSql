using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Framework.DomainModel.Entities
{
    public class PackageHistory : Entity
    {
        public PackageHistory()
        {
        }

        public int PackageId { get; set; }
        public int? OldPackageId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int FranchiseeTenantId { get; set; }
        public int RequestId { get; set; }
        public string AccountNumber { get; set; }
        public bool? IsApply { get; set; }
        public virtual FranchiseeTenant FranchiseeTenant { get; set; }
    }
}
