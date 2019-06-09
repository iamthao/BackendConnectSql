using System;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using Framework.DomainModel.Common;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using Framework.Utility;
using Newtonsoft.Json;
using QuickspatchWeb.Attributes;
using QuickspatchWeb.Models.Report;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Authentication;

namespace QuickspatchWeb.Controllers
{
    public partial class ReportController : Controller
    {
        private readonly ISystemPrintPdfService _systemPrintPdfService;
        private readonly IEmailHandler _emailHandler;
        private readonly IRequestService _requestService;
        private readonly string _savedPath;
        private readonly string _localPath;

        public ReportController(ISystemPrintPdfService systemPrintPdfService,
            IEmailHandler emailHandler,
            IAuthenticationService authenticationService, IRequestService requestService)
        {
            _systemPrintPdfService = systemPrintPdfService;
            _emailHandler = emailHandler;
            _requestService = requestService;

            _savedPath = ConfigurationManager.AppSettings["PlaceFileDownloadTemp"];
            if (string.IsNullOrEmpty(_savedPath))
            {
                throw new Exception("Missing config place contain file upload temp [PlaceFileDownloadTemp] in web.config");
            }
            _localPath = System.Web.HttpContext.Current.Server.MapPath(_savedPath);
            if (!Directory.Exists(_localPath))
            {
                Directory.CreateDirectory(_localPath);
            }
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Report, OperationAction = OperationAction.View)]
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Report, OperationAction = OperationAction.View)]
        public ActionResult ExportPdf(string courierId, string fromDate, string toDate, string displayName)
        {
            var queryInfo = new DriverReportQueryInfo
            {
                CourierId = Convert.ToInt32(courierId),
                FromDate = Convert.ToDateTime(fromDate),
                ToDate = Convert.ToDateTime(toDate),
                DisplayName = displayName.ToUpper()
            };
            var destPath = _localPath;
            var fileName = string.Format("DriverReport_{0}.pdf", DateTime.Now.ToString("yyyyMMddss"));
            destPath = Path.Combine(destPath, fileName);
            destPath = _systemPrintPdfService.ExportDriverReport(destPath, queryInfo);

            var fileInfo = new FileInfo(destPath);
            if (fileInfo.Exists)
            {
                return View(new DriverReportPdfViewModel { Path = _savedPath, FileName = fileInfo.Name });
            }
            throw new Exception("No Report");
        }


        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Request, OperationAction = OperationAction.View)]
        public ActionResult PrintPdfFile(string parameters, ReportType type)
        {
            string destPath = _localPath;
            string fileName = "";
            var email = "";
            var subject = "";
            switch (type)
            {
                case ReportType.DeliveryAgreement:
                    var paymentQuery = JsonConvert.DeserializeObject<RequestQueryInfo>(parameters);
                    email = "acbv@yahoo.com";
                    fileName = string.Format("DeliveryAgreement_{0}.pdf", DateTime.Now.ToString("yyyyMMddss"));
                    destPath = _systemPrintPdfService.ExportDeliveryAgreementReport(Path.Combine(destPath, fileName), paymentQuery);
                    break;
            }

            var fileInfo = new FileInfo(destPath);
            if (fileInfo.Exists)
            {
                return View(new PrintPdfFileViewModel { Path = _savedPath, FileName = fileInfo.Name, Email = email, EmailSubject = subject });
            }
            throw new Exception("No Report");
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Request, OperationAction = OperationAction.View)]
        public FileResult GetPdf(string filePath)
        {
            filePath = Server.MapPath(filePath);
            byte[] pdfByte = FileHelper.GetBytesFromFile(filePath);
            return File(pdfByte, "application/pdf");
        }


        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Tracking, OperationAction = OperationAction.View)]
        public JsonResult GetListRequestForReport(ReportQueryInfo queryInfo)
        {
            var courierId = queryInfo.CourierId;
            var fromDate = queryInfo.FromDate;
            var toDate = queryInfo.ToDate;

            var queryData = _requestService.GetListRequestForReport(courierId, fromDate, toDate);
            var clientsJson = Json(queryData, JsonRequestBehavior.AllowGet);
            return clientsJson;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Tracking, OperationAction = OperationAction.View)]
        public JsonResult GetHtmlReport(int courierId, DateTime fromDate, DateTime toDate,string displayName)
        {
            var content = _systemPrintPdfService.GetContentRequestReport(courierId, fromDate, toDate, displayName);
            return Json(new { Data = content, Error = "" }, JsonRequestBehavior.AllowGet);
        }
    }
}