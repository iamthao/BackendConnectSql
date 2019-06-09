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
using QuickspatchWeb.Models.FranchiseeConfiguration;
using QuickspatchWeb.Models.Template;

namespace QuickspatchWeb.Controllers
{
    public class TemplateController : ApplicationControllerGeneric<Template, DashboardTemplateDataViewModel>
    {
        private readonly ITemplateService _templateService;
        private readonly IGridConfigService _gridConfigService;

        public TemplateController(IAuthenticationService authenticationService, IGridConfigService gridConfigService,
            IDiagnosticService diagnosticService, ITemplateService templateService)
            : base(authenticationService, diagnosticService, templateService)
        {
            _templateService = templateService;
            _gridConfigService = gridConfigService;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View)]
        public ActionResult Index()
        {
            var viewModel = new DashboardTemplateIndexViewModel();
            Func<GridViewModel> gridViewModel = () => new GridViewModel
            {
                GridId = "TemplateGrid",
                ModelName = "Template",
                DocumentTypeId = (int)DocumentTypeKey.None,
                GridInternalName = "Template",
                UseDeleteColumn = true,
                CanDeleteRecord = false,
                CanAddNewRecord = false,
                CanExportGrid = false,
                PopupWidth =800,
                PopupHeight = 450
            };
            viewModel.GridViewModel = BuildGridViewModel(_gridConfigService, gridViewModel);
            return View(viewModel);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View)]
        public JsonResult GetDataForGrid(QueryInfo queryInfo)
        {
            return base.GetDataForGridMasterFile(queryInfo);
        }

        protected override IList<ViewColumnViewModel> GetViewColumns()
        {
            return new List<ViewColumnViewModel>
            {
                new ViewColumnViewModel
                {
                    ColumnOrder = 1,
                    ColumnWidth = 400,
                    Name = "Title",
                    Text = "Title",
                    ColumnJustification = GridColumnJustification.Left
                }
                ,new ViewColumnViewModel
                {
                    ColumnOrder = 2,
                    ColumnWidth = 100,
                    Name = "TemplateType",
                    Text = "Type",
                    ColumnJustification = GridColumnJustification.Left
                }
            };
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.Add)]
        public ActionResult Create()
        {
            var viewModel = new DashboardTemplateDataViewModel
            {
                SharedViewModel = new DashboardTemplateShareViewModel
                {
                    CreateMode = true
                }
            };
            return View(viewModel);
        }

        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.Add)]
        public int Create(TemplateParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);
            var entity = viewModel.MapTo<Template>();
            var savedEntity = MasterFileService.Add(entity);
            return savedEntity.Id;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.Update)]
        public ActionResult Update(int id)
        {
            var viewModel = GetMasterFileViewModel(id);
            return View(viewModel);
        }

        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.Update)]
        public ActionResult Update(TemplateParameter parameters)
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

        public LookupItemVo GetLookupItem(LookupItem itemInfo)
        {
            var selector = new Func<Template, LookupItemVo>(o => new LookupItemVo
            {
                KeyId = o.Id,
                DisplayName = o.Title
            });
            return _templateService.GetLookupItem(itemInfo, selector);
        }
     
    }
}