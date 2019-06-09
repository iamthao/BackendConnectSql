using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DomainModel.DataTransferObject
{
    public class AddPackageHistoryDto
    {
        public int PackageId { get; set; }
        public int OldPackageId { get; set; }
        public int RequestId { get; set; }
        public string FranchiseeName { get; set; }
        public string LicenseKey { get; set; }
        public int FranchiseeTenantId { get; set; }
        public string AccountNumber { get; set; }
        public bool IsApply { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? NextBillingDate { get; set; }
        public int? PackageNextBillingDate { get; set; }
    }
}
