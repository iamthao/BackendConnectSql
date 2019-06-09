using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DomainModel.Entities;
using Repositories.Interfaces;
using Database.Persistance.Tenants;
using Framework.DomainModel.ValueObject;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.Entities.Common;

namespace Repositories
{
    public class EntityFrameworkCountryOrRegionRepository : EntityFrameworkTenantRepositoryBase<CountryOrRegion>, ICountryOrRegionRepository
    {
        public EntityFrameworkCountryOrRegionRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {
            SearchColumns.Add("Name");
            DisplayColumnForCombobox = "Name";
        }

        public override IQueryable<ReadOnlyGridVo> BuildQueryToGetDataForGrid(IQueryInfo queryInfo)
        {
            var queryResult = (from entity in GetAll()                           
                               select new { entity }).Select(s => new CountryOrRegionGridVo
                               {
                                   Id = s.entity.Id,
                                   Name = s.entity.Name

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

        public List<LookupItemVo> GetAllCountryOrRegionForLookUp()
        {
            var data = GetAll().Select(s => new LookupItemVo()
            {
                KeyId = s.Id,
                DisplayName = s.Name,
            }).ToList();
            //data.Insert(0, new LookupItemVo()
            //{
            //    KeyId = 0,
            //    DisplayName = "Select Country / Region"
            //});
            return data;
        }
    }
}
