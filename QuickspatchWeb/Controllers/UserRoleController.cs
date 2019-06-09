using System.Configuration;
using System.Linq;
using DotNetOpenAuth.Messaging;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using Framework.DomainModel.ValueObject;
using Framework.Exceptions;
using Framework.Mapping;
using Framework.Service.Diagnostics;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using QuickspatchWeb.Attributes;
using QuickspatchWeb.Models;
using QuickspatchWeb.Models.UserRole;
using QuickspatchWeb.Services.Interface;
using Framework.DomainModel.Entities;
using ServiceLayer.Common;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Authentication;

namespace QuickspatchWeb.Controllers
{
    public class UserRoleController : ApplicationControllerGeneric<UserRole, DashboardUserRoleDataViewModel>
    {
        //
        // GET: /UserRole/
        private readonly IUserRoleService _userRoleService;
        private readonly IGridConfigService _gridConfigService;
        private readonly IRenderViewToString _renderViewToString;

        public UserRoleController(IAuthenticationService authenticationService, IDiagnosticService diagnosticService,
            IUserRoleService userRoleService, IGridConfigService gridConfigService, IRenderViewToString renderViewToString)
            : base(authenticationService, diagnosticService, userRoleService)
        {
            _userRoleService = userRoleService;
            _gridConfigService = gridConfigService;
            _renderViewToString = renderViewToString;
        }
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.UserRole, OperationAction = OperationAction.View)]
        public ActionResult Index()
        {
            var viewModel = new DashboardUserRoleIndexViewModel();
            Func<GridViewModel> gridViewModel = () => new GridViewModel
            {
                GridId = "UserRoleGrid",
                ModelName = "UserRole",
                DocumentTypeId = (int)DocumentTypeKey.UserRole,
                GridInternalName = "UserRole",
                //UseDeleteColumn = true,
                PopupWidth = 800,
                PopupHeight = 441
            };

            viewModel.GridViewModel = BuildGridViewModel(_gridConfigService, gridViewModel);
            return View(viewModel);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.UserRole, OperationAction = OperationAction.View)]
        public JsonResult GetDataForGrid(QueryInfo queryInfo)
        {
            return base.GetDataForGridMasterFile(queryInfo);
        }

        protected override IList<ViewColumnViewModel> GetViewColumns()
        {
            var objViewColumn = new List<ViewColumnViewModel>
            {
                new ViewColumnViewModel
                {
                    ColumnOrder = 1,
                    Name = "Name",
                    Text = "Name",
                    ColumnWidth = 600,
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
            return objViewColumn;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.UserRole, OperationAction = OperationAction.Update)]
        public ActionResult Update(int id)
        {
            var viewModel = GetMasterFileViewModel(id);

            return View(viewModel);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.UserRole, OperationAction = OperationAction.View)]
        public JsonResult GetRoleFunction(int id)
        {
            var queryData = _userRoleService.GetRoleFunction(id);
            var clientsJson = Json(queryData, JsonRequestBehavior.AllowGet);

            return clientsJson;
        }

        private List<UserRoleFunction> GetAllRoleFunction()
        {
            var objResult = new List<UserRoleFunction>();
            var objListDocumentType = _userRoleService.GetAllDocumentTypeId();
            foreach (var id in objListDocumentType)
            {
                var objViewAdd = new UserRoleFunction
                {
                    DocumentTypeId = id,
                    SecurityOperationId = (int)OperationAction.View
                };
                objResult.Add(objViewAdd);
                var objInsertAdd = new UserRoleFunction
                {
                    DocumentTypeId = id,
                    SecurityOperationId = (int)OperationAction.Add
                };
                objResult.Add(objInsertAdd);
                var objUpdateAdd = new UserRoleFunction
                {
                    DocumentTypeId = id,
                    SecurityOperationId = (int)OperationAction.Update
                };
                objResult.Add(objUpdateAdd);
                var objDeleteAdd = new UserRoleFunction
                {
                    DocumentTypeId = id,
                    SecurityOperationId = (int)OperationAction.Delete
                };
                objResult.Add(objDeleteAdd);
                var objProcessAdd = new UserRoleFunction
                {
                    DocumentTypeId = id,
                    SecurityOperationId = (int)OperationAction.Process
                };
                objResult.Add(objProcessAdd);
            }
            return objResult;
        }

        private List<UserRoleFunction> ProcessMappingFromUserRoleGrid(string userRoleFunctionDataParam)
        {
            var userRoleFunctionData = JsonConvert.DeserializeObject<List<UserRoleFunctionGridVo>>(userRoleFunctionDataParam);
            var objResult = new List<UserRoleFunction>();
            var listUpdate = userRoleFunctionData;
            if (listUpdate != null && listUpdate.Count != 0)
            {
                foreach (var userRoleFunctionGridVo in listUpdate)
                {
                    var objViewAdd = new UserRoleFunction
                    {
                        DocumentTypeId = userRoleFunctionGridVo.Id,
                        SecurityOperationId = (int)OperationAction.View,
                        IsDeleted = !userRoleFunctionGridVo.IsView
                    };
                    objResult.Add(objViewAdd);

                    //Implement View insert
                    var objInsertAdd = new UserRoleFunction
                    {
                        DocumentTypeId = userRoleFunctionGridVo.Id,
                        SecurityOperationId = (int)OperationAction.Add,
                        IsDeleted = !userRoleFunctionGridVo.IsInsert
                    };
                    objResult.Add(objInsertAdd);

                    //Implement View update
                    var objUpdateAdd = new UserRoleFunction
                    {
                        DocumentTypeId = userRoleFunctionGridVo.Id,
                        SecurityOperationId = (int)OperationAction.Update,
                        IsDeleted = !userRoleFunctionGridVo.IsUpdate
                    };
                    objResult.Add(objUpdateAdd);
                    //Implement View delete
                    var objDeleteAdd = new UserRoleFunction
                    {
                        DocumentTypeId = userRoleFunctionGridVo.Id,
                        SecurityOperationId = (int)OperationAction.Delete,
                        IsDeleted = !userRoleFunctionGridVo.IsDelete
                    };
                    objResult.Add(objDeleteAdd);
                    var objProcessAdd = new UserRoleFunction
                    {
                        DocumentTypeId = userRoleFunctionGridVo.Id,
                        SecurityOperationId = (int)OperationAction.Process,
                        IsDeleted = !userRoleFunctionGridVo.IsProcess
                    };
                    objResult.Add(objProcessAdd);
                }
            }
            return objResult;
        }

        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.UserRole, OperationAction = OperationAction.Update)]
        public ActionResult Update(UserRoleParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);
            byte[] lastModified = null;
            var shareViewModel = viewModel.SharedViewModel as DashboardUserRoleShareViewModel;
            if (shareViewModel != null)
            {
                var entity = MasterFileService.GetById(shareViewModel.Id);
                //if (entity.AppRoleName == AppRole.GlobalAdmin.ToString())
                //{
                //    throw new Exception("Not allow to change global admin role function");
                //}
                var listRoleFunctionUpdate = shareViewModel.CheckAll ? GetAllRoleFunction() : ProcessMappingFromUserRoleGrid(shareViewModel.UserRoleFunctionData);
                var mappedEntity = shareViewModel.MapPropertiesToInstance(entity);
                var listRoleFunctionOld = mappedEntity.UserRoleFunctions;
                // Check user have edit some value in list role old => delete role old and add role new
                foreach (var oldItem in listRoleFunctionOld)
                {
                    if (listRoleFunctionUpdate.Any(o => o.DocumentTypeId == oldItem.DocumentTypeId))
                    {
                        oldItem.IsDeleted = true;
                    }
                }
                //after check user removed, remove item of the list new with conditions has property IsDelete equal true;

                //Copy listRoleFunctionUpdate
                var listRoleFunctionUpdateRecheck = listRoleFunctionUpdate.ToList();
                foreach (var item in listRoleFunctionUpdateRecheck.Where(item => item.IsDeleted))
                {
                    listRoleFunctionUpdate.Remove(item);
                }
                // Add listUpdate
                mappedEntity.UserRoleFunctions.AddRange(listRoleFunctionUpdate);
                if (ModelState.IsValid)
                {
                    mappedEntity = MasterFileService.Update(mappedEntity);
                    lastModified = mappedEntity.LastModified;
                    MenuExtractData.Instance.RefershListData();
                }
            }


            return Json(new { Error = string.Empty, Data = new { LastModified = lastModified } }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.UserRole, OperationAction = OperationAction.Add)]
        public ActionResult Create()
        {
            var viewModel = new DashboardUserRoleDataViewModel
            {
                SharedViewModel = new DashboardUserRoleShareViewModel
                {
                    CreateMode = true
                }
            };
            return View(viewModel);
        }

        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.UserRole, OperationAction = OperationAction.Add)]
        public ActionResult Create(UserRoleParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);
            var shareViewModel = viewModel.SharedViewModel as DashboardUserRoleShareViewModel;
            if (shareViewModel != null)
            {
                var listRoleFunctionUpdate = shareViewModel.CheckAll ? GetAllRoleFunction() : ProcessMappingFromUserRoleGrid(shareViewModel.UserRoleFunctionData);
                var listRoleFunctionOld = new List<UserRoleFunction>();
                // Check user have edit some value in list role old => delete role old and add role new
                foreach (var oldItem in listRoleFunctionOld)
                {
                    if (listRoleFunctionUpdate.Any(o => o.DocumentTypeId == oldItem.DocumentTypeId))
                    {
                        oldItem.IsDeleted = true;
                    }
                }
                //after check user removed, remove item of the list new with conditions has property IsDelete equal true;

                //Copy listRoleFunctionUpdate
                var listRoleFunctionUpdateRecheck = listRoleFunctionUpdate.ToList();
                foreach (var item in listRoleFunctionUpdateRecheck.Where(item => item.IsDeleted))
                {
                    listRoleFunctionUpdate.Remove(item);
                }
                var objListFunctionForEntity = new List<UserRoleFunction>();
                // Add listUpdate
                objListFunctionForEntity.AddRange(listRoleFunctionUpdate);
                // Add list data in old
                foreach (var itemOld in listRoleFunctionOld.Where(o => !o.IsDeleted))
                {
                    var objAdd = new UserRoleFunction
                    {
                        DocumentTypeId = itemOld.DocumentTypeId,
                        SecurityOperationId = itemOld.SecurityOperationId
                    };
                    objListFunctionForEntity.Add(objAdd);
                }
                var entity = shareViewModel.MapTo<UserRole>();
                entity.UserRoleFunctions = objListFunctionForEntity;
                var savedEntity = MasterFileService.Add(entity);
                MenuExtractData.Instance.RefershListData();
                return Json(new { savedEntity.Id }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Id = 0 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.UserRole, OperationAction = OperationAction.Delete)]
        public ActionResult Delete(int id)
        {
            MasterFileService.DeleteById(id);
            MenuExtractData.Instance.RefershListData();
            return Json(new { isSuccess = true }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.UserRole, OperationAction = OperationAction.View)]
        [HttpPost]
        public JsonResult GetLookup(LookupQuery queryInfo)
        {
            var selector = new Func<UserRole, LookupItemVo>(o => new LookupItemVo
            {
                KeyId = o.Id,
                DisplayName = o.Name
            });
            queryInfo.Id = 0;
            return base.GetLookupForEntity(queryInfo, selector);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.UserRole, OperationAction = OperationAction.View)]
        public JsonResult GetListRoles()
        {
            var data = _userRoleService.ListAll();
            var result = data.Select(o => new LookupItemVo { DisplayName = o.Name, KeyId = o.Id }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.UserRole, OperationAction = OperationAction.View)]
        public JsonResult GetListRolesNoCourier()
        {
            var data = _userRoleService.GetUserRoleNoCourier();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExportExcel(List<ColumnModel> gridColumns, QueryInfo queryInfo)
        {
            return base.ExportExcelMasterfile(gridColumns, queryInfo);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.UserRole, OperationAction = OperationAction.View)]
        public FileResult DownloadExcelFile(string fileName)
        {
            return base.DownloadExcelMasterFile(fileName);
        }
    }
}