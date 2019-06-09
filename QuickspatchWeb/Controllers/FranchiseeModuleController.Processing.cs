using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using QuickspatchWeb.Attributes;
using QuickspatchWeb.Models.FranchiseeModule;

namespace QuickspatchWeb.Controllers
{
    public partial class FranchiseeModuleController
    {
        private List<DashboardFranchiseeModuleDataItem> ProcessDataForFranchiseeModule(int franchiseeId)
        {
            var franchisee = _franchiseeTenantService.GetById(franchiseeId);
            var franchiseeModuleDataItem = new List<DashboardFranchiseeModuleDataItem>();

            if (franchisee == null) return franchiseeModuleDataItem;

            //get current module of this franchisee
            foreach (var module in franchisee.FranchiseeModule)
            {
                franchiseeModuleDataItem.Add(new DashboardFranchiseeModuleDataItem()
                {
                    Id = module.ModuleId,
                    ModuleId = module.ModuleId,
                    ModuleName = module.Module.Name,
                    IsActive = true
                });
            }

            //get new module is not avaible of this franchisee
            var currentModules = franchisee.FranchiseeModule.ToList().Select(x => x.ModuleId);
            foreach (var module in _moduleService.Get(p=> !currentModules.Contains(p.Id)))
            {
                franchiseeModuleDataItem.Add(new DashboardFranchiseeModuleDataItem()
                {
                    Id = 0,
                    ModuleId = module.Id,
                    ModuleName = module.Name,
                    IsActive = false
                });
            }


            return franchiseeModuleDataItem;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeModule, OperationAction = OperationAction.View)]
        public JsonResult GetDataForModuleGrid(int franchiseeId)
        {
            var queryData = ProcessDataForFranchiseeModule(franchiseeId);
            var clientsJson = Json(new { Data = queryData, TotalRowCount = queryData.Count }, JsonRequestBehavior.AllowGet);
            
            return clientsJson;
        }
    }
}