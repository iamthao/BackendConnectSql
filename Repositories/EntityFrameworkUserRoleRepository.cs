using System.Configuration;
using System.Data.Entity;
using Database.Persistance.Tenants;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Data.SqlClient;

namespace Repositories
{
    public class EntityFrameworkUserRoleRepository : EntityFrameworkTenantRepositoryBase<UserRole>, IUserRoleRepository
    {
        public EntityFrameworkUserRoleRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {
            SearchColumns.Add("Name");
            DisplayColumnForCombobox = "Name";
        }

        public override IQueryable<ReadOnlyGridVo> BuildQueryToGetDataForGrid(IQueryInfo queryInfo)
        {
            // Show all user who is not admin
            var queryResult = (from entity in GetAll()
                               select new { entity }).OrderBy(queryInfo.SortString).Select(s => new UserRoleGridVo
                {
                    Id = s.entity.Id,
                    Name = String.IsNullOrEmpty(s.entity.Name) ? "" : s.entity.Name,
                    AppRoleName = String.IsNullOrEmpty(s.entity.AppRoleName) ? "" : s.entity.AppRoleName,
                });
            return queryResult;
        }

        protected override void BuildSortExpression(IQueryInfo queryInfo)
        {
            if (queryInfo.Sort == null || queryInfo.Sort.Count == 0)
            {
                queryInfo.Sort = new List<Sort> { new Sort { Field = "Name", Dir = "asc" } };
            }
            queryInfo.Sort.ForEach(x =>
            {
                x.Field = String.Format("entity.{0}", x.Field);
            });
        }

        public dynamic GetRoleFunction(int idRole)
        {
            var deploymentMode = ConfigurationManager.AppSettings["DeploymentMode"];
            var objListResult = new List<UserRoleFunctionGridVo>();
            var objListDocumentType =
                PersistenceService.CurrentWorkspace.Context.DocumentTypes.Where(
                    o => o.Type.Equals(deploymentMode, StringComparison.CurrentCultureIgnoreCase))
                    .AsNoTracking()
                    .OrderBy(o => o.Order)
                    .ThenByDescending(o => o.Id)
                    .ToList();
            var objListUserRoleFunction = new List<UserRoleFunction>();
            if (idRole > 0)
            {
                objListUserRoleFunction = PersistenceService.CurrentWorkspace.Context.UserRoleFunctions.AsNoTracking().Where(o => o.UserRoleId == idRole).ToList();
            }
            foreach (var documentType in objListDocumentType)
            {
                var objAdd = new UserRoleFunctionGridVo { Id = documentType.Id, Name = documentType.Title };
                // Check for isView
                var isView =
                    objListUserRoleFunction.Any(
                        o => o.SecurityOperationId == (int)OperationAction.View && o.DocumentTypeId == documentType.Id);
                objAdd.IsView = isView;
                // Check for isDelete
                var isDelete =
                    objListUserRoleFunction.Any(
                        o => o.SecurityOperationId == (int)OperationAction.Delete && o.DocumentTypeId == documentType.Id);
                objAdd.IsDelete = isDelete;
                // Check for isUpdate
                var isUpdate =
                    objListUserRoleFunction.Any(
                        o => o.SecurityOperationId == (int)OperationAction.Update && o.DocumentTypeId == documentType.Id);
                objAdd.IsUpdate = isUpdate;
                // Check for isAdd
                var isAdd =
                    objListUserRoleFunction.Any(
                        o => o.SecurityOperationId == (int)OperationAction.Add && o.DocumentTypeId == documentType.Id);
                objAdd.IsInsert = isAdd;
                // Check for isProcess
                var isProcess =
                    objListUserRoleFunction.Any(
                        o => o.SecurityOperationId == (int)OperationAction.Process && o.DocumentTypeId == documentType.Id);
                objAdd.IsProcess = isProcess;
                objListResult.Add(objAdd);
            }
            return new { Data = objListResult, TotalRowCount = objListResult.Count };
        }

        public IEnumerable<int> GetAllDocumentTypeId()
        {
            return PersistenceService.CurrentWorkspace.Context.DocumentTypes.Select(o => o.Id).ToList();
        }

        public List<DocumentType> GetAllDocumentType()
        {
            return PersistenceService.CurrentWorkspace.Context.DocumentTypes.ToList();
        }

        public List<LookupItemVo> GetUserRoleNoCourier()
        {
            var data = GetAll().Select(s => new LookupItemVo()
            {
                KeyId = s.Id,
                DisplayName = s.Name,
            }).Where(o=>o.KeyId!=2).ToList();
            data.Insert(0, new LookupItemVo()
            {
                KeyId = 0,
                DisplayName = "Select Role"
            });
            return data;
        }

       
    }
}