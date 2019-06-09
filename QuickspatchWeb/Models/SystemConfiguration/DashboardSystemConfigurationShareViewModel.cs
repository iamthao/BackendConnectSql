
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Framework.DomainModel.Entities;

namespace QuickspatchWeb.Models.SystemConfiguration
{
    public class DashboardSystemConfigurationShareViewModel : DashboardSharedViewModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public DataType DataType { get; set; }
        public SystemConfigType SystemConfigType { get; set; }
        public int DataTypeId { get; set; }
        public int SystemConfigTypeId { get; set; }
    }
}