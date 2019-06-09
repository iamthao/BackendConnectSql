namespace QuickspatchWeb.Models.Home
{
    public class DashboardHomeIndexViewModel : ViewModelBase
    {
        public bool IsQuickTour { get; set; }

        public override string PageTitle
        {
            get
            {
                return "Quickspatch Website";
            }
        }

       
    }
}