using Framework.DataAnnotations;
using Framework.Service.Translation;

namespace QuickspatchWeb.Models.Authentication
{
    public class DashboardAuthenticationRestorePasswordViewModel : ViewModelBase
    {
        public override string PageTitle
        {
            get
            {
                return SystemMessageLookup.GetMessage("RestorePasswordPageTitle");
            }
        }
        [LocalizeRequired]
        [LocalizeEmailAddress]
        public string Email { get; set; }
    }
}