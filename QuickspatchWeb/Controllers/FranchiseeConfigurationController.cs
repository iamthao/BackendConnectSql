
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using Consume.ServiceLayer.Interface;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using Framework.Mapping;
using Framework.Service.Diagnostics;
using Framework.Utility;
using Microsoft.Ajax.Utilities;
using QuickspatchWeb.Attributes;
using QuickspatchWeb.Models;
using QuickspatchWeb.Models.FranchiseeConfiguration;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Authentication;
using System.Transactions;
using System.Web;
using System.Web.Helpers;
using Framework.DomainModel.ValueObject;
using Framework.Service.Translation;
using Newtonsoft.Json;
using System.Configuration;
using System.Diagnostics;
using System.Web.WebPages;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using DocumentFormat.OpenXml.Presentation;


namespace QuickspatchWeb.Controllers
{
    public partial class FranchiseeConfigurationController : ApplicationControllerGeneric<FranchiseeConfiguration, DashboardFranchiseeConfigurationDataViewModel>
    {
        private readonly IFranchiseeConfigurationService _franchiseeConfigurationService;
        private readonly IGridConfigService _gridConfigService;
        private readonly IResizeImage _resizeImage;
        private readonly IWebApiConsumeUserService _webApiUserService;
        private readonly IDiagnosticService _diagnosticService;
        private readonly IUserService _userService;
        private readonly IWebApiPaymentService _webApiPaymentService;
        private readonly IEmailHandler _emailHandler;
        private readonly IContactService _contactService;
        private readonly ILocationService _locationService;
        private readonly ISystemConfigurationService _systemConfigurationService;

        public FranchiseeConfigurationController(IAuthenticationService authenticationService, IDiagnosticService diagnosticService,
            IResizeImage resizeImage, IGridConfigService gridConfigService, IFranchiseeConfigurationService franchiseeConfigurationService,
            IFranchiseeTenantService franchiseeTenantService, IUserService userService, IWebApiPaymentService webApiPaymentService,
            IWebApiConsumeUserService webApiUserService, IEmailHandler emailHandler, IContactService contactService, ILocationService locationService,
            ISystemConfigurationService systemConfigurationService)
            : base(authenticationService, diagnosticService, franchiseeConfigurationService)
        {

            _resizeImage = resizeImage;
            _franchiseeConfigurationService = franchiseeConfigurationService;
            _gridConfigService = gridConfigService;
            _webApiUserService = webApiUserService;
            _diagnosticService = diagnosticService;
            _userService = userService;
            _webApiPaymentService = webApiPaymentService;
            _emailHandler = emailHandler;
            _contactService = contactService;
            _locationService = locationService;
            _systemConfigurationService = systemConfigurationService;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeConfiguration, OperationAction = OperationAction.View)]
        public ActionResult Index(int? tabIndex)
        {
            return View();
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeConfiguration, OperationAction = OperationAction.Update)]
        public ActionResult Update(int id)
        {
            var viewModel = GetMasterFileViewModel(id);
            return View(viewModel);
        }

        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeConfiguration, OperationAction = OperationAction.Update)]
        public ActionResult Update(FranchiseeConfigurationParameter parameters)
        {
            using (var tran = new TransactionScope())
            {
                var viewModel = MapFromClientParameters(parameters);

                byte[] lastModified = null;

                var id = 0;
                var name = "";
                var logoFilePath = "";
                if (ModelState.IsValid)
                {
                    var sharViewModel = viewModel.SharedViewModel as DashboardFranchiseeConfigurationShareViewModel;


                    var entity = MasterFileService.GetById(viewModel.SharedViewModel.Id);
                    var mappedEntity = viewModel.MapPropertiesToInstance(entity);
                    var mappedFranchiseeTernantDto = viewModel.MapTo<FranchiseeTernantDto>();

                    if (sharViewModel != null)
                    {
                        if (!String.IsNullOrEmpty(sharViewModel.Logo))
                        {
                            if (!sharViewModel.Logo.Contains("data:image/jpg;base64"))
                            {
                                logoFilePath = Server.MapPath(sharViewModel.Logo);
                                mappedEntity.Logo = System.IO.File.ReadAllBytes(logoFilePath);
                                //_resizeImage.ResizeImageByHeight(logoFilePath, 40);
                            }
                        }
                    }
                    //gan value do bo bot filed trong _shared.cshtml
                    mappedEntity.FranchiseeContact = mappedEntity.Name;
                    mappedEntity.OfficePhone = mappedEntity.PrimaryContactPhone;
                    mappedEntity.FaxNumber = mappedEntity.PrimaryContactFax;

                    lastModified = MasterFileService.Update(mappedEntity).LastModified;
                    id = mappedEntity.Id;
                    name = mappedEntity.Name;

                    _webApiUserService.UpdateFranchiseeConfig(mappedFranchiseeTernantDto);
                    //Thread.Sleep(1000);
                }
                if (lastModified != null)
                {
                    if (!String.IsNullOrEmpty(logoFilePath) && System.IO.File.Exists(logoFilePath))
                    {
                        System.IO.File.Delete(logoFilePath);
                    }
                }
                tran.Complete();
                return Json(new { Error = string.Empty, Data = new { id, name, LastModified = lastModified } },
                    JsonRequestBehavior.AllowGet);

            }
        }
               
                          
    }

}