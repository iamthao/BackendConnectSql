using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Database.Persistance.Tenants;
using Framework.DomainModel.Entities;
using Repositories.Interfaces;

namespace Repositories
{
    public class EntityFrameworkUserRoleFunctionRepository : EntityFrameworkTenantRepositoryBase<UserRoleFunction>, IUserRoleFunctionRepository
    {

        public EntityFrameworkUserRoleFunctionRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {

        }

        public List<UserRoleFunction> LoadUserSecurityRoleFunction(int userRoleId, int documentTypeId)
        {
            var result = (from urf in PersistenceService.CurrentWorkspace.Context.UserRoleFunctions.AsQueryable().AsNoTracking()
                          join document in PersistenceService.CurrentWorkspace.Context.DocumentTypes.AsQueryable().AsNoTracking()
                    on urf.DocumentTypeId equals document.Id into temp
                          from docType in temp
                          where docType.Id == documentTypeId
                          where urf.UserRoleId == userRoleId
                          select urf);
            return result.ToList();
        }


    }
}