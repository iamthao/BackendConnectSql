using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConfigValues;
using Framework.DomainModel.Common;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using Framework.DomainModel.ValueObject;
using Framework.Service.Diagnostics;
using Framework.Utility;
using QuickspatchWeb.Attributes;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Authentication;
using System.IO;
using System.Configuration;
using DocumentFormat.OpenXml.Office2013.Word;
using Framework.Service.Translation;

namespace QuickspatchWeb.Controllers
{
    public class CommonController : ApplicationControllerBase
    {
        private readonly IDiagnosticService _diagnosticService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ITempUploadFileService _tempUploadFileService;
        private readonly ISystemEventService _systemEventService;
        private readonly IStaticValueService _staticValueService;
        private readonly IGoogleService _googleService;
        private readonly IFranchiseeConfigurationService _franchiseeConfigurationService;
        private readonly IEmailHandler _emailHandler;
        
        public CommonController(IAuthenticationService authenticationService,
            IDiagnosticService diagnosticService, ITempUploadFileService tempUploadFileService, ISystemEventService systemEventService,
            IGoogleService googleService, IFranchiseeConfigurationService franchiseeConfigurationService , IEmailHandler emailHandler,
            IStaticValueService staticValueService)
            : base(authenticationService, diagnosticService, null)
        {
            _authenticationService = authenticationService;
            _diagnosticService = diagnosticService;
            _tempUploadFileService = tempUploadFileService;
            _systemEventService = systemEventService;
            _staticValueService = staticValueService;
            _tempUploadFileService.FilePath = "/Content/Upload/";
            _googleService = googleService;
            _franchiseeConfigurationService = franchiseeConfigurationService;
            _emailHandler = emailHandler;
        }
        [HttpGet]
        public JsonResult GenderLookup()
        {
            var data = XmlDataHelpper.Instance.GetData(XmlDataTypeEnum.Gender.ToString()).Select(o => new { DisplayName = o.Value, KeyId = o.Key });
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View)]
        public JsonResult CheckChangeTable()
        {
            var checkSum = _staticValueService.CheckChangeTable();
            return Json(new { Error = string.Empty, Data = checkSum }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult SaveFileUpload(HttpPostedFileBase file)
        {
            var fileName = _tempUploadFileService.SaveFile(file);
            return Json(new { file = fileName }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RemoveFileUpload(string file)
        {
            _tempUploadFileService.RemoveFile(file);
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLogoContent()
        {
            string filePath;
            FileInfo file = null;
            byte[] fileBytes = null;
            FileStream fs = null;

            filePath = Server.MapPath("~/Content/quickspatch/img/logo_o.svg");
            file = new FileInfo(filePath);
            fileBytes = new byte[file.Length];
            fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            fs.Read(fileBytes, 0, (int)file.Length);
            fs.Close();
            return new FileContentResult(fileBytes, "image/png");

        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View)]
        public JsonResult GetNotifyDecline(NotifyDeclineQueryInfo queryInfo)
        {
            if (ConstantValue.DeploymentMode == DeploymentMode.Camino)
            {
                var data = new List<NotifyDeclineGridVo>();
                return Json(new { Data = data, TotalRowCount = 0 }, JsonRequestBehavior.AllowGet);
            }
            var queryData = _systemEventService.GetNotifyDecline(queryInfo);
            var clientsJson = Json(queryData, JsonRequestBehavior.AllowGet);
            return clientsJson;
        }

        [HttpGet]
        public JsonResult TemplateTypeLookup(int mode)
        {
            var data = Enum.GetValues(typeof(TemplateType))
             .Cast<TemplateType>()
             .Select(o => new LookupItemVo { DisplayName = o.GetDescription(), KeyId = (int)o })
             .ToList();          
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDistanceGoogle(string prevToLat, string prevToLng,string currFromLat, string currFromLng)
        {
            var origin = new GoogleMapPoint { Lat = Convert.ToDouble(prevToLat), Lng = Convert.ToDouble(prevToLng) };
            var destination = new GoogleMapPoint { Lat = Convert.ToDouble(currFromLat), Lng = Convert.ToDouble(currFromLng) };

            var dataGoogle = _googleService.GetDistance(origin, destination);
            return Json(dataGoogle, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.AllowAnonymous]
        public JsonResult SendContactUs(string fullname, string email, string subject, string content)
        {
            //TODO: Issue Respone all error 403
            Response.Status = "200 OK";
            Response.StatusCode = 200;
            //TODO: Issue Respone all error 403
            var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();
            //send email           
            var fromEmail = AppSettingsReader.GetValue("EmailFrom", typeof(String)) as string;
            var displayName = AppSettingsReader.GetValue("EmailFromDisplayName", typeof(String)) as string;
            var emailContactUs = AppSettingsReader.GetValue("EmailContactUs", typeof(String)) as string;
            var weblink = AppSettingsReader.GetValue("Url", typeof(String)) as string;

            var emailContent = TemplateHelpper.FormatTemplateWithContentTemplate(
                TemplateHelpper.ReadContentFromFile(TemplateConfigFile.ContactUsTemplate, true),
                new
                {
                    fullname_t = fullname,
                    email_t = email,
                    subject_t = subject,
                    content_t = content,
                    web_link = weblink
                });
            var logo = franchiseeConfiguration != null ? franchiseeConfiguration.Logo : new byte[0];
            _emailHandler.SendEmailSsl(fromEmail, new[] { emailContactUs }, "Email from Contact Us",
                                    emailContent, logo, true, displayName);
            var clientsJson = Json(true, JsonRequestBehavior.AllowGet);
            return clientsJson;
        }
    }
}