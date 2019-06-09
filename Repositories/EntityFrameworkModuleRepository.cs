using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Security.Cryptography;
using Database.Persistance.Tenants;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;
using Repositories.Interfaces;

namespace Repositories
{
    public class EntityFrameworkModuleRepository : EntityFrameworkTenantRepositoryBase<Module>, IModuleRepository
    {
        public EntityFrameworkModuleRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {
            SearchColumns.Add("Name");
            DisplayColumnForCombobox = "Name";
        }

        public override IQueryable<ReadOnlyGridVo> BuildQueryToGetDataForGrid(IQueryInfo queryInfo)
        {
            var queryResult = (from entity in GetAll()
                select new { entity }).Select(s => new ModuleGridVo()
                {
                    Id = s.entity.Id,
                    Name = s.entity.Name
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

        public IList<ModuleDocumentTypeOperationGridVo> GetModuleDocumentTypeOperationsByModuleId(int moduleId)
        {
            return GetById(moduleId).ModuleDocumentTypeOperations.Select(p => new ModuleDocumentTypeOperationGridVo()
            {
                Id = p.Id,
                Module = p.Module.Name,
                DocumentType = p.DocumentType.Name,
                SercurityOperation = p.SercurityOperationId
            }).ToList();
        }
    }
}