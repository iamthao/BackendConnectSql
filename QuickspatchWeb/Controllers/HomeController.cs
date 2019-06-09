using System;
using System.IO;
using ConfigValues;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using Framework.Service.Diagnostics;
using Framework.Utility;
using NLog.Internal;
using QuickspatchWeb.Attributes;
using QuickspatchWeb.Models.Home;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Authentication;
using System.Web.Mvc;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace QuickspatchWeb.Controllers
{
    public class HomeController : ApplicationControllerBase
    {
        private readonly IFranchiseeConfigurationService _franchiseeConfigurationService;
        
        public HomeController(IAuthenticationService authenticationService, IDiagnosticService diagnosticService, IFranchiseeConfigurationService franchiseeConfigurationService)
            : base(authenticationService, diagnosticService)
        {
            _franchiseeConfigurationService = franchiseeConfigurationService;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View)]
        public ActionResult Index()
        {
            var isQuickTour = false;

            if (ConstantValue.DeploymentMode != DeploymentMode.Camino)
            {
                var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();

                if (franchiseeConfiguration != null)
                {
                    isQuickTour = franchiseeConfiguration.IsQuickTour != null && (bool)franchiseeConfiguration.IsQuickTour;
                }
            }

            var model = new DashboardHomeIndexViewModel
            {
               IsQuickTour = false
            };
            return View(model);
        }

        public void ShowIsQuickTour(bool isQuickTour)
        {
            var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();
            if (franchiseeConfiguration != null){
                if (string.IsNullOrEmpty(franchiseeConfiguration.Address1))
                {
                    franchiseeConfiguration.Address1 = "N/A";
                }
                if (string.IsNullOrEmpty(franchiseeConfiguration.Zip))
                {
                    franchiseeConfiguration.Zip = "N/A";
                }
                if (string.IsNullOrEmpty(franchiseeConfiguration.City))
                {
                    franchiseeConfiguration.City = "N/A";
                }
                if (string.IsNullOrEmpty(franchiseeConfiguration.State))
                {
                    franchiseeConfiguration.State = "N/A";
                }
                franchiseeConfiguration.IsQuickTour = isQuickTour;
                _franchiseeConfigurationService.Update(franchiseeConfiguration);
            }
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View)]
        public ActionResult GetLogoContent()
        {
            var deploymentMode = ConfigurationManager.AppSettings["DeploymentMode"];
            if (deploymentMode != "camino")
            {
                var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();
                if (franchiseeConfiguration != null)
                {
                    if (franchiseeConfiguration.Logo != null && franchiseeConfiguration.Logo.Length > 0)
                    {
                        return new FileContentResult(franchiseeConfiguration.Logo, "image/jpeg");
                    }
                }
            }
            var filePath = Server.MapPath("~/content/quickspatch/img/logo.png");
            var file = new FileInfo(filePath);
            var fileBytes = new byte[file.Length];
            var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            fs.Read(fileBytes, 0, (int)file.Length);
            fs.Close();
            return new FileContentResult(fileBytes, "image/jpeg");
        }
    }
}