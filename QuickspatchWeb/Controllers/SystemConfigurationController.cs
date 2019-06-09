using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web.Mvc;
using Consume.ServiceLayer.Interface;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using Framework.DomainModel.ValueObject;
using Framework.Mapping;
using Framework.Service.Diagnostics;
using Framework.Service.Translation;
using Framework.Utility;
using QuickspatchWeb.Attributes;
using QuickspatchWeb.Models;
using QuickspatchWeb.Models.User;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Authentication;
using QuickspatchWeb.Services.Interface;
using Framework.Exceptions;
using Newtonsoft.Json;
using QuickspatchWeb.Models.SystemConfiguration;
using QuickspatchWeb.Models.Template;

namespace QuickspatchWeb.Controllers
{
    public class SystemConfigurationController : ApplicationControllerGeneric<SystemConfiguration, DashboardSystemConfigurationDataViewModel>
    {
        private readonly ISystemConfigurationService _systemConfigurationService;
        private readonly IGridConfigService _gridConfigService;

        public SystemConfigurationController(IAuthenticationService authenticationService, IGridConfigService gridConfigService,
            IDiagnosticService diagnosticService, ISystemConfigurationService systemConfigurationService)
            : base(authenticationService, diagnosticService, systemConfigurationService)
        {
            _systemConfigurationService = systemConfigurationService;
            _gridConfigService = gridConfigService;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View)]
        public ActionResult Index()
        {
            var viewModel = new DashboardSystemConfigurationIndexViewModel();
            Func<GridViewModel> gridViewModel = () => new GridViewModel
            {
                GridId = "SystemConfigurationGrid",
                ModelName = "SystemConfiguration",
                DocumentTypeId = (int)DocumentTypeKey.None,
                GridInternalName = "SystemConfiguration",
                UseDeleteColumn = true,
                CanDeleteRecord = false,
                CanAddNewRecord = false,
                CanExportGrid = false,
                CanSearchGrid = false,
                PopupWidth =700,
                PopupHeight = 270
            };
            viewModel.GridViewModel = BuildGridViewModel(_gridConfigService, gridViewModel);
            return View(viewModel);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View)]
        public JsonResult GetDataForGrid(QueryInfo queryInfo)
        {
            return base.GetDataForGridMasterFile(queryInfo);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeConfiguration, OperationAction = OperationAction.View)]
        public JsonResult GetListSystemConfiguration(QueryInfo queryInfo)
        {
            var listConfiguration = _systemConfigurationService.GetListSystemConfiguration();
            var clientsJson = Json(listConfiguration, JsonRequestBehavior.AllowGet);
            return clientsJson;
        }

        protected override IList<ViewColumnViewModel> GetViewColumns()
        {
            return new List<ViewColumnViewModel>
            {
                new ViewColumnViewModel
                {
                    ColumnOrder = 1,
                    ColumnWidth = 200,
                    Name = "Name",
                    Text = "Name",
                    ColumnJustification = GridColumnJustification.Left
                }
                ,new ViewColumnViewModel
                {
                    ColumnOrder = 2,
                    ColumnWidth = 200,
                    Name = "Value",
                    Text = "Value",
                    ColumnJustification = GridColumnJustification.Left
                }
            };
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.Add)]
        public ActionResult Create()
        {
            var viewModel = new DashboardSystemConfigurationDataViewModel
            {
                SharedViewModel = new DashboardSystemConfigurationShareViewModel
                {
                    CreateMode = true
                }
            };
            return View(viewModel);
        }

        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.Add)]
        public int Create(SystemConfigurationParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);
            var entity = viewModel.MapTo<SystemConfiguration>();
            var savedEntity = MasterFileService.Add(entity);
            return savedEntity.Id;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.Update)]
        public ActionResult Update(int id)
        {
            var data = _systemConfigurationService.GetById(id);
            //(DashboardSystemConfigurationDataViewModel)viewModel.SharedViewModel.
            var viewModel = new DashboardSystemConfigurationDataViewModel
            {
                SharedViewModel = new DashboardSystemConfigurationShareViewModel()
                {
                    Id =  data.Id,
                    Name = data.Name,
                    Value = data.Value,
                    DataType = data.DataType,
                    DataTypeId = (int)data.DataType,
                    SystemConfigType =  data.SystemConfigType,
                    SystemConfigTypeId = (int)data.SystemConfigType,
                }
            };
            return View(viewModel);
        }

        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.Update)]
        public ActionResult Update(SystemConfigurationParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);

            byte[] lastModified = null;

            if (ModelState.IsValid)
            {
                var entity = MasterFileService.GetById(viewModel.SharedViewModel.Id);
                var mappedEntity = viewModel.MapPropertiesToInstance(entity);
                lastModified = MasterFileService.Update(mappedEntity).LastModified;
            }

            return Json(new { Error = string.Empty, Data = new { LastModified = lastModified } }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.Delete)]
        public JsonResult Delete(int id)
        {
            MasterFileService.DeleteById(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }    
    }
}