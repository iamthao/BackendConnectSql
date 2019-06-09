using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using Framework.Mapping;
using Framework.Service.Diagnostics;
using Framework.Utility;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json.Linq;
using QuickspatchWeb.Attributes;
using QuickspatchWeb.Models;
using QuickspatchWeb.Models.Location;
using QuickspatchWeb.Services.Interface;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Authentication;
using Framework.DomainModel.ValueObject;
using Newtonsoft.Json;
using System.Net;
using Framework.Service.Translation;

namespace QuickspatchWeb.Controllers
{
    public class LocationController : ApplicationControllerGeneric<Location, DashboardLocationDataViewModel>
    {
        private readonly IGridConfigService _gridConfigService;
        private readonly ILocationService _locationService;
        private readonly IRenderViewToString _renderViewToString;
        private readonly ICountryOrRegionService _countryOrRegionService;
        private readonly IFranchiseeConfigurationService _franchiseeConfigurationService;
        private readonly ISystemConfigurationService _systemConfigurationService;

        public LocationController(IAuthenticationService authenticationService,
            IDiagnosticService diagnosticService,
            ILocationService locationService,
            IRenderViewToString renderViewToString,
            ICountryOrRegionService countryOrRegionService,
            IFranchiseeConfigurationService franchiseeConfigurationService,
            ISystemConfigurationService systemConfigurationService,
            IGridConfigService gridConfigService)
            : base(authenticationService, diagnosticService, locationService)
        {
            _gridConfigService = gridConfigService;
            _locationService = locationService;
            _renderViewToString = renderViewToString;
            _countryOrRegionService = countryOrRegionService;
            _franchiseeConfigurationService = franchiseeConfigurationService;
            _systemConfigurationService = systemConfigurationService;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Location, OperationAction = OperationAction.View)]
        public ActionResult Index()
        {
            var viewModel = new DashboardLocationIndexViewModel();
            Func<GridViewModel> gridViewModel = () => new GridViewModel
            {
                GridId = "LocationGrid",
                ModelName = "Location",
                DocumentTypeId = (int)DocumentTypeKey.Location,
                GridInternalName = "Location",
                UseDeleteColumn = true,
                PopupWidth = 600,
                PopupHeight = 500
            };
            viewModel.GridViewModel = BuildGridViewModel(_gridConfigService, gridViewModel);
            return View(viewModel);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Location, OperationAction = OperationAction.View)]
        public JsonResult GetDataForGrid(QueryInfo queryInfo)
        {
            var queryData = _locationService.GetDataForGridMasterfile(queryInfo);
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
                    ColumnWidth = 100,
                    Name = "Name",
                    Text = "Name",
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 3,
                    ColumnWidth = 100,
                    Name = "FullAddress",
                    Text = "Address",
                    ColumnJustification = GridColumnJustification.Left
                },
                               
                
                new ViewColumnViewModel
                {
                    ColumnOrder = 5,
                    ColumnWidth = 125,
                    Name = "AvailableTime",
                    Text = "Open Day(s)",
                    ColumnJustification = GridColumnJustification.Left
                },

                new ViewColumnViewModel
                {
                    ColumnOrder = 5,
                    ColumnWidth = 125,
                    Name = "OpenHour",
                    Text = "Open Hour",
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 5,
                    ColumnWidth = 125,
                    Name = "CloseHour",
                    Text = "Close Hour",
                    ColumnJustification = GridColumnJustification.Left
                },
            };
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Location, OperationAction = OperationAction.View)]
        public JsonResult GetLocationDataForDropdownlist()
        {
            var data = _locationService.ListAll();
            var result = data.Select(o => new LookupItemVo { DisplayName = o.Name, KeyId = o.Id }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Location, OperationAction = OperationAction.Add)]
        public ActionResult Create(string type)
        {
            var viewModel = new DashboardLocationDataViewModel
            {
                SharedViewModel = new DashboardLocationShareViewModel()
                {
                    CreateMode = true,
                    Type = type
                }
            };
            return View(viewModel);
        }

        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Location, OperationAction = OperationAction.Add)]
        public JsonResult Create(LocationParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);
            var entity = viewModel.MapTo<Location>();
            var savedEntity = MasterFileService.Add(entity);

            return
                Json(new
                {
                    id = savedEntity.Id,
                    name = savedEntity.Name
                }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Location, OperationAction = OperationAction.Update)]
        public ActionResult Update(int id, string type)
        {
            var viewModel = GetMasterFileViewModel(id);
            var locationViewModel = viewModel.SharedViewModel as DashboardLocationShareViewModel;
            if (locationViewModel != null) locationViewModel.Type = type;
            return View(viewModel);
        }

        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Location, OperationAction = OperationAction.Update)]
        public ActionResult Update(LocationParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);

            byte[] lastModified = null;

            var id = 0;
            var name = "";
            if (ModelState.IsValid)
            {
                var entity = MasterFileService.GetById(viewModel.SharedViewModel.Id);              
                var mappedEntity = viewModel.MapPropertiesToInstance(entity);
                lastModified = MasterFileService.Update(mappedEntity).LastModified;
                id = mappedEntity.Id;
                name = mappedEntity.Name;

            }

            return Json(new { Error = string.Empty, Data = new { id, name, LastModified = lastModified } }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Location, OperationAction = OperationAction.Delete)]
        public JsonResult Delete(int id)
        {
            var location = MasterFileService.GetById(id);
            if (location != null && 
                (location.RequestsOfLocationTo.Count > 0 || location.RequestsOfLocationFrom.Count > 0 
                ||location.HoldingRequestsTo.Count > 0 || location.HoldingRequestsFrom.Count > 0))
            {
                var mess = string.Format(SystemMessageLookup.GetMessage("DeleteLocationHasRequest"));
                MasterFileService.ThrowCustomValidation(mess);
            }
            var from =
                _systemConfigurationService.FirstOrDefault(
                    o => o.SystemConfigType == SystemConfigType.DefaultLocationFrom);
            if (from!= null)
            {
                if (Convert.ToInt32(from.Value) == id)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("DeleteLocationDefault"));
                    MasterFileService.ThrowCustomValidation(mess); 
                }
               
            }

            var to =
                _systemConfigurationService.FirstOrDefault(
                    o => o.SystemConfigType == SystemConfigType.DefaultLocationTo);
            if (to != null)
            {
                if (Convert.ToInt32(to.Value) == id)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("DeleteLocationDefault"));
                    MasterFileService.ThrowCustomValidation(mess);
                }

            }

            MasterFileService.DeleteById(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Location, OperationAction = OperationAction.View)]
        public JsonResult GetLookup(LookupQuery queryInfo)
        {
            var selector = new Func<Location, LookupItemVo>(o => new LookupItemVo
            {
                KeyId = o.Id,
                DisplayName = o.Name
            });

            return base.GetLookupForEntity(queryInfo, selector);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Location, OperationAction = OperationAction.View)]
        public JsonResult ExportExcel(List<ColumnModel> gridColumns, QueryInfo queryInfo)
        {
            return base.ExportExcelMasterfile(gridColumns, queryInfo);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Location, OperationAction = OperationAction.View)]
        public FileResult DownloadExcelFile(string fileName)
        {
            return base.DownloadExcelMasterFile(fileName);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View)]
        public JsonResult GetLocationFromZip(string zip,int idcountry)
        {
            var country = _countryOrRegionService.FirstOrDefault(o => o.Id == idcountry);
            string namecountry = "";
            if (country == null)
            {
                namecountry = "";
            }
            else
            {
                namecountry = country.Name;
            }
            var url = "http://maps.googleapis.com/maps/api/geocode/json?components=postal_code:"+zip+"|country:" + namecountry;
           
            var client = new WebClient();
            //client.Headers.Add("User-Agent", "Nobody"); //my endpoint needs this...
            var response = client.DownloadString(new Uri(url));

            var data = JsonConvert.DeserializeObject<GoogleGetLocation>(response);

            // Display data read from web site        

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View)]
        public JsonResult GetListLocation()
        {
            var listLocation = _locationService.GetListLocation();

            return Json(listLocation.OrderBy(o=>o.Id), JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Location, OperationAction = OperationAction.View)]
        public JsonResult GetDataLocation()
        {
            var listLocation = _locationService.Get(o => true).Select(o => new LocationLookupItemVo
            {
                KeyId = o.Id,
                DisplayName = o.Name,
                Lng = o.Lng,
                Lat = o.Lat,
            }).ToList();
            return Json(listLocation, JsonRequestBehavior.AllowGet);
        }

    }
}