using System;
using Framework.Utility;
namespace Framework.DomainModel.ValueObject
{
    public class FranchiseeTenantGridVo : ReadOnlyGridVo
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

        public DateTime? StartActiveDateNoFormat { get; set; }
        public string StartActiveDate
        {
            get
            {
                if (StartActiveDateNoFormat != null)
                    return ((DateTime)StartActiveDateNoFormat).ToString("MM/dd/yyyy hh:mm tt");
                return "";
            }
        }
        public DateTime? EndActiveDateNoFormat { get; set; }
        public string EndActiveDate
        {
            get
            {
                if (EndActiveDateNoFormat != null)
                    return ((DateTime)EndActiveDateNoFormat).ToString("MM/dd/yyyy hh:mm tt");
                return "";
            }
        }

        public bool IsActive { get; set; }
    }
}