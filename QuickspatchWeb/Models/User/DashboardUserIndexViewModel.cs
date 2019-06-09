namespace QuickspatchWeb.Models.User
{
    public class DashboardUserIndexViewModel : DashboardGridViewModelBase<Framework.DomainModel.Entities.User>
    {
        public override string PageTitle
        {
            get
            {
                return "User";
            }
        }

        public string WebsiteUrl { get; set; }
    }
}