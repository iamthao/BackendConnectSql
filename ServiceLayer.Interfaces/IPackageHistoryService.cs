using System.Collections.Generic;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;

namespace ServiceLayer.Interfaces
{
    public interface IPackageHistoryService : IMasterFileService<PackageHistory>
    {
        bool AddPackageHistory(AddPackageHistoryDto packageHistoryInfo);

        PackageHistoryDto GetPackageCurrent(FranchisseNameAndLicenseDto franchiseeData);
        List<PackageHistoryDto> GetListPackageChange(FranchisseNameAndLicenseDto franchiseeData);

        int GetRequestCurrentId(FranchisseNameAndLicenseDto franchiseeData);

        PackageHistoryDto GetPackageCurrentNoToken(FranchisseNameAndLicenseDto franchiseeData);

        bool AddPackageHistoryNoToken(PackageHistoryDto packageHistoryInfo);
    }
}
