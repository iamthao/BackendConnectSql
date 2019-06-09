using System.Collections.Generic;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Framework.Repositories;

namespace Repositories.Interfaces
{
    public interface IPackageHistoryRepository : IRepository<PackageHistory>, IQueryableRepository<PackageHistory>
    {
        PackageHistoryDto GetPackageCurrent(int franchiseeId);
        List<PackageHistoryDto> GetListPackageChange(int franchiseeId);

        int GetCurrentRequestId(int franchiseeId);
    }
}
