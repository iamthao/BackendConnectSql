using System;
using System.Linq;
using System.Web.Mvc;
using Framework.DomainModel.Entities;
using Framework.Mapping;
using Framework.Service.Diagnostics;
using Framework.Service.Translation;
using Framework.Utility;
using QuickspatchWeb.Models;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Authentication;

namespace QuickspatchWeb.Controllers
{
    public class GridConfigController : ApplicationControllerBase
    {
        //
        // GET: /GridConfig/
        private readonly IGridConfigService _gridConfigService;
        public GridConfigController(IAuthenticationService authenticationService,
                                    IDiagnosticService diagnosticService,
                                    IGridConfigService gridConfigService)
            : base(authenticationService, diagnosticService)
        {
            _gridConfigService = gridConfigService;
        }

        [HttpPost]
        public ActionResult Save(GridConfigViewModel viewModel)
        {
            //check if gird not have Mandatory columns are selected anymore
            if (viewModel.ViewColumns.Where(o => !o.Mandatory).Count(o => (!o.HideColumn)) <= 0)
            {
                return
                Json(
                    new
                    {
                        Error = SystemMessageLookup.GetMessage("HideAllColumn"),
                    },
                    JsonRequestBehavior.AllowGet);
            }
            var gridConfig = _gridConfigService.FirstOrDefault(x => x.Id == viewModel.Id);
            if (gridConfig != null && gridConfig.UserId==0)
            {
                gridConfig = null;
                viewModel.Id = 0;
            }
            gridConfig = viewModel.MapPropertiesToInstance(gridConfig);
            _gridConfigService.InsertOrUpdate(gridConfig);
            return
               Json(
                   new
                   {
                       Error = string.Empty,
                       Data = new { gridConfig.Id }
                   },
                   JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Get(GridConfigViewModel viewModel)
        {
            Func<GridConfig, GridConfigViewModel> selector = g => g.MapTo<GridConfigViewModel>();

            var gridConfig = _gridConfigService.GetGridConfig(selector,
                                                            viewModel.UserId,
                                                            viewModel.DocumentTypeId,
                                                            viewModel.GridInternalName);
            var xml = SerializationHelper.SerializeToXml(gridConfig);
            var gridConfigJson = Json(gridConfig, JsonRequestBehavior.AllowGet);

            return gridConfigJson;
        }

    }
}
