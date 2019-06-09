using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using Database.Persistance.Tenants;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;
using Repositories.Interfaces;

namespace Repositories
{
    public class EntityFrameworkPackageHistoryRepository : EntityFrameworkTenantRepositoryBase<PackageHistory>, IPackageHistoryRepository
    {
        public EntityFrameworkPackageHistoryRepository(ITenantPersistenceService persistenceService, IFranchiseeTenantRepository franchiseeTenantRepository)
            : base(persistenceService)
        {
          
        }

        public PackageHistoryDto GetPackageCurrent(int franchiseeId)
        {
            var package = PersistenceService.CurrentWorkspace.Context.PackageHistories
                .Where(w => w.FranchiseeTenantId == franchiseeId && w.IsApply == true).OrderByDescending(o => o.Id).FirstOrDefault();
            if (package != null)
                return new PackageHistoryDto
                {
                    Id = package.Id,
                    StartDate = package.StartDate,
                    EndDate = package.EndDate,
                    PackageId = package.PackageId,
                    OldPackageId = package.OldPackageId,
                    RequestId = package.RequestId,
                    FranchiseeTenantId = package.FranchiseeTenantId
                };
            return null;
        }


        public List<PackageHistoryDto> GetListPackageChange(int franchiseeId)
        {
            var package = PersistenceService.CurrentWorkspace.Context.PackageHistories
                .Where(w => w.FranchiseeTenantId == franchiseeId).Select(s => new PackageHistoryDto
                    {
                        Id = s.Id,
                        OldPackageId = s.OldPackageId,
                        PackageId =  s.PackageId,
                        StartDate = s.StartDate,
                        EndDate = s.EndDate,
                        FranchiseeTenantId = s.FranchiseeTenantId,
                        RequestId = s.RequestId  ,
                        IsApply = s.IsApply?? false ,
                    }).OrderByDescending(o=>o.Id).ToList();

            return package;
        }

        public int GetCurrentRequestId(int franchiseeId)
        {
            var package = PersistenceService.CurrentWorkspace.Context.PackageHistories
                .Where(w => w.FranchiseeTenantId == franchiseeId && w.IsApply == true).OrderByDescending(o => o.Id).FirstOrDefault();
            return package == null ? 0 : package.RequestId;
        }

    }
}
