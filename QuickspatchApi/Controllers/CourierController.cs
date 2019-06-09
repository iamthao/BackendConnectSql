using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Consume.ServiceLayer.Interface;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using Framework.Mapping;
using Framework.Service.Diagnostics;
using Framework.Service.Translation;
using Framework.Utility;
using QuickspatchApi.Attributes;
using ServiceLayer.Interfaces;

namespace QuickspatchApi.Controllers
{
    [RoutePrefix("api/Courier")]
    public class CourierController : ApplicationControllerBase
    {
        private readonly IDiagnosticService _diagnosticService;
        private readonly ICourierService _courierService;
        private readonly IFranchiseeConfigurationService _franchiseeConfigurationService;
        private readonly IEmailHandler _emailHandler;
        private readonly IUserService _userService;
        private readonly IWebApiConsumeUserService _webApiConsumeUserService;
        private readonly IContactService _contactService;
        public CourierController(IDiagnosticService diagnosticService, ICourierService courierService, IUserService userService, IContactService contactService, IFranchiseeConfigurationService franchiseeConfigurationService, IEmailHandler emailHandler, IWebApiConsumeUserService webApiConsumeUserService)
            : base(diagnosticService, null)
        {
            _diagnosticService = diagnosticService;
            _courierService = courierService;
            _userService = userService;
            _contactService = contactService;
            _franchiseeConfigurationService = franchiseeConfigurationService;
            _emailHandler = emailHandler;
            _webApiConsumeUserService = webApiConsumeUserService;
        }

        [HttpPost]
        [Route("UpdateProfileInfo")]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.Update, QuickspatchClientType = QuickspatchClientType.MobileClient)]
        public IHttpActionResult UpdateProfileInfo(CourierDto userData)
        {
            var courier = _courierService.GetById(userData.Id);
            //courier = userData.MapPropertiesToInstance(courier);
            courier.CarNo = userData.CarNo;
            courier.User.Email = userData.User.Email;
            courier.User.MobilePhone = userData.User.MobilePhone.RemoveFormatPhone();
            var newPassword = PasswordHelper.HashString(userData.User.Password, courier.User.UserName);
            courier.User.Password = newPassword;
            var result = _courierService.Update(courier);
            var courierDto = result.MapTo<CourierDto>();
            courierDto.User.Password = userData.User.Password;
            return Ok(courierDto);
        }

        [HttpPost]
        [Route("GetProfileInfo")]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View, QuickspatchClientType = QuickspatchClientType.MobileClient)]
        public IHttpActionResult GetProfileInfo(UsernameAndPasswordDto userData)
        {
            var courier = _courierService.GetCourierWithUsernameAndPassword(userData);
            var courierDto = courier.MapTo<CourierDto>();
            courierDto.IsSameImei = courier.Imei == userData.Imei;
            courierDto.Contacts = _contactService.ListAll().OrderBy(o=>o.Name).Select(o => o.MapTo<ContactDto>()).ToList();
            return Ok(courierDto);
        }

        [HttpPost]
        [Route("CheckConnection")]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View, QuickspatchClientType = QuickspatchClientType.MobileClient)]
        public IHttpActionResult CheckConnection(CheckConnectDto checkConnectDto)
        {
            var franchisee = _franchiseeConfigurationService.FirstOrDefault();
            var franchiseeData = new FranchisseNameAndLicenseDto()
            {
                LicenseKey = franchisee.LicenseKey,
                FranchiseeName = franchisee.Name
            };

            var objTokenStore = _webApiConsumeUserService.GetToken(franchiseeData);
            if (objTokenStore != null)
            {
                int checkConnectValue = _courierService.CheckConnection(checkConnectDto);
                return Ok(new DtoBase { Id = checkConnectValue });
            }

                return Ok(new DtoBase {Id = 10});
            
         
        }

        [HttpPost]
        [Route("LogOut")]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View, QuickspatchClientType = QuickspatchClientType.MobileClient)]
        public IHttpActionResult LogOut(DtoBase courierId)
        {
            _courierService.LogOut(courierId.Id);
            return Ok();
        }

        [HttpPost]
        [Route("SendEmailForgotPassword")]
        public IHttpActionResult SendEmailForgotPassword(ForgotPasswordDto dtoForgotPassword)
        {
            var dtoBase = new DtoBase();
            const string randomPassword = "123456";
            var user = _userService.FirstOrDefault(o => o.Email == dtoForgotPassword.Email);
            if (user != null)
            {
                var courier = _courierService.GetById(user.Id);
                if (courier != null)
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
                    _emailHandler.SendEmail(fromEmail, new[] { user.Email }, SystemMessageLookup.GetMessage("SubjectToSendEmailForCreateUser"),
                                            emailContent, true, displayName);
                    dtoBase.Id = 1;
                }
            }
            else
            {
                dtoBase.Id = 2;
            }
            return Ok(dtoBase);
            //return Json(new { }, JsonRequestBehavior.AllowGet);
        }
    }
}