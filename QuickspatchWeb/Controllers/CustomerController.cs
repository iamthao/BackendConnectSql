//using System;
//using System.Collections.Generic;
//using System.Web.Mvc;
//using Framework.DomainModel.Entities;
//using Framework.DomainModel.Entities.Common;
//using Framework.DomainModel.Entities.Security;
//using Framework.Mapping;
//using Framework.Service.Diagnostics;
//using QuickspatchWeb.Attributes;
//using QuickspatchWeb.Models;
//using QuickspatchWeb.Models.Customer;
//using ServiceLayer.Interfaces;
//using ServiceLayer.Interfaces.Authentication;

//namespace QuickspatchWeb.Controllers
//{
//    public class CustomerController : ApplicationControllerGeneric<Customer, DashboardCustomerDataViewModel>
//    {
//        private readonly ICustomerService _customerService;
//        private readonly IGridConfigService _gridConfigService;
//        private readonly List<string> _imageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };

//        public CustomerController(IAuthenticationService authenticationService, IDiagnosticService diagnosticService, IGridConfigService gridConfigService, ICustomerService customerService)
//            : base(authenticationService, diagnosticService, customerService)
//        {
//            _customerService = customerService;
//            _gridConfigService = gridConfigService;
//        }

//        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Customer, OperationAction = OperationAction.View)]
//        public ActionResult Index()
//        {
//            var viewModel = new DashboardCustomerIndexViewModel();
//            Func<GridViewModel> gridViewModel = () => new GridViewModel
//            {
//                GridId = "CustomerGrid",
//                ModelName = "Customer",
//                DocumentTypeId = (int)DocumentTypeKey.Customer,
//                GridInternalName = "Customer",
//                UseDeleteColumn = true,
//                PopupWidth = 800,
//                PopupHeight = 400
//            };

//            viewModel.GridViewModel = BuildGridViewModel(_gridConfigService, gridViewModel);
//            return View(viewModel);
//        }

//        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Customer, OperationAction = OperationAction.View)]
//        public JsonResult GetDataForGrid(QueryInfo queryInfo)
//        {
//            var queryData = _customerService.GetDataForGridMasterfile(queryInfo);
//            var clientsJson = Json(queryData, JsonRequestBehavior.AllowGet);

//            return clientsJson;
//        }

//        protected override IList<ViewColumnViewModel> GetViewColumns()
//        {
//            return new List<ViewColumnViewModel>
//            {
//                new ViewColumnViewModel
//                {
//                    ColumnOrder = 2,
//                    ColumnWidth = 100,
//                    Name = "Name",
//                    Text = "Name",
//                    ColumnJustification = GridColumnJustification.Left
//                }
//            };
//        }

//        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Customer, OperationAction = OperationAction.Add)]
//        public ActionResult Create()
//        {
//            var viewModel = new DashboardCustomerDataViewModel
//            {
//                SharedViewModel = new DashboardCustomerShareViewModel
//                {
//                    CreateMode = true
//                }
//            };
//            return View(viewModel);
//        }

//        [HttpPost]
//        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Customer, OperationAction = OperationAction.Add)]
//        public int Create(CustomerParameter parameters)
//        {
//            var viewModel = MapFromClientParameters(parameters);
//            var entity = viewModel.MapTo<Customer>();
//            var savedEntity = MasterFileService.Add(entity);
//            return savedEntity.Id;
//        }

//        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Customer, OperationAction = OperationAction.Update)]
//        public ActionResult Update(int id)
//        {
//            var viewModel = GetMasterFileViewModel(id);
//            return View(viewModel);
//        }

//        [HttpPost]
//        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Customer, OperationAction = OperationAction.Update)]
//        public ActionResult Update(CustomerParameter parameters)
//        {
//            var viewModel = MapFromClientParameters(parameters);


//            byte[] lastModified = null;

//            if (ModelState.IsValid)
//            {
//                var entity = MasterFileService.GetById(viewModel.SharedViewModel.Id);
//                var mappedEntity = viewModel.MapPropertiesToInstance(entity);
//                lastModified = MasterFileService.Update(mappedEntity).LastModified;
//            }

//            return Json(new { Error = string.Empty, Data = new { LastModified = lastModified } }, JsonRequestBehavior.AllowGet);
//        }



//        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Customer, OperationAction = OperationAction.Delete)]
//        public JsonResult Delete(int id)
//        {
//            MasterFileService.DeleteById(id);
//            return Json(true, JsonRequestBehavior.AllowGet);
//        }
//    }
//}