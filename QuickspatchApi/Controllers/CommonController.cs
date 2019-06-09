using System.Web.Http;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using Framework.Mapping;
using Framework.Service.Diagnostics;
using QuickspatchApi.Attributes;
using ServiceLayer.Interfaces;

namespace QuickspatchApi.Controllers
{
    [RoutePrefix("api/Common")]
    public class CommonController : ApplicationControllerBase
    {
        private readonly IDiagnosticService _diagnosticService;
        private readonly IFranchiseeTenantService _franchiseeTenantService;
        private readonly IPackageHistoryService _packageHistoryService;
        public CommonController(IDiagnosticService diagnosticService, IFranchiseeTenantService franchiseeTenantService, IPackageHistoryService packageHistoryService)
            : base(diagnosticService, null)
        {
            _diagnosticService = diagnosticService;
            _franchiseeTenantService = franchiseeTenantService;
            _packageHistoryService = packageHistoryService;
        }

        [HttpPost]
        [Route("GetModuleForFranchisee")]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View,QuickspatchClientType = QuickspatchClientType.WebClient)]
        public IHttpActionResult GetModuleForFranchisee(FranchisseNameAndLicenseDto franchiseeData)
        {
            return Ok(_franchiseeTenantService.GetModuleForFranchisee(franchiseeData));
        }
        [HttpPost]
        [Route("GetRequest")]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View, QuickspatchClientType = QuickspatchClientType.WebClient)]
        public IHttpActionResult GetRequest(FranchisseNameAndLicenseDto franchiseeData)
        {
            return Ok(_franchiseeTenantService.GetModuleForFranchisee(franchiseeData));
        }
        [HttpPost]
        [Route("GetActiveDateLicenseKey")]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View, QuickspatchClientType = QuickspatchClientType.WebClient)]
        public IHttpActionResult GetActiveDateLicenseKey(FranchisseNameAndLicenseDto franchiseeData)
        {
            return Ok(_franchiseeTenantService.GetActiveDateLicenseKey(franchiseeData));
        }
        [HttpPost]
        [Route("UpdateFranchiseeConfig")]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View, QuickspatchClientType = QuickspatchClientType.WebClient)]
        public IHttpActionResult UpdateFranchiseeConfig(FranchiseeTernantDto franchiseeData)
        {
            return Ok(_franchiseeTenantService.UpdateFranchiseeConfig(franchiseeData));
        }

        [HttpPost]
        [Route("UpdateFranchiseeTenantCloseAccount")]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View, QuickspatchClientType = QuickspatchClientType.WebClient)]
        public IHttpActionResult UpdateFranchiseeTenantCloseAccount(FranchiseeTernantCloseAccountDto franchiseeData)
        {
            return Ok(_franchiseeTenantService.UpdateFranchiseeTenantCloseAccount(franchiseeData));
        }

        [HttpPost]
        [Route("UpdateFranchiseeTenantCancelAccount")]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View, QuickspatchClientType = QuickspatchClientType.WebClient)]
        public IHttpActionResult UpdateFranchiseeTenantCancelAccount(FranchisseNameAndLicenseDto franchiseeData)
        {
            return Ok(_franchiseeTenantService.UpdateFranchiseeTenantCancelAccount(franchiseeData));
        }

        [HttpPost]
        [Route("CheckIsExpire")]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View, QuickspatchClientType = QuickspatchClientType.WebClient)]
        public IHttpActionResult CheckIsExpire(FranchisseNameAndLicenseDto franchiseeData)
        {
            return Ok(_franchiseeTenantService.CheckFranchiseIsExpire(franchiseeData));
        }

        [HttpPost]
        [Route("GetInfoFranchisee")]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View, QuickspatchClientType = QuickspatchClientType.WebClient)]
        public IHttpActionResult GetInfoFranchisee(FranchisseNameAndLicenseDto franchiseeData)
        {
            return Ok(_franchiseeTenantService.GetInfoFranchisee(franchiseeData));
        }

        [HttpPost]
        [Route("AddPackageHistory")]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View, QuickspatchClientType = QuickspatchClientType.WebClient)]
        public IHttpActionResult AddPackageHistory(AddPackageHistoryDto packageHistoryInfo)
        {
            return Ok(_packageHistoryService.AddPackageHistory(packageHistoryInfo));
        }

        [HttpPost]
        [Route("GetPackageCurrent")]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View, QuickspatchClientType = QuickspatchClientType.WebClient)]
        public IHttpActionResult GetPackageCurrent(FranchisseNameAndLicenseDto franchiseeData)
        {
            return Ok(_packageHistoryService.GetPackageCurrent(franchiseeData));
        }

        [HttpPost]
        [Route("GetListPackageChange")]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View, QuickspatchClientType = QuickspatchClientType.WebClient)]
        public IHttpActionResult GetListPackageChange(FranchisseNameAndLicenseDto franchiseeData)
        {
            return Ok(_packageHistoryService.GetListPackageChange(franchiseeData));
        }
        [HttpPost]
        [Route("FranchiseeTenantUpdatePayment")]
        public IHttpActionResult FranchiseeTenantUpdatePayment(FranchiseeTenantUpdatePaymentDto franchiseeTenantUpdatePaymentDto)
        {
            return Ok(_franchiseeTenantService.FranchiseeTenantUpdatePayment(franchiseeTenantUpdatePaymentDto));
        }
        #region No Token
        [HttpPost]
        [Route("GetPackageCurrentId")]
        public IHttpActionResult GetPackageCurrentId(FranchisseNameAndLicenseDto franchiseeData)
        {
            return Ok(_franchiseeTenantService.GetPackageCurrentId(franchiseeData));
        }

        [HttpPost]
        [Route("GetRequestCurrentId")]
        public IHttpActionResult GetRequestCurrentId(FranchisseNameAndLicenseDto franchiseeData)
        {
            return Ok(_packageHistoryService.GetRequestCurrentId(franchiseeData));
        }
        [HttpPost]
        [Route("UpdateFranchiseeTenantLicenceExtentsion")]
        public IHttpActionResult UpdateFranchiseeTenantLicenceExtentsion(FranchisseNameAndLicenseDto franchiseeData)
        {
            return Ok(_franchiseeTenantService.UpdateFranchiseeTenantLicenceExtentsion(franchiseeData));
        }
        [HttpPost]
        [Route("GetInfoFranchiseeNoToken")]
        public IHttpActionResult GetInfoFranchiseeNoToken(FranchisseNameAndLicenseDto franchiseeData)
        {
            return Ok(_franchiseeTenantService.GetInfoFranchiseeNoToken(franchiseeData));
        }
        [HttpPost]
        [Route("GetPackageCurrentNoToken")]
        public IHttpActionResult GetPackageCurrentNoToken(FranchisseNameAndLicenseDto franchiseeData)
        {
            return Ok(_packageHistoryService.GetPackageCurrentNoToken(franchiseeData));
        }
        [HttpPost]
        [Route("AddPackageHistoryNoToken")]
        public IHttpActionResult AddPackageHistoryNoToken(PackageHistoryDto packageHistoryInfo)
        {
            return Ok(_packageHistoryService.AddPackageHistoryNoToken(packageHistoryInfo));
        }
        #endregion
    }
}