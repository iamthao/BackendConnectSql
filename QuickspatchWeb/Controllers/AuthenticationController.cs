using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Common;
using ConfigValues;
using Consume.ServiceLayer.Interface;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;
using Framework.Exceptions;
using Framework.Service.Diagnostics;
using Framework.Service.Translation;
using Framework.Utility;
using System.Web.Caching;
using System.Web.Mvc;
using Newtonsoft.Json;
using QuickspatchWeb.Attributes;
using QuickspatchWeb.Models.Authentication;
using QuickspatchWeb.Models.User;
using ServiceLayer.Common;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Authentication;

namespace QuickspatchWeb.Controllers
{
    public class AuthenticationController : ApplicationControllerBase
    {
        private readonly IDiagnosticService _diagnosticService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly ICourierService _courierService;
        private readonly IEmailHandler _emailHandler;
        private readonly IWebApiConsumeUserService _webApiUserService;
        private readonly IFranchiseeConfigurationService _franchiseeConfigurationService;
        private readonly IFranchiseeTenantService _franchiseeTenantService;

        public AuthenticationController(IAuthenticationService authenticationService,
                                        IDiagnosticService diagnosticService, IUserService userService, ICourierService courierService,
            IWebApiConsumeUserService webApiUserService, IFranchiseeConfigurationService franchiseeConfigurationService, IFranchiseeTenantService franchiseeTenantService)
            : base(authenticationService, diagnosticService, null)
        {
            _authenticationService = authenticationService;
            _diagnosticService = diagnosticService;
            _userService = userService;
            _webApiUserService = webApiUserService;
            _courierService = courierService;
            _franchiseeConfigurationService = franchiseeConfigurationService;
            _franchiseeTenantService = franchiseeTenantService;
            _emailHandler = new EmailHandler();
        }
        public ActionResult SignOut()
        {
            //Sign out from authentication
            _authenticationService.SignOut();
            return RedirectToAction("SignIn");
        }
        [AllowAnonymous]
        public ActionResult SignIn(string returnUrl)
        {
            if (ConstantValue.DeploymentMode == DeploymentMode.Camino)
            {
                var viewModel = new DashboardAuthenticationSignInViewModel();
                return View(viewModel);
            }
            var configuration = _franchiseeConfigurationService.FirstOrDefault(o => true);
            if (configuration != null)
            {
                var viewModel = new DashboardAuthenticationSignInViewModel();
                return View(viewModel);
            }
            return RedirectToAction("Waiting");
        }

        [AllowAnonymous]
        public ActionResult Waiting()
        {
            return View();
        }

        private bool SignInToWebApi(FranchisseNameAndLicenseDto objFranchiseeAndLicense, string keyAuthentication)
        {
            //var claimExceptiona = new InvalidClaimsException("InvalidLicenseKey", keyAuthentication)
            //{
            //    QuickspatchUserName = string.Empty
            //};
            //throw claimExceptiona;
            // Get token
            if (ConstantValue.DeploymentMode == DeploymentMode.Camino)
            {
                return true;
            }

            // Get token
            var objTokenStore = _webApiUserService.GetToken(objFranchiseeAndLicense);
            if (objTokenStore == null)
            {
                var claimException = new InvalidClaimsException("InvalidLicenseKey", keyAuthentication)
                {
                    QuickspatchUserName = string.Empty
                };
                throw claimException;
            }
            if (!string.IsNullOrEmpty(objTokenStore.AccessToken))
            {
                // Store token to cookie
                var accessTokenCookie = new HttpCookie(ClaimsDeclaration.TokenClaim, objTokenStore.AccessToken)
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddDays(7)
                };
                if (HttpContext.Response.Cookies[ClaimsDeclaration.TokenClaim] != null)
                {
                    HttpContext.Response.Cookies.Remove(ClaimsDeclaration.TokenClaim);
                }
                HttpContext.Response.Cookies.Add(accessTokenCookie);
                var refreshTokenCookie = new HttpCookie(ClaimsDeclaration.RefreshTokenClaim, objTokenStore.RefreshToken)
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddYears(1)
                };
                if (HttpContext.Response.Cookies[ClaimsDeclaration.RefreshTokenClaim] != null)
                {
                    HttpContext.Response.Cookies.Remove(ClaimsDeclaration.RefreshTokenClaim);
                }
                HttpContext.Response.Cookies.Add(refreshTokenCookie);
            }


