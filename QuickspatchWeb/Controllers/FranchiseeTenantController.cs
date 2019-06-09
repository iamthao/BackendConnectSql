using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using Framework.Mapping;
using Framework.Service.Diagnostics;
using Framework.Utility;
using QuickspatchWeb.Attributes;
using QuickspatchWeb.Models;
using QuickspatchWeb.Models.FranchiseeTenant;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Authentication;
using QuickspatchWeb.Services.Interface;

namespace QuickspatchWeb.Controllers
{
    public class FranchiseeTenantController : ApplicationControllerGeneric<FranchiseeTenant, DashboardFranchiseeTenantDataViewModel>
    {
        private readonly IFranchiseeTenantService _franchiseeTenantService;
        private readonly IGridConfigService _gridConfigService;
        private readonly IFranchiseeConfigurationService _franchiseeConfigurationService;
        private readonly IResizeImage _resizeImage;
        private readonly IRenderViewToString _renderViewToString;

        public FranchiseeTenantController(IAuthenticationService authenticationService, IDiagnosticService diagnosticService, 
                                        IGridConfigService gridConfigService, IFranchiseeConfigurationService franchiseeConfigurationService,
                                    IRenderViewToString renderViewToString,
                                        IResizeImage resizeImage,IFranchiseeTenantService franchiseeTenantService)
            : base(authenticationService, diagnosticService, franchiseeTenantService)
        {
            _franchiseeTenantService = franchiseeTenantService;
            _gridConfigService = gridConfigService;
            _franchiseeConfigurationService = franchiseeConfigurationService;
            _resizeImage = resizeImage;
            _renderViewToString = renderViewToString;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeTenant, OperationAction = OperationAction.View)]
        public ActionResult Index()
        {
            var viewModel = new DashboardFranchiseeTenantIndexViewModel();
            Func<GridViewModel> gridViewModel = () => new GridViewModel
            {
                GridId = "FranchiseeTenantGrid",
                ModelName = "FranchiseeTenant",
                DocumentTypeId = (int)DocumentTypeKey.FranchiseeTenant,
                GridInternalName = "FranchiseeTenant",
                UseDeleteColumn = false,
                PopupWidth = 800,
                PopupHeight = 550
            };

            viewModel.GridViewModel = BuildGridViewModel(_gridConfigService, gridViewModel);
            return View(viewModel);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeTenant, OperationAction = OperationAction.View)]
        public JsonResult GetDataForGrid(QueryInfo queryInfo)
        {
            var queryData = _franchiseeTenantService.GetDataForGridMasterfile(queryInfo);
            var clientsJson = Json(queryData, JsonRequestBehavior.AllowGet);

            return clientsJson;
        }

        protected override IList<ViewColumnViewModel> GetViewColumns()
        {
            return new List<ViewColumnViewModel>
            {
                new ViewColumnViewModel
                {
                    ColumnOrder = 1,
                    ColumnWidth = 400,
                    Name = "Name",
                    Text = "Franchisee Name",
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 1,
                    ColumnWidth = 400,
                    Name = "IsActive",
                    Text = "Active",
                    ColumnJustification = GridColumnJustification.Left,
                    CustomTemplate = "franchiseeTenantTemplate"
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 1,
                    ColumnWidth = 150,
                    Name = "Command",
                    Text = " ",
                    ColumnJustification = GridColumnJustification.Left,
                    CustomTemplate = "commandFranchisee",
                    Mandatory = true,
                    Sortable = false
                }
            };
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeTenant, OperationAction = OperationAction.Add)]
        public ActionResult Create()
        {
            //var currentUser = AuthenticationService.GetCurrentUser().User;
            var randomGuid = Guid.NewGuid().ToString("D").ToUpper();

            var viewModel = new DashboardFranchiseeTenantDataViewModel
            {
                SharedViewModel = new DashboardFranchiseeTenantShareViewModel
                {
                    CreateMode = true,
                    LicenseKey = randomGuid
                }
            };
            return View(viewModel);
        }

        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeTenant, OperationAction = OperationAction.Add)]
        public int Create(FranchiseeTenantParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);//
            var franchiseeTenant = viewModel.MapTo<FranchiseeTenant>();//
            var franchiseeConfiguration = viewModel.MapTo<FranchiseeConfiguration>();//
            var sharViewModel = viewModel.SharedViewModel as DashboardFranchiseeTenantShareViewModel;//
            var logoFilePath = "";

            if (sharViewModel != null)
            {
                if (!String.IsNullOrEmpty(sharViewModel.Logo))
                {
                    if (!sharViewModel.Logo.Contains("data:image/jpg;base64"))
                    {
                        logoFilePath = Server.MapPath(sharViewModel.Logo);
                        franchiseeConfiguration.Logo = _resizeImage.ResizeImageByHeight(logoFilePath, 40);
                    }
                }
                else
                {
                    franchiseeConfiguration.Logo = Convert.FromBase64String("NULL");
                }

                if (String.IsNullOrEmpty(sharViewModel.Address2))
                {
                    franchiseeConfiguration.Address2 = "";
                }
                if (String.IsNullOrEmpty(sharViewModel.PrimaryContactFax))
                {
                    franchiseeConfiguration.PrimaryContactFax = "";
                }
                if (String.IsNullOrEmpty(sharViewModel.PrimaryContactEmail))
                {
                    franchiseeConfiguration.PrimaryContactEmail = "";
                }
                if (String.IsNullOrEmpty(sharViewModel.PrimaryContactCellNumber))
                {
                    franchiseeConfiguration.PrimaryContactCellNumber = "";
                }

            }
            var idInsert = _franchiseeTenantService.SetupFranchisee(franchiseeTenant, franchiseeConfiguration);//
            return idInsert;

        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeTenant, OperationAction = OperationAction.Update)]
        public ActionResult Update(int id)
        {
            FranchiseeTenant outFranchiseeTenant;
            FranchiseeConfiguration outFranchiseeConfiguration;
            _franchiseeTenantService.GetFranchiseeDataForUpdate(id, out outFranchiseeTenant, out outFranchiseeConfiguration);
            var viewModel = new DashboardFranchiseeTenantDataViewModel();
            if (outFranchiseeTenant != null)
            {
                viewModel = outFranchiseeTenant.MapPropertiesToInstance(viewModel);
            }
            if (outFranchiseeConfiguration != null)
            {
                viewModel = outFranchiseeConfiguration.MapPropertiesToInstance(viewModel);
            }
            return View(viewModel);
        }

        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeTenant, OperationAction = OperationAction.Update)]
        public ActionResult Update(FranchiseeTenantParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);

            byte[] lastModified = null;
            var franchiseeTenant = _franchiseeTenantService.GetById(viewModel.SharedViewModel.Id);
            var mappedFranchiseeTenant = viewModel.MapPropertiesToInstance(franchiseeTenant);
            var franchiseeConfiguration = viewModel.MapTo<FranchiseeConfiguration>();

            var sharViewModel = viewModel.SharedViewModel as DashboardFranchiseeTenantShareViewModel;
            var logoFilePath = "";
            if (ModelState.IsValid)
            {                              
                if (sharViewModel != null)
                {
                    if (!String.IsNullOrEmpty(sharViewModel.Logo))
                    {
                        if (!sharViewModel.Logo.Contains("data:image/jpg;base64"))
                        {
                            logoFilePath = Server.MapPath(sharViewModel.Logo);
                            franchiseeConfiguration.Logo = System.IO.File.ReadAllBytes(logoFilePath);//_resizeImage.ResizeImageByHeight(logoFilePath, 40);
                        }
                        
                    }
                    else
                    {
                        franchiseeConfiguration.Logo = Convert.FromBase64String("NULL");
                    }
                    if (String.IsNullOrEmpty(sharViewModel.Address2))
                    {
                        franchiseeConfiguration.Address2 = "";
                    }
                    if (String.IsNullOrEmpty(sharViewModel.PrimaryContactFax))
                    {
                        franchiseeConfiguration.PrimaryContactFax = "";
                    }
                    if (String.IsNullOrEmpty(sharViewModel.PrimaryContactEmail))
                    {
                        franchiseeConfiguration.PrimaryContactEmail = "";
                    }
                    if (String.IsNullOrEmpty(sharViewModel.PrimaryContactCellNumber))
                    {
                        franchiseeConfiguration.PrimaryContactCellNumber = "";
                    }
                    
                }
                lastModified = _franchiseeTenantService.UpdateFranchisee(mappedFranchiseeTenant, franchiseeConfiguration);

            }
            
            return Json(new { Error = string.Empty, Data = new { LastModified = lastModified } }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeTenant, OperationAction = OperationAction.Delete)]
        public JsonResult Delete(int id)
        {
            MasterFileService.DeleteById(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeTenant, OperationAction = OperationAction.View)]
        public JsonResult GenerateLicenseKey()
        {
            //var currentUser = AuthenticationService.GetCurrentUser().User;
            var randomGuid = Guid.NewGuid().ToString("D").ToUpper();
            return Json(new { Data = randomGuid }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeTenant, OperationAction = OperationAction.Update)]
        public JsonResult DeactivateFranchisee(int id)
        {
            var franchisee = _franchiseeTenantService.DeactivateFranchisee(id);
            return Json(new { Data = franchisee.Id }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeTenant, OperationAction = OperationAction.Update)]
        public JsonResult ActivateFranchisee(int id)
        {
            var franchisee = _franchiseeTenantService.ActivateFranchisee(id);
            return Json(new { Data = franchisee.Id }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeTenant, OperationAction = OperationAction.View)]
        public JsonResult ExportExcel(List<ColumnModel> gridColumns, QueryInfo queryInfo)
        {
            return base.ExportExcelMasterfile(gridColumns, queryInfo);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeTenant, OperationAction = OperationAction.View)]
        public FileResult DownloadExcelFile(string fileName)
        {
            return base.DownloadExcelMasterFile(fileName);
        }
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeTenant, OperationAction = OperationAction.View)]
        public JsonResult GetListIndustry()
        {
            var queryData = _franchiseeTenantService.GetListIndustry();
            var clientsJson = Json(queryData, JsonRequestBehavior.AllowGet);
            return clientsJson;
        }
    }
}