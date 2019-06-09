using System;
using System.Linq;
using Database.Persistance.Tenants;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;
using Repositories.Interfaces;
using System.Linq.Dynamic;
namespace Repositories
{
    public class EntityFrameworkCustomerRepository : EntityFrameworkTenantRepositoryBase<Customer>, ICustomerRepository
    {
        public EntityFrameworkCustomerRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {
            SearchColumns.Add("Name");
            DisplayColumnForCombobox = "Name";
        }

        public override IQueryable<ReadOnlyGridVo> BuildQueryToGetDataForGrid(IQueryInfo queryInfo)
        {
            var queryResult = (from entity in GetAll()
                select new {entity}).OrderBy(queryInfo.SortString).Select(s => new CustomerGridVo
                {
                    Id = s.entity.Id,
                    Name = s.entity.Name

                });

            return queryResult;
        }
    }
}