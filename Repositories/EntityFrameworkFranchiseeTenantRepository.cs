using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Util;
using ConfigValues;
using Database.Persistance.Tenants;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;
using Framework.Utility;
using Repositories.Interfaces;

namespace Repositories
{
    public class EntityFrameworkFranchiseeTenantRepository : EntityFrameworkTenantRepositoryBase<FranchiseeTenant>, IFranchiseeTenantRepository
    {
        public EntityFrameworkFranchiseeTenantRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {
            var deploymentMode = ConfigurationManager.AppSettings["DeploymentMode"];
            if (deploymentMode == "camino")
            {
                persistenceService.CreateWorkspace(new Tenant
                {
                    Name = ConstantValue.ConnectionStringAdminDb
                });
            }
            SearchColumns.Add("Name");
            DisplayColumnForCombobox = "Name";
        }

        public override IQueryable<ReadOnlyGridVo> BuildQueryToGetDataForGrid(IQueryInfo queryInfo)
        {
            var queryResult = (from entity in GetAll()
                               select new { entity }).Select(s => new FranchiseeTenantGridVo()
                               {
                                   Id = s.entity.Id,
                                   Name = s.entity.Name,
                                   IsActive = s.entity.IsActive
                               }).OrderBy(queryInfo.SortString);
            return queryResult;
        }

        protected override void BuildSortExpression(IQueryInfo queryInfo)
        {
            if (queryInfo.Sort == null || queryInfo.Sort.Count == 0)
            {
                queryInfo.Sort = new List<Sort> { new Sort { Field = "Id", Dir = "desc" } };
            }
            queryInfo.Sort.ForEach(x =>
            {
                if (x.Field == "Name")
                {
                    x.Field = "Name";
                }
            });
        }

        public ModuleForFranchiseeDto GetModuleForFranchisee(FranchisseNameAndLicenseDto franchiseeData)
        {
            var franchisee =
                GetAll()
                    .FirstOrDefault(
                        o => o.Name == franchiseeData.FranchiseeName && o.LicenseKey == franchiseeData.LicenseKey);
            if (franchisee == null)
            {
                return null;
            }
            var listModule = from o in franchisee.FranchiseeModule
                             select new { ModuleId=o.ModuleId, ListModuleOperation=o.Module.ModuleDocumentTypeOperations};
            var objResult = new ModuleForFranchiseeDto
            {
                NumberOfCourier = franchisee.NumberOfCourier,
                ListModuleId = new List<int>(),
                ListModuleDocumentTypeOperations=new List<ModuleDocumentTypeOperationDto>()
            };
            foreach (var item in listModule)
            {
                objResult.ListModuleId.Add(item.ModuleId);
                foreach (var moduleOper in item.ListModuleOperation)
                {
                    var objAdd = new ModuleDocumentTypeOperationDto
                    {
                        DocumentTypeId = moduleOper.DocumentTypeId,
                        ModuleId = moduleOper.ModuleId,
                        OperationId = moduleOper.SercurityOperationId
                    };
                    objResult.ListModuleDocumentTypeOperations.Add(objAdd);
                }
            }
            return objResult;
        }
        public ActiveDateLicenseKeyDto GetActiveDateLicenseKey(FranchisseNameAndLicenseDto franchiseeData)
        {
            var franchisee =
                GetAll()
                    .FirstOrDefault(
                        o => o.Name == franchiseeData.FranchiseeName && o.LicenseKey == franchiseeData.LicenseKey);
            if (franchisee == null)
            {
                return null;
            }
            var objResult = new ActiveDateLicenseKeyDto
            {
                StartActiveDate = franchisee.StartActiveDate,
                EndActiveDate = franchisee.EndActiveDate
            };
            return objResult;
        }
        public List<LookupItemVo> GetListIndustry()
        {
            return TenantPersistenceService.CurrentWorkspace.Context.Industries.Select(p => new LookupItemVo()
            {
                KeyId = p.Id,
                DisplayName = p.Name
            }).ToList();
        }
        //get info franchisee
        public FranchiseeTernantDto GetInfoFranchisee(FranchisseNameAndLicenseDto franchiseeData)
        {
            var franchisee =
                GetAll()
                    .FirstOrDefault(
                        o => o.Name == franchiseeData.FranchiseeName && o.LicenseKey == franchiseeData.LicenseKey);
            if (franchisee == null)
            {
                return null;
            }
            var objResult = new FranchiseeTernantDto
            {
                Id = franchisee.Id,
                FranchiseeId = franchisee.Id.ToString(),
                Name = franchisee.Name,
                Address1 = franchisee.Address1,
                Address2 = franchisee.Address2,
                City = franchisee.City,
                Zip = franchisee.Zip,
                OfficePhone = franchisee.OfficePhone,
                FaxNumber = franchisee.FaxNumber,
                IndustryId = franchisee.IndustryId,
                NumberOfCourier = franchisee.NumberOfCourier,
                StartActiveDate = franchisee.StartActiveDate,
                EndActiveDate = franchisee.EndActiveDate,
                AccountNumber = franchisee.AccountNumber ,
                CloseDate = franchisee.CloseDate,
                Amount = franchisee.RemainingAmount,
                NextBillingDate = franchisee.NextBillingDate,
                PackageNextBillingDate = franchisee.PackageNextBillingDate,
                AlertExtendedPackage = franchisee.AlertExtendedPackage
            };
            return objResult;
        }
    }
}