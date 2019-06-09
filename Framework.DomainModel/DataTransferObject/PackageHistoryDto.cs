using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DomainModel.DataTransferObject
{
    public class PackageHistoryDto : DtoBase
    {
        public string FranchiseeName { get; set; }
        public string LicenseKey { get; set; }

        public int PackageId { get; set; }
        public int? OldPackageId { get; set; }

        public DateTime StartDate { get; set; }
        public string ChangeDate
        {
            get { return StartDate.ToShortDateString(); }
        }

        public DateTime? EndDate { get; set; }

        public int RequestId { get; set; }
        public int FranchiseeTenantId { get; set; }

        public string PackageName
        {
            get { return GetNamePackage(PackageId); }
        }
        public string OldPackageName 
        {
            get { return GetNamePackage(OldPackageId ?? 0); }
        }

        private string GetNamePackage(int packageId)
        {
            switch (packageId)
            {
                case 1:
                    return "Package (10 Couriers) - Paid Annually";
                case 2:
                    return "Package (10 Couriers) - Paid Monthly";
                case 3:
                    return "Package (25 Couriers) - Paid Annually";
                case 4:
                    return "Package (25 Couriers) - Paid Monthly";
                case 5:
                    return "Package (50 Couriers) - Paid Annually";
                case 6:
                    return "Package (50 Couriers) - Paid Monthly";
                //case 7:
                //    return "Package (115 Couriers) - Paid Annually";
                //case 8:
                //    return "Package (115 Couriers) - Paid Monthly";
            }
            return "Trial (2 Couriers)";
        }

        public bool IsApply { get; set; }
        public bool IsCancel { get; set; }

        public decimal? Amount { get; set; }
        public DateTime? NextBillingDate { get; set; }
        public int? PackageNextBillingDate { get; set; }
    }



}