            return false;
        }

        [HttpPost]
        public JsonResult QuickspatchSignIn(string userName, string password, bool rememberMe, string returnUrl)
        {
            var isCamino = true;
            // Check if system is the franchisse, login to the web api to verify the license
           
            if (ConstantValue.DeploymentMode == DeploymentMode.Franchisee)
            {
                isCamino = false;
                var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();
                var objFranchiseeAndLicense = new FranchisseNameAndLicenseDto
                {
                    //FranchiseeName = ConstantValue.FranchiseeName,
                    //LicenseKey = ConstantValue.LicenseKey
                    FranchiseeName = franchiseeConfiguration!=null?franchiseeConfiguration.Name:"",
                    LicenseKey = franchiseeConfiguration != null ? franchiseeConfiguration.LicenseKey : "",
                };
                //Get Key Authentication for exeption Licence Invalid.
                var keyAuthentication = GetKeyAuthentication(userName, password);
                SignInToWebApi(objFranchiseeAndLicense, keyAuthentication);
                // Get list module for franchisee
                var franchiseeData = _webApiUserService.GetModuleForFranchisee(objFranchiseeAndLicense);
                MenuExtractData.Instance.ModuleForFranchisee = franchiseeData;
                MenuExtractData.Instance.NumberOfCourier = franchiseeData.NumberOfCourier;
            }

            password = PasswordHelper.HashString(password, userName);
            _authenticationService.SignIn(userName, password, rememberMe, ConstantValue.DeploymentMode.ToString());

            return Json(new { isCamino }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get key Authentication for case Licence Invalid.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private string GetKeyAuthentication(string userName, string password)
        {
            var user = _userService.GetUserByUserNameAndPass(userName, password);
            if (user == null)
            {
                var claimException = new InvalidClaimsException("InvalidUserAndPasswordText")
                {
                    QuickspatchUserName = userName
                };
                throw claimException;
            }
            var passPhrase = ConfigurationManager.AppSettings["PassPhrase"];
            if (string.IsNullOrEmpty(passPhrase))
            {
                throw new Exception("Missing config PassPhrase");
            }
            var obj = new {UserName = userName, Password = password};
            var data = JsonConvert.SerializeObject(obj);
            var encryptData = EncryptHelper.Encrypt(data, passPhrase);
            var bas64EncodeData = EncryptHelper.Base64Encode(encryptData);
            return bas64EncodeData;
        }

        private string ModifyReturnUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
                return "/";
            return returnUrl;

        }

        //Restore Password
        public ActionResult RestorePassword()
        {
            var viewModel = new DashboardAuthenticationRestorePasswordViewModel();
            return View(viewModel);
        }
        [HttpPost]
        public JsonResult RestorePassword(string email)
        {

            var user = _userService.FirstOrDefault(o => o.Email == email);
            if (user != null)
            {

                var webLink = AppSettingsReader.GetValue("Url", typeof(String)) as string;
                    var urlSignIn = webLink + "/Authentication/SignIn";
                    var imgSrc = webLink + "/Content/quickspatch/img/logo-o.svg";

                    var urlChangePass = webLink + "/Authentication/ChangeRestorePassword?code=" + PasswordHelper.HashString(user.Id.ToString(), user.UserName);
                    var fromEmail = AppSettingsReader.GetValue("EmailFrom", typeof(String)) as string;
                    var displayName = AppSettingsReader.GetValue("EmailFromDisplayName", typeof(String)) as string;
                    var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();
                    var franchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "";
                    var emailContent = TemplateHelpper.FormatTemplateWithContentTemplate(
                        TemplateHelpper.ReadContentFromFile(TemplateConfigFile.RestorePassword, true),
                        new
                        {
                            img_src = imgSrc,
                            full_name = Framework.Utility.CaculatorHelper.GetFullName(user.FirstName, user.MiddleName, user.LastName),
                            web_link = webLink,
                            user_name = user.UserName,
                            url_change_pass = urlChangePass,
                            franchisee_Name = franchiseeName,
                            url_sign_in = urlSignIn
                        });
                    // send email
                    var logo = franchiseeConfiguration != null ? franchiseeConfiguration.Logo : new byte[0];
                    _emailHandler.SendEmailSsl(fromEmail, new[] { user.Email }, SystemMessageLookup.GetMessage("SubjectToSendEmailForCreateUser"),
                                            emailContent,logo, true, displayName);
                
            }
            else
            {
                throw new UserVisibleException("EmailInvalidText");
            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        //change restore password user
        [AllowAnonymous]
        public ActionResult ChangeRestorePassword(string code)
        {
            var user = _userService.GetUserFromHashStringPasswordAndUsername(code);

            var viewModel = new ChangePasswordViewModel
            {
                Id = user != null ? user.Id : 0,
                Username = user != null ? user.UserName : ""
            };
            return View(viewModel);
        }
        
        //save restore password user
        [AllowAnonymous]
        public JsonResult SaveChangeRestorePassword(string param)
        {
            var data = JsonConvert.DeserializeObject<ChangePasswordViewModel>(param);
            if (data != null)
            {
                var id = data.Id;
                var user = _userService.GetById(id);
                if (user != null)
                {
                    
                        var newPassword = PasswordHelper.HashString(data.NewPassword, user.UserName);
                        user.Password = newPassword;
                        _userService.Update(user);
                        return Json(true, JsonRequestBehavior.AllowGet);
                   
                }
            }
            return Json(new { Error = SystemMessageLookup.GetMessage("ChangePasswordError") }, JsonRequestBehavior.AllowGet);
        }
        //
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            const string randomPassword = "123456";
            var user = _userService.FirstOrDefault(o => o.Email == email);
            if (user != null)
            {
                var courier = _courierService.GetById(user.Id);
                if (courier != null)
                {
                    var webLink = AppSettingsReader.GetValue("Url", typeof(String)) as string;
                    var imgSrc = webLink + "/Content/quickspatch/img/logo-o.svg";
                    var urlChangePass = webLink + "/Authentication/ChangePasswordCourier?code=" + PasswordHelper.HashString(courier.Id.ToString(), courier.User.UserName);
                    var fromEmail = AppSettingsReader.GetValue("EmailFrom", typeof(String)) as string;
                    var displayName = AppSettingsReader.GetValue("EmailFromDisplayName", typeof(String)) as string;
                    var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();
                    var franchiseeName = franchiseeConfiguration!=null?franchiseeConfiguration.Name:"";
                    var emailContent = Framework.Utility.TemplateHelpper.FormatTemplateWithContentTemplate(
                        TemplateHelpper.ReadContentFromFile(TemplateConfigFile.CreateCourierEmailTemplate, true),
                        new
                        {
                            img_src = imgSrc,
                            full_name = Framework.Utility.CaculatorHelper.GetFullName(courier.User.FirstName, courier.User.MiddleName, courier.User.LastName),
                            web_link = webLink,
                            user_name = courier.User.UserName,
                            password = randomPassword,
                            url_change_pass = urlChangePass,
                            franchisee_Name = franchiseeName
                        });
                    // send email
                    var logo = franchiseeConfiguration != null ? franchiseeConfiguration.Logo : new byte[0];
                    _emailHandler.SendEmailSsl(fromEmail, new[] { courier.User.Email }, SystemMessageLookup.GetMessage("SubjectToSendEmailForCreateUser"),
                                            emailContent,logo, true, displayName);
                }
                else
                {
                    user.Password = PasswordHelper.HashString(randomPassword, user.UserName);
                    _userService.Update(user);

                    var webLink = AppSettingsReader.GetValue("Url", typeof(String)) as string;
                    var urlSignIn = webLink + "/Authentication/SignIn";
                    var imgSrc = webLink + "/Content/quickspatch/img/logo-o.svg";
                    var urlChangePass = webLink + "/Authentication/ChangePassword?code=" + PasswordHelper.HashString(user.Id.ToString(), user.UserName);
                    var fromEmail = AppSettingsReader.GetValue("EmailFrom", typeof(String)) as string;
                    var displayName = AppSettingsReader.GetValue("EmailFromDisplayName", typeof(String)) as string;
                    var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();
                    var franchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "";
                    var emailContent = TemplateHelpper.FormatTemplateWithContentTemplate(
                        TemplateHelpper.ReadContentFromFile(TemplateConfigFile.RestorePasswordTemplate, true),
                        new
                        {
                            img_src = imgSrc,
                            full_name = Framework.Utility.CaculatorHelper.GetFullName(user.FirstName, user.MiddleName, user.LastName),
                            web_link = webLink,
                            user_name = user.UserName,
                            password = randomPassword,
                            url_change_pass = urlChangePass,
                            franchisee_Name = franchiseeName,
                            url_sign_in = urlSignIn
                        });
                    // send email
                    var logo = franchiseeConfiguration != null ? franchiseeConfiguration.Logo : new byte[0];
                    _emailHandler.SendEmailSsl(fromEmail, new[] { user.Email }, SystemMessageLookup.GetMessage("SubjectToSendEmailForCreateUser"),
                                            emailContent,logo, true, displayName);
                }
            }
            else
            {
                throw new UserVisibleException("EmailInvalidText");
            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult ChangePassword(string code)
        {
            var user = _userService.GetUserFromHashStringPasswordAndUsername(code);

            var viewModel = new ChangePasswordViewModel
            {
                Id = user != null ? user.Id : 0,
                Username=user!=null?user.UserName:""
            };
            return View(viewModel);
        }
        [AllowAnonymous]
        public ActionResult ChangePasswordCourier(string code)
        {
            var user = _userService.GetUserFromHashStringPasswordAndUsername(code);

            var viewModel = new ChangePasswordViewModel
            {
                Id = user != null ? user.Id : 0,
                Username = user != null ? user.UserName : ""
            };
            return View(viewModel);
        }
        [AllowAnonymous]
        public JsonResult SaveChangePassword(string param)
        {
            var data = JsonConvert.DeserializeObject<ChangePasswordViewModel>(param);
            if (data != null)
            {
                var id = data.Id;
                var user = _userService.GetById(id);
                if (user != null)
                {
                    var oldPassword = PasswordHelper.HashString(data.CurrentPassword, user.UserName);
                    if (user.Password.Equals(oldPassword))
                    {
                        var newPassword = PasswordHelper.HashString(data.NewPassword, user.UserName);
                        user.Password = newPassword;
                        _userService.Update(user);
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { Error = SystemMessageLookup.GetMessage("CurrentPasswordInvalid") }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { Error = SystemMessageLookup.GetMessage("ChangePasswordError") }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult GetDisplayLabel()
        {
            var courier = _franchiseeTenantService.GetDisplayNameForCourier();
            return Json(new { data = courier }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult GetDisplayAlertExtended()
        {
            if (ConstantValue.DeploymentMode == DeploymentMode.Camino)
            {
                return Json(new { data = false }, JsonRequestBehavior.AllowGet);
            }
            var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();
            var objFranchiseeAndLicense = new FranchisseNameAndLicenseDto
            {
                FranchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "",
                LicenseKey = franchiseeConfiguration != null ? franchiseeConfiguration.LicenseKey : ""
            };
            var check = false;
            var info = _webApiUserService.GetInfoFranchiseeNoToken(objFranchiseeAndLicense);

            double totalDay = 0;
            if (info != null && info.AlertExtendedPackage != null)
            {
                if (info.NextBillingDate != null)
                {
                    totalDay = Math.Ceiling((info.NextBillingDate.GetValueOrDefault() - DateTime.UtcNow).TotalDays);                   
                }              
                check = info.AlertExtendedPackage == true;
            }
            return Json(new { data = check, totaldDay = totalDay }, JsonRequestBehavior.AllowGet);
        }
    }
}