using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using Framework.Mapping;
using Framework.Service.Diagnostics;
using QuickspatchWeb.Attributes;
using QuickspatchWeb.Models;
using QuickspatchWeb.Models.FranchiseeModule;
using Newtonsoft.Json;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Authentication;

namespace QuickspatchWeb.Controllers
{
    public partial class FranchiseeModuleController : ApplicationControllerGeneric<FranchiseeModule, DashboardFranchiseeModuleDataViewModel>
    {
        private readonly IFranchiseeModuleService _franchiseeModuleService;
        private readonly IFranchiseeTenantService _franchiseeTenantService;
        private readonly IModuleService _moduleService;
        private readonly IGridConfigService _gridConfigService;

        public FranchiseeModuleController(IAuthenticationService authenticationService, IDiagnosticService diagnosticService, IGridConfigService gridConfigService, 
            IFranchiseeModuleService franchiseeModuleService,
            IFranchiseeTenantService franchiseeTenantService,
            IModuleService moduleService)
            : base(authenticationService, diagnosticService, franchiseeModuleService)
        {
            _franchiseeModuleService = franchiseeModuleService;
            _franchiseeTenantService = franchiseeTenantService;
            _moduleService = moduleService;
            _gridConfigService = gridConfigService;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeModule, OperationAction = OperationAction.View)]
        public JsonResult GetDataForGrid(QueryInfo queryInfo)
        {
            var queryData = _franchiseeModuleService.GetDataForGridMasterfile(queryInfo);
            var clientsJson = Json(queryData, JsonRequestBehavior.AllowGet);

            return clientsJson;
        }

        protected override IList<ViewColumnViewModel> GetViewColumns()
        {
            return new List<ViewColumnViewModel>
            {
                new ViewColumnViewModel
                {
                    ColumnOrder = 2,
                    ColumnWidth = 400,
                    Name = "Name",
                    Text = "Module Name",
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 2,
                    Name = "Command",
                    Text = " ",
                    ColumnWidth = 200,
                    Sortable = false,
                    ColumnJustification = GridColumnJustification.Left,
                    CustomTemplate = "userRoleGridCommandTemplate"
                }
            };
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeModule, OperationAction = OperationAction.Update)]
        public ActionResult Update(int id)
        {
            var franchisee = _franchiseeTenantService.GetById(id);
            var viewModel = new DashboardFranchiseeModuleDataViewModel
            {
                SharedViewModel = new DashboardFranchiseeModuleShareViewModel()
                {
                    Id = franchisee.Id,
                    FranchiseeName = franchisee.Name,
                    FranchiseeId = franchisee.Id
                }
            };

            return View(viewModel);
        }

        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeModule, OperationAction = OperationAction.Update)]
        public ActionResult Update(FranchiseeModuleParameter parameters)
        {
            var data = JsonConvert.DeserializeObject<List<DashboardFranchiseeModuleDataItem>>(parameters.ModuleData);
            var viewModel = MapFromClientParameters(parameters);
            var entity = _franchiseeTenantService.GetById(viewModel.SharedViewModel.Id);

            foreach (var franchiseeModule in entity.FranchiseeModule)
            {
                franchiseeModule.IsDeleted = true;
            }

            foreach (var item in data)
            {
                if (item.IsActive)
                {
                    entity.FranchiseeModule.Add(new FranchiseeModule()
                    {
                        ModuleId = item.ModuleId,
                        FranchiseeId = entity.Id
                    });
                }
            }

            byte[] lastModified = null;
            entity.FranchiseeContact = "nosave";//
            entity.PrimaryContactPhone = "1111111111";//
            lastModified = _franchiseeTenantService.Update(entity).LastModified;

            return Json(new { Error = string.Empty, Data = new { LastModified = lastModified } }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeModule, OperationAction = OperationAction.Delete)]
        public JsonResult Delete(int id)
        {
            MasterFileService.DeleteById(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}