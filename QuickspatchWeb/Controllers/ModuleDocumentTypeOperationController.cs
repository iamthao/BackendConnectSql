using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using Framework.Service.Diagnostics;
using QuickspatchWeb.Attributes;
using QuickspatchWeb.Models.ModuleDocumentTypeOperation;
using Newtonsoft.Json;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Authentication;

namespace QuickspatchWeb.Controllers
{
    public partial class ModuleDocumentTypeOperationController : ApplicationControllerGeneric<ModuleDocumentTypeOperation, DashboardModuleDocumentTypeOperationDataViewModel>
    {
        private readonly IModuleDocumentTypeOperationService _moduleDocumentTypeOperationService;
        private readonly IDocumentTypeService _documentTypeService;
        private readonly IModuleService _moduleService;
        private readonly IGridConfigService _gridConfigService;

        public ModuleDocumentTypeOperationController(IAuthenticationService authenticationService, IDiagnosticService diagnosticService, IGridConfigService gridConfigService,
            IModuleDocumentTypeOperationService moduleDocumentTypeOperationService,
            IDocumentTypeService documentTypeService,
            IModuleService moduleService)
            : base(authenticationService, diagnosticService, moduleDocumentTypeOperationService)
        {
            _moduleDocumentTypeOperationService = moduleDocumentTypeOperationService;
            _documentTypeService = documentTypeService;
            _moduleService = moduleService;
            _gridConfigService = gridConfigService;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeModule, OperationAction = OperationAction.View)]
        public JsonResult GetDataForModuleGrid(int moduleId)
        {
            var queryData = ProcessDataForModule(moduleId);
            var clientsJson = Json(new { Data = queryData, TotalRowCount = queryData.Count }, JsonRequestBehavior.AllowGet);

            return clientsJson;
        }

        private List<DashboardModuleDocumentTypeOperationDataItem> ProcessDataForModule(int moduleId)
        {
            var module = _moduleService.GetById(moduleId);
            var moduleDataItem = new List<DashboardModuleDocumentTypeOperationDataItem>();

            if (module == null) return moduleDataItem;
            
            #region get current module of this franchisee
            foreach (var m in module.ModuleDocumentTypeOperations)
            {
                var item = moduleDataItem.FirstOrDefault(p => p.DocumentTypeId == m.DocumentTypeId && p.ModuleId == m.ModuleId);
                if (item == null)
                {
                    var addItem = new DashboardModuleDocumentTypeOperationDataItem()
                    {
                        Id = m.ModuleId,
                        ModuleId = m.ModuleId,
                        ModuleName = m.Module.Name,
                        DocumentTypeId = m.DocumentTypeId,
                        DocumentTypeName = m.DocumentType.Title,
                        SercurityOperation = m.SercurityOperationId
                    };
                    moduleDataItem.Add(addItem);
                    item = moduleDataItem.FirstOrDefault(p => p.DocumentTypeId == m.DocumentTypeId && p.ModuleId == m.ModuleId);
                }

                switch (m.SercurityOperationId)
                {
                    case (int)OperationAction.View:
                        item.IsView = true;
                        break;
                    case (int)OperationAction.Delete:
                        item.IsDelete = true;
                        break;
                    case (int)OperationAction.Add:
                        item.IsInsert = true;
                        break;
                    case (int)OperationAction.Update:
                        item.IsUpdate = true;
                        break;
                    case (int)OperationAction.Process:
                        item.IsProcess = true;
                        break;
                }
            }
            #endregion

            #region get new operation is not avaible of this franchisee
            var currentDocumentTypeId = module.ModuleDocumentTypeOperations.ToList().Select(x => x.DocumentTypeId);
            foreach (var m in _documentTypeService.Get(p => !currentDocumentTypeId.Contains(p.Id)))
            {
                var addItem = new DashboardModuleDocumentTypeOperationDataItem()
                {
                    Id = module.Id,
                    ModuleId = module.Id,
                    ModuleName = module.Name,
                    DocumentTypeId = m.Id,
                    DocumentTypeName = m.Title,
                };
                moduleDataItem.Add(addItem);
            }
            #endregion

            return moduleDataItem.OrderBy(p=>p.DocumentTypeName).ToList();
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeModule, OperationAction = OperationAction.Update)]
        public ActionResult Update(int id)
        {
            var module = _moduleService.GetById(id);
            var viewModel = new DashboardModuleDocumentTypeOperationDataViewModel
            {
                SharedViewModel = new DashboardModuleDocumentTypeOperationShareViewModel()
                {
                    Id = module.Id,
                    ModuleName = module.Name,
                    ModuleId = module.Id
                }
            };

            return View(viewModel);
        }

        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeModule, OperationAction = OperationAction.Update)]
        public ActionResult Update(ModuleDocumentTypeOperationParameter parameters)
        {
            var data = JsonConvert.DeserializeObject<List<DashboardModuleDocumentTypeOperationDataItem>>(parameters.ModuleOperationData);
            var viewModel = MapFromClientParameters(parameters);
            var entity = _moduleService.GetById(viewModel.SharedViewModel.Id);

            foreach (var module in entity.ModuleDocumentTypeOperations)
            {
                module.IsDeleted = true;
            }
            
            #region add data to database
            foreach (var item in data)
            {
                if (item.IsView)
                {
                    entity.ModuleDocumentTypeOperations.Add(new ModuleDocumentTypeOperation()
                    {
                        ModuleId = item.ModuleId,
                        DocumentTypeId = item.DocumentTypeId,
                        SercurityOperationId = (int)OperationAction.View
                    });
                }
                if (item.IsDelete)
                {
                    entity.ModuleDocumentTypeOperations.Add(new ModuleDocumentTypeOperation()
                    {
                        ModuleId = item.ModuleId,
                        DocumentTypeId = item.DocumentTypeId,
                        SercurityOperationId = (int)OperationAction.Delete
                    });
                }

                if (item.IsUpdate)
                {
                    entity.ModuleDocumentTypeOperations.Add(new ModuleDocumentTypeOperation()
                    {
                        ModuleId = item.ModuleId,
                        DocumentTypeId = item.DocumentTypeId,
                        SercurityOperationId = (int)OperationAction.Update
                    });
                }

                if (item.IsInsert)
                {
                    entity.ModuleDocumentTypeOperations.Add(new ModuleDocumentTypeOperation()
                    {
                        ModuleId = item.ModuleId,
                        DocumentTypeId = item.DocumentTypeId,
                        SercurityOperationId = (int)OperationAction.Add
                    });
                }

                if (item.IsProcess)
                {
                    entity.ModuleDocumentTypeOperations.Add(new ModuleDocumentTypeOperation()
                    {
                        ModuleId = item.ModuleId,
                        DocumentTypeId = item.DocumentTypeId,
                        SercurityOperationId = (int)OperationAction.Process
                    });
                }
            }
            #endregion

            byte[] lastModified = null;
            lastModified = _moduleService.Update(entity).LastModified;

            return Json(new { Error = string.Empty, Data = new { LastModified = lastModified } }, JsonRequestBehavior.AllowGet);
        }

    }
}