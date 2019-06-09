using Framework.DataAnnotations;
using Framework.Service.Translation;

namespace QuickspatchWeb.Models.Authentication
{
    public class DashboardAuthenticationSignInViewModel : ViewModelBase
    {
        public override string PageTitle
        {
            get
            {
                return SystemMessageLookup.GetMessage("SignInPageTitle");
            }
        }
        [LocalizeRequired]
        public string UserName { get; set; }
        [LocalizeRequired]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class DashboardForgotPasswordViewModel : ViewModelBase
    {
        public override string PageTitle
        {
            get
            {
                return "Forgot Password";
            }
        }
        [LocalizeRequired]
        public string UserName { get; set; }
    }
}