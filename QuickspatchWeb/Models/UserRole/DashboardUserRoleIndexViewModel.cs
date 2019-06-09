namespace QuickspatchWeb.Models.UserRole
{
    public class DashboardUserRoleIndexViewModel : DashboardGridViewModelBase<Framework.DomainModel.Entities.UserRole>
    {
        public override string PageTitle
        {
            get
            {
                return "Roles";
            }
        }

        public bool CheckIsAppRole(int idRole)
        {
            return true;
        }
    }
}