using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;

namespace Consume.ServiceLayer.Interface
{
    public interface IWebApiConsumeUserService : IWebApiConsumeMasterFileService<UserLoginForWebApiDto>
    {
        TokenStoreDto GetToken(FranchisseNameAndLicenseDto loginData);
        ModuleForFranchiseeDto GetModuleForFranchisee(FranchisseNameAndLicenseDto franchiseeData);
        ActiveDateLicenseKeyDto GetActiveDateLicenseKey(FranchisseNameAndLicenseDto franchiseeData);
        bool UpdateFranchiseeConfig(FranchiseeTernantDto franchiseeData);
        bool IsExpireFranchisee(FranchisseNameAndLicenseDto franchisseNameAndLicenseDto);
        FranchiseeTernantDto GetInfoFranchisee(FranchisseNameAndLicenseDto franchiseeData);
        bool AddPackageHistory(AddPackageHistoryDto packageHistoryInfo);
        PackageHistoryDto GetPackageCurrent(FranchisseNameAndLicenseDto franchiseeData);
        bool UpdateFranchiseeTenantCloseAccount(FranchiseeTernantCloseAccountDto franchiseeData);
        List<PackageHistoryDto> GetListPackageChange(FranchisseNameAndLicenseDto franchiseeData);
        bool UpdateFranchiseeTenantCancelAccount(FranchisseNameAndLicenseDto franchiseeData);
        bool FranchiseeTenantUpdatePayment(FranchiseeTenantUpdatePaymentDto franchiseeTenantUpdatePaymentDto);
        #region No Token
        FranchiseeTernantCurrentPackageDto GetPackageCurrentId(FranchisseNameAndLicenseDto franchiseeData);
        int GetRequestCurrentId(FranchisseNameAndLicenseDto franchiseeData);
        bool UpdateFranchiseeTenantLicenceExtentsion(FranchisseNameAndLicenseDto objFranchiseeAndLicense);
        bool AddPackageHistoryNoToken(PackageHistoryDto packageHistoryInfo);
        FranchiseeTernantDto GetInfoFranchiseeNoToken(FranchisseNameAndLicenseDto franchiseeData);
        PackageHistoryDto GetPackageCurrentNoToken(FranchisseNameAndLicenseDto franchiseeData);
        #endregion
    }
}