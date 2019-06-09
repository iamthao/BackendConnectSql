using System;
using Framework.DomainModel.Interfaces;

namespace ServiceLayer.Interfaces.Authentication
{
    public interface IFormAuthenticationService
    {
        string AuthenticationCookieValue { get; }
        void SignOut();
        void SignIn(IQuickspatchPrincipal principal, bool rememberMe, string authToken, DateTime? expires);
        void SetAuthenticationCookie(IQuickspatchPrincipal principal, string authToken, DateTime? expires);
        void SetPrincipalCache(IQuickspatchPrincipal principal, string authKey, DateTime? expires);
    }
}