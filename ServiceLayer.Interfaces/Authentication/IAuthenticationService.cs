using Framework.DomainModel.Entities;
using Framework.DomainModel.Interfaces;

namespace ServiceLayer.Interfaces.Authentication
{
    public interface IAuthenticationService
    {
        IQuickspatchPrincipal GetCurrentUser();
        void SignOut();
        bool SignIn(string userName, string password, bool rememberMe, string deploymentMode);
        User RestorePassword(string email, out string passwordRandom);
        void UpdatePrincipal(IQuickspatchPrincipal principal);
        
    }
}
