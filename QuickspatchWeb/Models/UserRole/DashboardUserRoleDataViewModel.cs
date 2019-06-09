namespace QuickspatchWeb.Models.UserRole
{
    public class DashboardUserRoleDataViewModel : MasterfileViewModelBase<Framework.DomainModel.Entities.UserRole>
    {
        public override void MapFromClientParameters(MasterfileParameter parameters)
        {
            SharedViewModel = MapFromClientParameters<DashboardUserRoleShareViewModel>(parameters);
        }

        public override string PageTitle
        {
            get
            {
                return SharedViewModel.CreateMode ? "Create Role" : "Update Role";
            }
        }
    }
}