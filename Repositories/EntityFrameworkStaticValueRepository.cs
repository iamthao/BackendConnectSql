using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Persistance.Tenants;
using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Repositories.Interfaces;

namespace Repositories
{
    public class EntityFrameworkStaticValueRepository : EntityFrameworkTenantRepositoryBase<StaticValue>, IStaticValueRepository
    {
        public EntityFrameworkStaticValueRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {

        }

        public int GetNewRequestNumber()
        {
            var total = 0;
            var newRequestNumber = GetDataFromStoredProcedure<StaticValueGridVo>(GetConnectionString(), "udsGetNewRequestNumber",null,ref total);
            if (newRequestNumber != null)
            {
                var staticValueGridVo = newRequestNumber.FirstOrDefault();
                if (staticValueGridVo != null) return staticValueGridVo.RequestNumber;
            }
            return 0;
        }
        public CheckSumChange CheckChangeTable()
        {
            var total = 0;
            var checkSumRequest = GetDataFromStoredProcedure<CheckSumChange>(GetConnectionString(), "udsCheckChangeTable", null, ref total);
            if (checkSumRequest != null)
            {
                return checkSumRequest.FirstOrDefault();
            }
            return null;
        }
    }
}
