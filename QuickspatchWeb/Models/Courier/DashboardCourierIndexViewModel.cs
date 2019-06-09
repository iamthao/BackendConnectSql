namespace QuickspatchWeb.Models.Courier
{
    public class DashboardCourierIndexViewModel : DashboardGridViewModelBase<Framework.DomainModel.Entities.Courier>
    {
        public override string PageTitle
        {
            get
            {
                return "Courier";
            }
        }

        public string WebsiteUrl { get; set; }
    }
}