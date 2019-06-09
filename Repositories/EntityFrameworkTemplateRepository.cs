using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using Database.Persistance.Tenants;
using Framework.DomainModel.Common;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;
using Repositories.Interfaces;

namespace Repositories
{
    public class EntityFrameworkTemplateRepository : EntityFrameworkTenantRepositoryBase<Template>, ITemplateRepository
    {
        public EntityFrameworkTemplateRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {
            SearchColumns.Add("Title");
            DisplayColumnForCombobox = "Title";
        }

        public override IQueryable<ReadOnlyGridVo> BuildQueryToGetDataForGrid(IQueryInfo queryInfo)
        {
            var queryResult = (from entity in GetAll()
                               select new { entity }).OrderBy(queryInfo.SortString).Select(s => new TemplateGridVo
                               {
                                   Id = s.entity.Id,
                                   Title = s.entity.Title,
                                   TemplateTypeId = (int)s.entity.TemplateType
                               });
            return queryResult;
        }

        public Template GetTemplateInfo(int id)
        {
            return GetAll()
                .FirstOrDefault(o => o.Id == id);
        }
        protected override string BuildLookupCondition(LookupQuery query)
        {
            var where = new StringBuilder();
            @where.Append("(");
            var innerWhere = new List<string>();
            var queryDisplayName = String.Format("Title.Contains(\"{0}\")", query.Query);
            innerWhere.Add(queryDisplayName);
            @where.Append(String.Join(" OR ", innerWhere.ToArray()));
            @where.Append(")");
            if (query.HierachyItems != null)
            {
                foreach (var parentItem in query.HierachyItems.Where(parentItem => parentItem.Value != string.Empty && parentItem.Value != "-1"
                                                                                    && parentItem.Value != "0" && !parentItem.IgnoredFilter))
                {
                    var filterValue = parentItem.Value.Replace(",", string.Format(" OR {0} = ", parentItem.Name));
                    @where.Append(string.Format(" AND ( {0} = {1})", parentItem.Name, filterValue));
                }
            }
            return @where.ToString();
        }
       
    }
}
