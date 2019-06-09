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
    public class EntityFrameworkRegisterRepository : EntityFrameworkTenantRepositoryBase<Register>, IRegisterRepository
    {
        public EntityFrameworkRegisterRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {
        }

    }
}