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
using QuickspatchWeb.Models.State;
using Newtonsoft.Json;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Authentication;
using Framework.DomainModel.ValueObject;

namespace QuickspatchWeb.Controllers
{
    public class StateController : ApplicationControllerGeneric<State, DashboardStateDataViewModel>
    {
        private readonly IStateService _stateService;

        public StateController(IAuthenticationService authenticationService, IDiagnosticService diagnosticService,
            IStateService stateService)
            : base(authenticationService, diagnosticService, stateService)
        {
            _stateService = stateService;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View)]
        public JsonResult GetLookup(LookupQuery queryInfo)
        {
            var selector = new Func<State, LookupItemVo>(o => new LookupItemVo
            {
                KeyId = o.Id,
                DisplayName = o.Name
            });
            queryInfo.Id = 0;
            return base.GetLookupForEntity(queryInfo, selector);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View)]
        public JsonResult GetListState()
        {
            var data = _stateService.GetAllStateForLookUp();
            return Json(data, JsonRequestBehavior.AllowGet);

        }
    }
}