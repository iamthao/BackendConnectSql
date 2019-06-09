
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IdentityModel.Services;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;
using Framework.Exceptions;
using Framework.Service.Diagnostics;
using Framework.Service.Translation;
using Framework.Web;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Authentication;
using System.Linq;
using System.Security.Claims;

namespace ServiceLayer.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private const int MaxSessionDuration = 540;
        private readonly IFranchiseeConfigurationRepository _franchiseeConfigurationRepository;
        public AuthenticationService(
                                     IQuickspatchHttpContext httpContext,
                                     IClaimsManager claimsManager, IFormAuthenticationService formAuthenticationService,
                                     ISessionIDManager sessionIdManager, IDiagnosticService diagnosticService, IUserRepository userRepository
                                    , ISystemEventService systemEventService, IFranchiseeConfigurationRepository franchiseeConfigurationRepository)
        {
            HttpContext = httpContext;
            FormAuthenticationService = formAuthenticationService;
            ClaimsManager = claimsManager;
            SessionIdManager = sessionIdManager;
            _diagnosticService = diagnosticService;
            _userRepository = userRepository;
            _systemEventService = systemEventService;
            _franchiseeConfigurationRepository = franchiseeConfigurationRepository;
        }
        private readonly IDiagnosticService _diagnosticService;
        public IClaimsManager ClaimsManager { get; set; }
        public IFormAuthenticationService FormAuthenticationService { get; private set; }
        public IQuickspatchHttpContext HttpContext { get; private set; }
        public ISessionIDManager SessionIdManager { get; private set; }
        private readonly IUserRepository _userRepository;
        private readonly ISystemEventService _systemEventService;
        public IQuickspatchPrincipal GetCurrentUser()
        {
            var principal = (HttpContext.User as IQuickspatchPrincipal);
            if (principal == null)
                FormAuthenticationService.SignOut();

            return principal;
        }



        public void SignOut()
        {
            //Nghiep Test Event User Login
            //var user = GetCurrentUser().User;
            //if (user != null)
            //{
            //    _systemEventService.Add(new SystemEvent
            //    {
            //        Description = string.Format(SystemMessageLookup.GetMessage("SystemEventLogOut"), "User", user.UserName)
            //    });
            //}
            FormAuthenticationService.SignOut();
            
            if (FederatedAuthentication.WSFederationAuthenticationModule != null)
            {
                FederatedAuthentication.WSFederationAuthenticationModule.SignOut();
                FederatedAuthentication.WSFederationAuthenticationModule.SignOut(true);
                FederatedAuthentication.SessionAuthenticationModule.SignOut();
            }

        }

        public bool SignIn(string userName, string password, bool rememberMe, string deploymentMode)
        {
            // encript pasword

            var claims = ClaimsManager.CreateClaims(userName, password).ToList();
            var user = ClaimsManager.ValidateQuickspatchUserLogin(claims);

            if (user == null || !user.IsQuickspatchUser)
            {

                var claimException = new InvalidClaimsException("InvalidUserAndPasswordText")
                {
                    QuickspatchUserName = (user != null) ? user.UserName : string.Empty
                };
                _diagnosticService.Error(SystemMessageLookup.GetMessage("InvalidUserAndPasswordText"));
                _diagnosticService.Error("UserName:" + userName);
                throw claimException;
            }
            if (!user.IsActive)
            {
                var claimException = new UserVisibleException("LoginWithInacticeUser");
                _diagnosticService.Error(SystemMessageLookup.GetMessage("LoginWithInacticeUser"));
                _diagnosticService.Error("UserName:" + userName);
                throw claimException;
            }
            if (deploymentMode != "Camino" && user.Courier != null)
            {
                var claimException = new UserVisibleException("LoginWithCourierUser");
                _diagnosticService.Error(SystemMessageLookup.GetMessage("LoginWithCourierUser"));
                _diagnosticService.Error("UserName:" + userName);
                throw claimException;
            }

            var principal = CreatePrincipalFromClaimsAndUser(user, claims);
            FormAuthenticationService.SignIn(principal, true, principal.AuthToken,
                                              DateTime.UtcNow.AddMinutes(MaxSessionDuration));
            return true;
        }
        private void AddCookie(string name, string value, int minutes)
        {
            var sessionCookie = new HttpCookie(name, value)
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(minutes)
            };
            System.Web.HttpContext.Current.Response.Cookies.Add(sessionCookie);
        }
        private void RemoveCookie(HttpCookie cookie, int minutes)
        {
            cookie.Expires = DateTime.UtcNow.AddMinutes(minutes);
            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
        }


        public User RestorePassword(string email, out string passwordRandom)
        {
            var failed = false;
            var validationResult = new List<ValidationResult>();
            User objUser = null;
            // Check email
            if (string.IsNullOrEmpty(email))
            {
                var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Email");
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }
            else if (
                !Regex.IsMatch(email,
                    @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z"))
            {
                var mess = string.Format(SystemMessageLookup.GetMessage("FieldInvalidText"), "Email");
                validationResult.Add(new ValidationResult(mess));
                failed = true;
            }
            else
            {
                objUser = _userRepository.FirstOrDefault(o => o.Email == email);
                if (objUser == null)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("FieldInvalidText"), "Email");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
            }
            var result = new BusinessRuleResult(failed, "", "RestorePassword", 0, null, "RestorePasswordRule") { ValidationResults = validationResult };
            if (failed)
            {
                // Give messages on every rule that failed
                throw new BusinessRuleException("BussinessGenericErrorMessageKey", new[] { result });
            }
            // Create password
            passwordRandom = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
            //var password = PasswordHelper.HashString(passwordRandom, objUser.UserName);
            // Update password to database
            //objUser.Password = password;
            _userRepository.Update(objUser);
            _userRepository.Commit();
            return objUser;
        }

        /// <summary>
        ///     Update principal for security threads.
        /// </summary>
        public void UpdatePrincipal(IQuickspatchPrincipal principal)
        {
            FormAuthenticationService.SetPrincipalCache(principal, principal.AuthToken, null);
        }

        protected IQuickspatchPrincipal CreatePrincipalFromClaimsAndUser(User user, List<Claim> claims)
        {
            var firstOrDefault = claims.FirstOrDefault(x => x.Type == ClaimsDeclaration.AuthenticationTypeClaimType);
            if (firstOrDefault != null)
            {
                var providerClaim = firstOrDefault.Value;
                var orDefault = claims.FirstOrDefault(x => x.Type == ClaimsDeclaration.NameClaimType);
                if (orDefault != null)
                {
                    var loginName = orDefault.Value;

                    var primaryIdentity = new QuickspatchIdentity(loginName, user.Id, providerClaim);

                    // create principal with primary identity
                    var returnPrincipal = new QuickspatchPrincipal(primaryIdentity)
                    {
                        AuthToken = SessionIdManager.CreateSessionID(null),
                        User = user
                    };

                    return returnPrincipal;
                }
            }
            return null;
        }

        
    }
}