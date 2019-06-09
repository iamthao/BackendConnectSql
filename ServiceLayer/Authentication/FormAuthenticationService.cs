using System;
using System.Web;
using System.Web.Caching;
using System.Web.Script.Serialization;
using Framework.DomainModel.Interfaces;
using ServiceLayer.Interfaces.Authentication;

namespace ServiceLayer.Authentication
{
    public class FormAuthenticationService : IFormAuthenticationService
    {
        public void SignOut()
        {
            var authCookie = HttpContext.Current.Request.Cookies[ClaimsDeclaration.AuthenticationCookie];
            if (authCookie != null)
            {
                RemoveCookie(authCookie, -1);
            }
        }

        public string AuthenticationCookieValue
        {
            get
            {
                // Retrieve cookie.
                var formsAuthCookie = HttpContext.Current.Request.Cookies[System.Web.Security.FormsAuthentication.FormsCookieName];

                if (formsAuthCookie != null)
                {
                    // An authentication cookie is available.
                    return formsAuthCookie.Value;
                }
                else
                {
                    // No authentication cookie is available.
                    return null;
                }
            }
        }

        public void SignIn(IQuickspatchPrincipal principal, bool rememberMe, string authToken, DateTime? expires)
        {
            SetAuthenticationCookie(principal, authToken, expires);
        }

        public void SetAuthenticationCookie(IQuickspatchPrincipal principal, string authToken, DateTime? expires)
        {
            var serializer = new JavaScriptSerializer();
            var authCookie = HttpContext.Current.Request.Cookies[ClaimsDeclaration.AuthenticationCookie];
            var issueDate = DateTime.UtcNow;

            if (authCookie != null)
            {
                expires = expires.HasValue ? expires : authCookie.Expires;

                //Remove existing cookies:
                RemoveCookie(authCookie, -1);
            }

            // Write session token. Retrieve maximum duration from the session returned from the session
            // controller. Set the cookie scope to the application path, and only the application path.
            if (expires != null)
            {
                var sessionCookie = new HttpCookie(ClaimsDeclaration.AuthenticationCookie, principal.AuthToken)
                {
                    HttpOnly = true,
                    Expires = (DateTime)expires
                };

                HttpContext.Current.Response.Cookies.Add(sessionCookie);
                SetPrincipalCache(principal, principal.AuthToken, sessionCookie.Expires);
            }
        }

        public void SetPrincipalCache(IQuickspatchPrincipal principal, string authKey, DateTime? expires)
        {
            if (HttpContext.Current.Cache[authKey] == null)
                HttpContext.Current.Cache.Insert(principal.AuthToken, principal, null, (DateTime)expires, Cache.NoSlidingExpiration);
            else
                HttpContext.Current.Cache[authKey] = principal;
        }

        private void RemoveCookie(HttpCookie cookie, int day)
        {
            cookie.Expires = DateTime.UtcNow.AddDays(day);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}