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
using QuickspatchWeb.Models.Module;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Authentication;
using Framework.Utility;
using QuickspatchWeb.Services.Interface;

namespace QuickspatchWeb.Controllers
{
    public class ModuleController : ApplicationControllerGeneric<Module, DashboardModuleDataViewModel>
    {
        private readonly IModuleService _moduleService;
        private readonly IGridConfigService _gridConfigService;
        private readonly IRenderViewToString _renderViewToString;

        public ModuleController(IAuthenticationService authenticationService, IDiagnosticService diagnosticService, IGridConfigService gridConfigService,
            IRenderViewToString renderViewToString, IModuleService moduleService)
            : base(authenticationService, diagnosticService, moduleService)
        {
            _moduleService = moduleService;
            _gridConfigService = gridConfigService;
            _renderViewToString = renderViewToString;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Module, OperationAction = OperationAction.View)]
        public ActionResult Index()
        {
            var viewModel = new DashboardModuleIndexViewModel();
            Func<GridViewModel> gridViewModel = () => new GridViewModel
            {
                GridId = "ModuleGrid",
                ModelName = "Module",
                DocumentTypeId = (int)DocumentTypeKey.Module,
                GridInternalName = "Module",
                UseDeleteColumn = false,
                PopupWidth = 500,
                PopupHeight = 150
            };

            viewModel.GridViewModel = BuildGridViewModel(_gridConfigService, gridViewModel);
            return View(viewModel);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Module, OperationAction = OperationAction.View)]
        public JsonResult GetDataForGrid(QueryInfo queryInfo)
        {
            var queryData = _moduleService.GetDataForGridMasterfile(queryInfo);
            var clientsJson = Json(queryData, JsonRequestBehavior.AllowGet);

            return clientsJson;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Module, OperationAction = OperationAction.View)]
        public JsonResult GetModuleDocumentTypeOperationsGrid(int moduleId)
        {
            var queryData = _moduleService.GetModuleDocumentTypeOperationsGrid(moduleId);
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
                    Text = "Module Name",
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 2,
                    ColumnWidth = 150,
                    Name = "Command",
                    Text = " ",
                    ColumnJustification = GridColumnJustification.Left,
                    CustomTemplate = "commandModule",
                    Mandatory = true,
                    Sortable = false
                }
            };
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Module, OperationAction = OperationAction.Add)]
        public ActionResult Create()
        {
            var viewModel = new DashboardModuleDataViewModel
            {
                SharedViewModel = new DashboardModuleShareViewModel
                {
                    CreateMode = true
                }
            };
            return View(viewModel);
        }

        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Module, OperationAction = OperationAction.Add)]
        public int Create(ModuleParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);
            var entity = viewModel.MapTo<Module>();
            var savedEntity = MasterFileService.Add(entity);

            return savedEntity.Id;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Module, OperationAction = OperationAction.Update)]
        public ActionResult Update(int id)
        {
            var viewModel = GetMasterFileViewModel(id);
            return View(viewModel);
        }

        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Module, OperationAction = OperationAction.Update)]
        public ActionResult Update(ModuleParameter parameters)
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
        
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Module, OperationAction = OperationAction.Delete)]
        public JsonResult Delete(int id)
        {
            MasterFileService.DeleteById(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Module, OperationAction = OperationAction.View)]
        public JsonResult ExportExcel(List<ColumnModel> gridColumns, QueryInfo queryInfo)
        {
            return base.ExportExcelMasterfile(gridColumns, queryInfo);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Module, OperationAction = OperationAction.View)]
        public FileResult DownloadExcelFile(string fileName)
        {
            return base.DownloadExcelMasterFile(fileName);
        }
    }
}