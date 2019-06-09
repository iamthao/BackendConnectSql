using System.Collections.Generic;
using Framework.DomainModel.Entities.Security;

namespace QuickspatchWeb.Models.ModuleDocumentTypeOperation
{
    public class DashboardModuleDocumentTypeOperationShareViewModel : DashboardSharedViewModel
    {
        public string ModuleName { get; set; }
        public int ModuleId { get; set; }
    }

    public class DashboardModuleDocumentTypeOperationDataItem
    {
        public int Id { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentTypeName { get; set; }
        public int SercurityOperation { get; set; }

        public bool IsView { get; set; }
        public bool IsInsert { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }
        public bool IsProcess { get; set; }
    }
}