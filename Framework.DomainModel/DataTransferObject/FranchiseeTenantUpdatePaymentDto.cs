using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DomainModel.DataTransferObject
{
    public class FranchiseeTenantUpdatePaymentDto : FranchisseNameAndLicenseDto
    {
        public decimal Amount { get; set; }
        public DateTime NextBillingDate { get; set; }
    }
}
