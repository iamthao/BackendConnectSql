using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;
using Framework.DomainModel.ValueObject;
using Framework.Utility;

namespace ServiceLayer
{
    public class PackageHistoryService : MasterFileService<PackageHistory>, IPackageHistoryService
    {
        private readonly IPackageHistoryRepository _packageHistoryRepository;
        private readonly IFranchiseeTenantRepository _franchiseeTenantRepository;

        public PackageHistoryService(ITenantPersistenceService tenantPersistenceService, IPackageHistoryRepository packageHistoryRepository, IFranchiseeTenantRepository franchiseeTenantRepository,
            IBusinessRuleSet<PackageHistory> businessRuleSet = null)
            : base(packageHistoryRepository, packageHistoryRepository, tenantPersistenceService, businessRuleSet)
        {
            _packageHistoryRepository = packageHistoryRepository;
            _franchiseeTenantRepository = franchiseeTenantRepository;
        }

        public bool AddPackageHistory(AddPackageHistoryDto packageHistoryInfo)
        {
            var franchisee = _franchiseeTenantRepository.FirstOrDefault(
               o => o.Name == packageHistoryInfo.FranchiseeName && o.LicenseKey == packageHistoryInfo.LicenseKey);
            var packageHistory = new PackageHistory();

            using (var scope = new TransactionScope())
            {

                packageHistory.PackageId = packageHistoryInfo.PackageId;
                packageHistory.OldPackageId = packageHistoryInfo.OldPackageId;
                packageHistory.StartDate = packageHistoryInfo.StartDate;
                packageHistory.EndDate = packageHistoryInfo.EndDate;
                packageHistory.RequestId = packageHistoryInfo.RequestId;
                packageHistory.FranchiseeTenantId = packageHistoryInfo.FranchiseeTenantId;
                packageHistory.AccountNumber = packageHistoryInfo.AccountNumber;
                packageHistory.IsApply = packageHistoryInfo.IsApply;
                Add(packageHistory);

                if (!packageHistoryInfo.IsApply)
                {
                    franchisee.EndActiveDate = packageHistoryInfo.StartDate;
                }
                if (packageHistoryInfo.IsApply)
                {
                    franchisee.CurrentPackageId = packageHistoryInfo.PackageId;
                    franchisee.EndActiveDate = packageHistoryInfo.EndDate;                   
                }

                franchisee.RemainingAmount = packageHistoryInfo.Amount;
                franchisee.NextBillingDate = packageHistoryInfo.NextBillingDate;
                franchisee.PackageNextBillingDate = packageHistoryInfo.PackageNextBillingDate;
                franchisee.StartDateSuccess = null;
                franchisee.EndDateSuccess = null;
                _franchiseeTenantRepository.Update(franchisee);
                _franchiseeTenantRepository.Commit();

                scope.Complete();
                return true;
            }
        }

        public PackageHistoryDto GetPackageCurrent(FranchisseNameAndLicenseDto franchiseeData)
        {
            var franchisee = _franchiseeTenantRepository.FirstOrDefault(
                   o => o.Name == franchiseeData.FranchiseeName && o.LicenseKey == franchiseeData.LicenseKey);
            return _packageHistoryRepository.GetPackageCurrent(franchisee.Id);
        }

        public List<PackageHistoryDto> GetListPackageChange(FranchisseNameAndLicenseDto franchiseeData)
        {
            var franchisee = _franchiseeTenantRepository.FirstOrDefault(
                   o => o.Name == franchiseeData.FranchiseeName && o.LicenseKey == franchiseeData.LicenseKey);
            return _packageHistoryRepository.GetListPackageChange(franchisee.Id);
        }
        public int GetRequestCurrentId(FranchisseNameAndLicenseDto franchiseeData)
        {
            var franchisee = _franchiseeTenantRepository.FirstOrDefault(
                   o => o.Name == franchiseeData.FranchiseeName && o.LicenseKey == franchiseeData.LicenseKey);
            return _packageHistoryRepository.GetCurrentRequestId(franchisee.Id);
        }


        public PackageHistoryDto GetPackageCurrentNoToken(FranchisseNameAndLicenseDto franchiseeData)
        {
            var franchisee = _franchiseeTenantRepository.FirstOrDefault(
                   o => o.Name == franchiseeData.FranchiseeName && o.LicenseKey == franchiseeData.LicenseKey);
            return _packageHistoryRepository.GetPackageCurrent(franchisee.Id);
        }
        public bool AddPackageHistoryNoToken(PackageHistoryDto packageHistoryInfo)
        {
            var packageHistory = new PackageHistory();

            using (var scope = new TransactionScope())
            {

                var franchiseeTenant = _franchiseeTenantRepository.GetById(packageHistoryInfo.FranchiseeTenantId);

                packageHistory.PackageId = packageHistoryInfo.PackageId;
                packageHistory.OldPackageId = packageHistoryInfo.OldPackageId;
                packageHistory.StartDate = packageHistoryInfo.StartDate;
                packageHistory.EndDate = packageHistoryInfo.EndDate;
                packageHistory.RequestId = packageHistoryInfo.RequestId;
                packageHistory.FranchiseeTenantId = packageHistoryInfo.FranchiseeTenantId;
                packageHistory.AccountNumber = franchiseeTenant.AccountNumber;
                packageHistory.IsApply = packageHistoryInfo.IsApply;
                Add(packageHistory);


                franchiseeTenant.CurrentPackageId = packageHistory.PackageId;
                franchiseeTenant.RemainingAmount = packageHistoryInfo.Amount;
                franchiseeTenant.NextBillingDate = packageHistoryInfo.NextBillingDate;
                franchiseeTenant.PackageNextBillingDate = packageHistoryInfo.PackageNextBillingDate;

                _franchiseeTenantRepository.Update(franchiseeTenant);
                _franchiseeTenantRepository.Commit();
                scope.Complete();
                return true;
            }
        }
    }
}
