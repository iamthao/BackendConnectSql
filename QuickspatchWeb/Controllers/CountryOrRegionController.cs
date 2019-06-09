using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using Framework.Mapping;
using Framework.Service.Diagnostics;
using QuickspatchWeb.Attributes;
using QuickspatchWeb.Models;
using QuickspatchWeb.Models.CountryOrRegion;
using QuickspatchWeb.Models.State;
using Newtonsoft.Json;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Authentication;
using Framework.DomainModel.ValueObject;

namespace QuickspatchWeb.Controllers
{
    public class CountryOrRegionController : ApplicationControllerGeneric<CountryOrRegion, DashboardCountryOrRegionDataViewModel>
    {

        private readonly ICountryOrRegionService _countryOrRegionService;
        public CountryOrRegionController(IAuthenticationService authenticationService, IDiagnosticService diagnosticService,
            ICountryOrRegionService countryOrRegionService)
            : base(authenticationService, diagnosticService, countryOrRegionService)
        {
            _countryOrRegionService = countryOrRegionService;
        }
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View)]
        public JsonResult GetLookup(LookupQuery queryInfo)
        {
            var selector = new Func<CountryOrRegion, LookupItemVo>(o => new LookupItemVo
            {
                KeyId = o.Id,
                DisplayName = o.Name
            });
            queryInfo.Id = 0;
            return base.GetLookupForEntity(queryInfo, selector);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View)]
        public JsonResult GetListCountryOrRegion()
        {
            var data = _countryOrRegionService.GetAllCountryOrRegionForLookUp();
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View)]
        public JsonResult GetNameCountryOrRegion(int id)
        {
            var data = _countryOrRegionService.FirstOrDefault(o=>o.Id ==id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

	}
}