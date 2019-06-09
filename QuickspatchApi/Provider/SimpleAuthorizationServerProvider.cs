using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Consume.ServiceLayer.Interface;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Security;
using Framework.DomainModel.Interfaces;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using ServiceLayer.Interfaces;

namespace QuickspatchApi.Provider
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private const string QuickspatchWebClient = "QuickspatchWebClient";
        private const string QuickspatchMobileClient = "QuickspatchMobileClient";
        private readonly Dictionary<string, string> _listClientIdAndSecret = new Dictionary<string, string>
        {
            {QuickspatchWebClient, "QuickspatchSecretCode"},
            {QuickspatchMobileClient, "QuickspatchSecretCode"}
        };

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // validate client credentials
            string clientId, secret;
            if (context.TryGetBasicCredentials(out clientId, out secret))
            {
                if (_listClientIdAndSecret.ContainsKey(clientId) &&
                    String.Equals(_listClientIdAndSecret[clientId], secret, StringComparison.CurrentCultureIgnoreCase))
                {
                    context.Validated();
                }
            }
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            switch (context.ClientId)
            {
                case QuickspatchWebClient:
                {
                    context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
                    var franchisseService =
                        GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IFranchiseeTenantService)) as IFranchiseeTenantService;
                    FranchiseeTenant franchisee = null;
                    if (franchisseService != null)
                    {
                        franchisee = franchisseService.CheckFranchiseeWithNameAndLicenseKey(context.UserName, context.Password);

                        if (franchisee == null)
                        {
                            context.Rejected();
                            return;
                        }
                    }
                    if (franchisee == null)
                    {
                        context.Rejected();
                        return;
                    }
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    identity.AddClaim(new Claim(ClaimsDeclaration.IdClaim, franchisee.Id.ToString()));
                    identity.AddClaim(new Claim(ClaimsDeclaration.QuickspatchClientType, ((int)QuickspatchClientType.WebClient).ToString()));
                    // create metadata to pass on to refresh token provider
                    var props = new AuthenticationProperties(new Dictionary<string, string>
                    {
                        {"as:client_id", context.ClientId}
                    });
                    //var diagnosticService = new DiagnosticService();
                    var ticket = new AuthenticationTicket(identity, props);
                    context.Validated(ticket);
                    //bool b = context.Validated(ticket);
                    //diagnosticService.Debug(string.Format("Validated ticket with user{0}:{1}", context.UserName, b));
                    return;
                }
                case QuickspatchMobileClient:
                {
                    var franchiseeConfigurationService = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IFranchiseeConfigurationService)) as IFranchiseeConfigurationService;
                    var webApiConsumeUserService = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IWebApiConsumeUserService)) as IWebApiConsumeUserService;
                    
                    if (franchiseeConfigurationService!=null && webApiConsumeUserService != null)
                    {
                        var franchisee = franchiseeConfigurationService.FirstOrDefault();
                        var franchiseeData = new FranchisseNameAndLicenseDto()
                        {
                            LicenseKey   = franchisee.LicenseKey,
                            FranchiseeName = franchisee.Name
                        };

                        var objTokenStore = webApiConsumeUserService.GetToken(franchiseeData);

                        if (objTokenStore != null)
                        {
                            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] {"*"});
                            var userService =
                                GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof (IUserService))
                                    as
                                    IUserService;
                            User user = null;
                            //verify is expire or not

                            if (userService != null)
                            {
                                user = userService.GetUserByUserNameAndPass(context.UserName, context.Password);
                                if (user == null)
                                {
                                    context.Rejected();
                                    return;
                                }
                            }
                            if (user == null)
                            {
                                context.Rejected();
                                return;
                            }
                            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                            identity.AddClaim(new Claim(ClaimsDeclaration.IdClaim, user.Id.ToString()));
                            identity.AddClaim(new Claim(ClaimsDeclaration.QuickspatchClientType,
                                ((int) QuickspatchClientType.MobileClient).ToString()));

                            // create metadata to pass on to refresh token provider
                            var props = new AuthenticationProperties(new Dictionary<string, string>
                            {
                                {"as:client_id", context.ClientId}
                            });

                            var ticket = new AuthenticationTicket(identity, props);
                            context.Validated(ticket);
                            return;
                        }
                        else
                        {
                            context.Response.StatusCode = 99;
                            context.SetError("Invalid license");
                            //context.Rejected();
                            return;
                        }
                    }
                    return;
                }
                default:
                    return;
            }
        }

        public override async Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            string originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            string currentClient = context.ClientId;

            // enforce client binding of refresh token
            if (originalClient != currentClient)
            {
                context.Rejected();
                return;
            }

            // chance to change authentication ticket for refresh token requests
            var newId = new ClaimsIdentity(context.Ticket.Identity);
            newId.AddClaim(new Claim("newClaim", "refreshToken"));

            var newTicket = new AuthenticationTicket(newId, context.Ticket.Properties);
            context.Validated(newTicket);
        }
    }
}