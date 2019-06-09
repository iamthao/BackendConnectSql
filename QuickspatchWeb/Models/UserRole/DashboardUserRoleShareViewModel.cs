namespace QuickspatchWeb.Models.UserRole
{
    public class DashboardUserRoleShareViewModel : DashboardSharedViewModel
    {
        public string Name { get; set; }
        public bool CheckAll { get; set; }
        public string UserRoleFunctionData { get; set; }
        public string AppRoleName { get; set; }
    }
}