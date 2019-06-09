using System.Collections.Generic;

namespace QuickspatchWeb.Models.FranchiseeModule
{
    public class DashboardFranchiseeModuleShareViewModel : DashboardSharedViewModel
    {
        public string FranchiseeName { get; set; }
        public int FranchiseeId { get; set; }
    }

    public class DashboardFranchiseeModuleDataItem
    {
        public int Id { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public bool IsActive { get; set; }
    }
}