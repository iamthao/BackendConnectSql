using System.Collections.Generic;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Framework.Repositories;

namespace Repositories.Interfaces
{
    public interface IFranchiseeTenantRepository : IRepository<FranchiseeTenant>, IQueryableRepository<FranchiseeTenant>
    {
        ModuleForFranchiseeDto GetModuleForFranchisee(FranchisseNameAndLicenseDto franchiseeData);
        ActiveDateLicenseKeyDto GetActiveDateLicenseKey(FranchisseNameAndLicenseDto franchiseeData);
        List<LookupItemVo> GetListIndustry();
        FranchiseeTernantDto GetInfoFranchisee(FranchisseNameAndLicenseDto franchiseeData);
    }
}