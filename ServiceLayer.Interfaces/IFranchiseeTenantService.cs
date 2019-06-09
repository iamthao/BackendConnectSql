using System;
using System.Collections.Generic;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;

namespace ServiceLayer.Interfaces
{
    public interface IFranchiseeTenantService : IMasterFileService<FranchiseeTenant>
    {
        FranchiseeTenant CheckFranchiseeWithNameAndLicenseKey(string franchiseeName, string licenseKey);
        ModuleForFranchiseeDto GetModuleForFranchisee(FranchisseNameAndLicenseDto franchiseeData);
        ActiveDateLicenseKeyDto GetActiveDateLicenseKey(FranchisseNameAndLicenseDto franchiseeData);
        bool UpdateFranchiseeConfig(FranchiseeTernantDto franchiseeData);
        //
        int SetupFranchisee(FranchiseeTenant franchiseeTenant, FranchiseeConfiguration franchiseeConfiguration);

        void GetFranchiseeDataForUpdate(int labId, out FranchiseeTenant outfranchiseeTenant, out FranchiseeConfiguration outfranchiseeConfiguration);
       
        byte[] UpdateFranchisee(FranchiseeTenant franchiseeTenant, FranchiseeConfiguration franchiseeConfiguration);
        bool CheckFranchiseIsExpire(FranchisseNameAndLicenseDto franchiseeData);
        string GetDisplayNameForCourier();
        FranchiseeTenant DeactivateFranchisee(int id);
        FranchiseeTenant ActivateFranchisee(int id);
        List<LookupItemVo> GetListIndustry();
        FranchiseeTernantDto GetInfoFranchisee(FranchisseNameAndLicenseDto franchiseeData);

        bool UpdateFranchiseeTenantCloseAccount(FranchiseeTernantCloseAccountDto franchiseeData);
        bool UpdateFranchiseeTenantCancelAccount(FranchisseNameAndLicenseDto franchiseeData);

        FranchiseeTernantCurrentPackageDto GetPackageCurrentId(FranchisseNameAndLicenseDto franchiseeData);

        bool UpdateFranchiseeTenantLicenceExtentsion(FranchisseNameAndLicenseDto franchiseeData);

        FranchiseeTernantDto GetInfoFranchiseeNoToken(FranchisseNameAndLicenseDto franchiseeData);
        bool FranchiseeTenantUpdatePayment(FranchiseeTenantUpdatePaymentDto franchiseeTenantUpdatePaymentDto);
    }
}