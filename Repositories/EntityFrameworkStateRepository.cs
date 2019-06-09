using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using Database.Persistance.Tenants;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;
using Repositories.Interfaces;

namespace Repositories
{
    public class EntityFrameworkStateRepository : EntityFrameworkTenantRepositoryBase<State>, IStateRepository
    {
        public EntityFrameworkStateRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {
            SearchColumns.Add("Name");
            DisplayColumnForCombobox = "Name";
        }

        public override IQueryable<ReadOnlyGridVo> BuildQueryToGetDataForGrid(IQueryInfo queryInfo)
        {
            var queryResult = GetAll().OrderBy(queryInfo.SortString).Select(s => new StateGridVo
            {
                Id = s.Id,
                Name = s.Name,
                AbbreviationName = s.AbbreviationName,
            });
            return queryResult;
        }

        protected override void BuildSortExpression(IQueryInfo queryInfo)
        {
            if (queryInfo.Sort == null || (queryInfo.Sort != null && queryInfo.Sort.Count == 0))
            {
                queryInfo.Sort = new List<Sort> { new Sort { Field = "Name", Dir = "asc" } };
            }
        }

        public List<LookupItemVo> GetAllStateForLookUp()
        {
            var data = GetAll().Select(s => new LookupItemVo()
            {
                KeyId = s.Id,
                DisplayName = s.Name,
            }).ToList();
            data.Insert(0, new LookupItemVo()
            {
                KeyId = 0,
                DisplayName = "Select state"
            });
            return data;
        }
    }
}
