using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;

using Consume.ServiceLayer.Interface;
using DocumentFormat.OpenXml.Office2013.PowerPoint.Roaming;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Framework.Service.Diagnostics;
using Framework.Utility;
using QuickspatchWeb.Models.FranchiseeConfiguration;
using Newtonsoft.Json;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Authentication;
using System.Configuration;

namespace QuickspatchWeb.Controllers
{
    public class LicenceExtensionController : ApplicationControllerGeneric<FranchiseeConfiguration, DashboardFranchiseeConfigurationDataViewModel>
    {
        private readonly IFranchiseeConfigurationService _franchiseeConfigurationService;
        private readonly IUserService _userService;
        private readonly IWebApiConsumeUserService _webApiUserService;
        public LicenceExtensionController(IAuthenticationService authenticationService, IDiagnosticService diagnosticService,
            IFranchiseeConfigurationService franchiseeConfigurationService, IUserService userService, IWebApiConsumeUserService webApiUserService)
            : base(authenticationService, diagnosticService, franchiseeConfigurationService)
        {
            _franchiseeConfigurationService = franchiseeConfigurationService;
            _userService = userService;
            _webApiUserService = webApiUserService;
        }

        public ActionResult Index(string keyCode)
        {
            if (string.IsNullOrEmpty(keyCode))
            {
                return Redirect("/");
            }
            var password = string.Empty;
            var userName = GetKeyAuthenticationDecrypt(keyCode, ref password);
            var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();
            var objFranchiseeAndLicense = new FranchisseNameAndLicenseDto
            {
                FranchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "",
                LicenseKey = franchiseeConfiguration != null ? franchiseeConfiguration.LicenseKey : ""
            };
            var franchiseeTenantPackageInfo = _webApiUserService.GetPackageCurrentId(objFranchiseeAndLicense);
            //TODO: franchiseeTenantPackageInfo.Active == false update franchiseeTenantPackageInfo.Active == true when deploy
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password) || franchiseeTenantPackageInfo == null || franchiseeTenantPackageInfo.Active)
            {
                return Redirect("/");
            }
            return View(new LicenceExtensionData { KeyCode = keyCode, UserName = userName });
        }


        public ActionResult Active(string keyCode, int packageId)
        {
            //TODO: Issue Respone all error 403
            Response.Status = "200 OK";
            Response.StatusCode = 200;
            //TODO: Issue Respone all error 403

            var productKey = ConfigurationManager.AppSettings["ProductKey"];
            var secretKey = ConfigurationManager.AppSettings["SecretKey"];
            var url = ConfigurationManager.AppSettings["Url"];
            var isRecurrence = ConfigurationManager.AppSettings["IsRecurrence"];
            var recurrenceInterval = ConfigurationManager.AppSettings["RecurrenceInterval"];
            var passPhrase = ConfigurationManager.AppSettings["PassPhrase"];
            var paymentUrl = ConfigurationManager.AppSettings["PaymentUrl"];
            if (string.IsNullOrEmpty(productKey) || string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(url) ||
                string.IsNullOrEmpty(isRecurrence) || string.IsNullOrEmpty(recurrenceInterval) || string.IsNullOrEmpty(passPhrase))
            {
                throw new Exception("Missing configuration ProductKey or SecretKey or Url or IsRecurrence or RecurrenceInterval or PassPhrase");
            }

            var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();
            var objFranchiseeAndLicense = new FranchisseNameAndLicenseDto
            {
                FranchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "",
                LicenseKey = franchiseeConfiguration != null ? franchiseeConfiguration.LicenseKey : ""
            };
            //TODO: franchiseeTenantPackageInfo.Active == false update franchiseeTenantPackageInfo.Active == true when deploy
            var franchiseeTenantPackageInfo = _webApiUserService.GetPackageCurrentId(objFranchiseeAndLicense);
            if (franchiseeTenantPackageInfo == null || franchiseeTenantPackageInfo.Active == true)
            {
                return Redirect("/");
            }

            var registerPaymentDto = new PaymentInfoDto();
            registerPaymentDto.AccountNumber = franchiseeTenantPackageInfo.AccountNumber;
            registerPaymentDto.RequestId = _webApiUserService.GetRequestCurrentId(objFranchiseeAndLicense);
            registerPaymentDto.ProductKey = productKey;
            registerPaymentDto.SecretKey = secretKey;
            registerPaymentDto.ReturnUrl = url + "LicenceExtension/PaySuccess";
            registerPaymentDto.CancelUrl = url + "LicenceExtension?keyCode=" + keyCode;
            registerPaymentDto.IsRecurrence = int.Parse(isRecurrence);
            registerPaymentDto.RecurrenceInterval = int.Parse(recurrenceInterval);
            //0: new; 1: change package; 2: change paymentInfo
            registerPaymentDto.TransactionType = 1;
            registerPaymentDto.StartDate = DateTime.UtcNow;

            if (franchiseeTenantPackageInfo.Amount.GetValueOrDefault() > CaculatorHelper.GetPricePackage(packageId))
            {
                var packageInfo = _webApiUserService.GetPackageCurrentNoToken(objFranchiseeAndLicense);
                var packageHistory = new PackageHistoryDto();
                packageHistory.StartDate = DateTime.UtcNow;
                if (packageId % 2 == 0)
                {
                    packageHistory.EndDate = DateTime.UtcNow.AddMonths(1);
                }
                else
                {
                    packageHistory.EndDate = DateTime.UtcNow.AddMonths(12);
                }

                packageHistory.OldPackageId = franchiseeTenantPackageInfo.PackageId;
                packageHistory.PackageId = packageId;
                packageHistory.FranchiseeTenantId = franchiseeTenantPackageInfo.Id;
                packageHistory.RequestId = 0;
                packageHistory.IsApply = true;
                packageHistory.Amount = franchiseeTenantPackageInfo.Amount.GetValueOrDefault() -
                                        CaculatorHelper.GetPricePackage(packageId);
                packageHistory.NextBillingDate = packageHistory.EndDate;
                packageHistory.PackageNextBillingDate = packageId;

                var isSuccessAddPackage = _webApiUserService.AddPackageHistoryNoToken(packageHistory);
                bool isSuccessUpdateFranchisee = _webApiUserService.UpdateFranchiseeTenantLicenceExtentsion(objFranchiseeAndLicense);
                if (isSuccessUpdateFranchisee && isSuccessAddPackage)
                {
                    return RedirectToAction("Success", "LicenceExtension");
                }

                return RedirectToAction("Error", "LicenceExtension");

            }

            registerPaymentDto.TrialAmount = CaculatorHelper.GetPricePackage(packageId) -
                                             franchiseeTenantPackageInfo.Amount.GetValueOrDefault();

            registerPaymentDto.Items = new List<RegisterProduct>
            {
                new RegisterProduct { ItemId = 1, ItemName = CaculatorHelper.GetNamePackage(packageId), ItemQuantity = 1, ItemPrice = CaculatorHelper.GetPricePackage(packageId)}
            };

            var textdate = DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/ | " + DateTime.Now.Hour + "/" + DateTime.Now.Minute + "/" + DateTime.Now.Second;
            registerPaymentDto.AdditionInfo = JsonConvert.SerializeObject(new { PackageId = packageId, DateTimeSend = textdate });

            var user = _userService.FirstOrDefault(o => o.UserRoleId == 1);

            if (user != null)
            {
                registerPaymentDto.FirstName = user.FirstName;
                registerPaymentDto.MiddleName = user.MiddleName;
                registerPaymentDto.LastName = user.LastName;
            }

            if (franchiseeConfiguration != null)
            {
                registerPaymentDto.Email = franchiseeConfiguration.PrimaryContactEmail;
                registerPaymentDto.Address1 = franchiseeConfiguration.Address1;
                registerPaymentDto.Address2 = franchiseeConfiguration.Address2;
                registerPaymentDto.Zip = franchiseeConfiguration.Zip;
                registerPaymentDto.City = franchiseeConfiguration.City;
                registerPaymentDto.State = franchiseeConfiguration.State;
            }

            var data = JsonConvert.SerializeObject(registerPaymentDto);
            var result = EncryptHelper.Encrypt(data, passPhrase);
            var encodeUrl = HttpUtility.UrlEncode(result);
            var encodeBase64 = EncryptHelper.Base64Encode(encodeUrl);

            var returnData = new { PaymentUrl = paymentUrl, Data = encodeBase64 };

            return Json(returnData, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// Get key Authentication for case Licence Invalid.
        /// </summary>
        /// <param name="keyCode"></param>
        /// <returns></returns>
        private string GetKeyAuthenticationDecrypt(string keyCode, ref string password)
        {
            var passPhrase = ConfigurationManager.AppSettings["PassPhrase"];
            if (string.IsNullOrEmpty(passPhrase))
            {
                throw new Exception("Missing config PassPhrase");
            }
            var bas64EncodeData = EncryptHelper.Base64Decode(keyCode);
            var decryptData = EncryptHelper.Decrypt(bas64EncodeData, passPhrase);

            var obj = JsonConvert.DeserializeObject<dynamic>(decryptData);
            if (DynamicExtensions.IsPropertyExist(obj, "UserName") && DynamicExtensions.IsPropertyExist(obj, "Password"))
            {
                password = obj.Password;
                return obj.UserName;
            }
            return string.Empty;
        }


        public ActionResult PaySuccess(string data)
        {
            var passPhrase = ConfigurationManager.AppSettings["PassPhrase"];

            if (string.IsNullOrEmpty(passPhrase))
            {
                throw new Exception("Missing configuration PassPhrase");
            }
            var base64EncodedBytes = Convert.FromBase64String(data);
            var decode = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            decode = HttpUtility.UrlDecode(decode);
            var decrypt = EncryptHelper.Decrypt(decode, passPhrase);
            var model = JsonConvert.DeserializeObject<PaymentViewModel>(decrypt);


            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.AdditionInfo))
                {
                    // additionInfo is info of packingeId
                    var additionInfo = JsonConvert.DeserializeObject<dynamic>(model.AdditionInfo);
                    if (CaculatorHelper.IsPropertyExist(additionInfo, "PackageId"))
                    {
                        model.PackageId = (int?)additionInfo.PackageId;
                    }

                }
                var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();
                var objFranchiseeAndLicense = new FranchisseNameAndLicenseDto
                {
                    FranchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "",
                    LicenseKey = franchiseeConfiguration != null ? franchiseeConfiguration.LicenseKey : ""
                };

                var packageInfo = _webApiUserService.GetPackageCurrentNoToken(objFranchiseeAndLicense);
                var info = _webApiUserService.GetInfoFranchiseeNoToken(objFranchiseeAndLicense);
                if (packageInfo != null && info != null)
                {
                    var packageHistory = new PackageHistoryDto();
                    packageHistory.StartDate = DateTime.UtcNow;
                    if (model.PackageId % 2 == 0)
                    {
                        packageHistory.EndDate = DateTime.UtcNow.AddMonths(1);
                    }
                    else
                    {
                        packageHistory.EndDate = DateTime.UtcNow.AddMonths(12);
                    }

                    packageHistory.OldPackageId = packageInfo.PackageId;
                    packageHistory.PackageId = model.PackageId != null ? (int)model.PackageId : 0;
                    packageHistory.FranchiseeTenantId = info.Id;
                    packageHistory.RequestId = model.RequestId != null ? (int)model.RequestId : 0;
                    packageHistory.IsApply = true;
                    var isSuccessAddPackage = _webApiUserService.AddPackageHistoryNoToken(packageHistory);
                    bool isSuccessUpdateFranchisee = _webApiUserService.UpdateFranchiseeTenantLicenceExtentsion(objFranchiseeAndLicense);
                    if (isSuccessUpdateFranchisee && isSuccessAddPackage)
                    {
                        return RedirectToAction("Success", "LicenceExtension");
                    }
                }
            }
            return RedirectToAction("Error", "LicenceExtension");
        }
        // return page when change package success
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult Success()
        {
            //TODO: Issue Respone all error 403
            Response.Status = "200 OK";
            Response.StatusCode = 200;
            //TODO: Issue Respone all error 403
            return View();
        }

        [System.Web.Mvc.AllowAnonymous]
        public ActionResult Package(string keyCode)
        {
            var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();
            var objFranchiseeAndLicense = new FranchisseNameAndLicenseDto
            {
                FranchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "",
                LicenseKey = franchiseeConfiguration != null ? franchiseeConfiguration.LicenseKey : ""
            };
            var franchiseeTenantPackageInfo = _webApiUserService.GetPackageCurrentId(objFranchiseeAndLicense);
            if (franchiseeTenantPackageInfo == null || franchiseeTenantPackageInfo.Active)
            {
                return Redirect("/");
            }
            return View(new LicenceExtensionData { KeyCode = keyCode, UserName = string.Empty, PackageId = franchiseeTenantPackageInfo.PackageId });
        }
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult Close(int? id)
        {
            return View(id.GetValueOrDefault());
        }
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult Error()
        {
            return View();
        }

        [System.Web.Mvc.AllowAnonymous]
        public JsonResult GetDescripttionPackage(int packageId)
        {
            //TODO: Issue Respone all error 403
            Response.Status = "200 OK";
            Response.StatusCode = 200;
            //TODO: Issue Respone all error 403
            var descripttion = CaculatorHelper.GetDescriptionPackage(packageId);
            var clientsJson = Json(descripttion, JsonRequestBehavior.AllowGet);
            return clientsJson;
        }

        [System.Web.Mvc.AllowAnonymous]
        public ActionResult ContactUs(string keyCode)
        {
            return View(new LicenceExtensionData { KeyCode = keyCode });
        }

        [System.Web.Mvc.AllowAnonymous]
        public JsonResult SendMessage(string fname, string email, string subject, string message)
        {
            //TODO: Issue Respone all error 403
            Response.Status = "200 OK";
            Response.StatusCode = 200;
            //TODO: Issue Respone all error 403
            bool check = EmailValidateHelper.IsValidEmail(email);
            //send email
            var clientsJson = Json(check, JsonRequestBehavior.AllowGet);
            return clientsJson;
        }

    }
}